///**********************************************************///
/// Licensed under the Unity Companion License               ///
/// https://unity.com/legal/licenses/unity-companion-license ///
///**********************************************************///

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.Text;

namespace ZBase.Foundation.SourceGen
{
    using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    public static partial class SourceGenHelpers
    {
        public const string TRACKED_NODE_ANNOTATION_USED_BY_ROSLYN = "Id";

        private static string s_projectPath = string.Empty;

        private static bool InUnity2021OrNewer { get; set; }

        public static string ProjectPath
        {
            get
            {
                if (string.IsNullOrEmpty(s_projectPath))
                {
                    throw new Exception(
                        "ProjectPath must set before use, this is also only permitted before 2020."
                    );
                }

                return s_projectPath;
            }

            private set => s_projectPath = value;
        }

        public static bool CanWriteToProjectPath => !string.IsNullOrEmpty(s_projectPath);

        public static void Setup(GeneratorExecutionContext context)
        {
            // needs to be disabled for e.g. Sonarqube static code analysis (which also uses analyzers)
            if (Environment.GetEnvironmentVariable("SOURCEGEN_DISABLE_PROJECT_PATH_OUTPUT") == "1")
            {
                return;
            }

            var isDotsRuntime = context.ParseOptions.PreprocessorSymbolNames.Contains("UNITY_DOTSRUNTIME");
            InUnity2021OrNewer = context.ParseOptions.PreprocessorSymbolNames.Contains("UNITY_2021_1_OR_NEWER");

            if (context.AdditionalFiles.Any() == false
                || string.IsNullOrEmpty(context.AdditionalFiles[0].Path)
            )
            {
                return;
            }

            ProjectPath = InUnity2021OrNewer && isDotsRuntime == false
                ? context.AdditionalFiles[0].GetText().ToString()
                : context.AdditionalFiles[0].Path;
        }

        private static string GetTempGeneratedPathToFile(string fileNameWithExtension)
        {
            if (CanWriteToProjectPath == false)
            {
                return Path.Combine("Temp", "GeneratedCode");
            }

            var tempFileDirectory = Path.Combine(ProjectPath, "Temp", "GeneratedCode");
            Directory.CreateDirectory(tempFileDirectory);
            return Path.Combine(tempFileDirectory, fileNameWithExtension);
        }

        public static SyntaxList<AttributeListSyntax> GetCompilerGeneratedAttribute()
            => AttributeListFromAttributeName("global::System.Runtime.CompilerServices.CompilerGenerated");

        private static SyntaxList<AttributeListSyntax> AttributeListFromAttributeName(string attributeName)
            => new SyntaxList<AttributeListSyntax>(
                AttributeList(SingletonSeparatedList(Attribute(IdentifierName(attributeName))))
            );

        public static void LogInfo(string message)
        {
            if (CanWriteToProjectPath == false)
            {
                return;
            }

            // Ignore IO exceptions in case there is already a lock,
            // could use a named mutex but don't want to eat the performance cost
            try
            {
                using StreamWriter w = File.AppendText(GetTempGeneratedPathToFile("SourceGen.log"));
                w.WriteLine(message);
            }
            catch (IOException) { }
        }

        public static class CompilerError
        {
            public static string WithMessage(string errorMessage)
                => "This error indicates a bug in the ZBase.Foundation source generators. " +
                "We'd appreciate a bug report. Thanks! " +
                $"Error message: '{errorMessage}'";
        }

        public static void LogError(
              this GeneratorExecutionContext context
            , string errorCode
            , string title
            , string errorMessage
            , Location location
            , string description = ""
        )
        {
            if (errorCode.Contains("ICE"))
            {
                errorMessage = CompilerError.WithMessage(errorMessage);
            }

            context.Log(DiagnosticSeverity.Error, errorCode, title, errorMessage, location, description);
        }

        private static void LogInfo(
              this GeneratorExecutionContext context
            , string errorCode
            , string title
            , string errorMessage
            , Location location
            , string description = ""
        )
        {
            context.Log(
                  DiagnosticSeverity.Info
                , errorCode
                , title
                , errorMessage
                , location
                , description
            );
        }

        private static void Log(
              this GeneratorExecutionContext context
            , DiagnosticSeverity diagnosticSeverity
            , string errorCode
            , string title
            , string errorMessage
            , Location location
            , string description = ""
        )
        {
            LogInfo($"{diagnosticSeverity}: {errorCode}, {title}, {errorMessage}");

            var rule = new DiagnosticDescriptor(
                  errorCode
                , title
                , errorMessage
                , "Source Generator"
                , diagnosticSeverity
                , true
                , description
            );

            context.ReportDiagnostic(Diagnostic.Create(rule, location));
        }

        public static bool TryParseQualifiedEnumValue<TEnum>(string value, out TEnum result)
            where TEnum : struct
        {
            var unqualifiedEnumValue = value.Split('.').Last();
            return Enum.TryParse(unqualifiedEnumValue, out result)
                && Enum.IsDefined(typeof(TEnum), result);
        }

        public static IEnumerable<Enum> GetFlags(this Enum e)
            => Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag);

        public static SourceText WithInitialLineDirectiveToGeneratedSource(
              this SourceText sourceText
            , string generatedSourceFilePath
        )
        {
            var firstLine = sourceText.Lines.FirstOrDefault();

            return sourceText.WithChanges(new TextChange(firstLine.Span
                , $"#line 1 \"{generatedSourceFilePath}\"" + Environment.NewLine + firstLine
            ));
        }

        public static SourceText WithIgnoreUnassignedVariableWarning(this SourceText sourceText)
        {
            var firstLine = sourceText.Lines.FirstOrDefault();
            return sourceText.WithChanges(new TextChange(firstLine.Span
                , $"#pragma warning disable 0219" + Environment.NewLine + firstLine
            ));
        }

        // Stable version of String.GetHashCode
        public static int GetStableHashCode(string str)
        {
            unchecked
            {
                var hash1 = 5381;
                var hash2 = hash1;

                for (var i = 0; i < str.Length && str[i] != '\0'; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1 || str[i + 1] == '\0')
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }

        public static void OutputSourceToFile(GeneratorExecutionContext context, string generatedSourceFilePath, SourceText sourceTextForNewClass)
        {
            // Output as generated source file for debugging/inspection
            if (CanWriteToProjectPath == false)
            {
                return;
            }

            try
            {
                LogInfo($"Outputting generated source to file {generatedSourceFilePath}...");
                File.WriteAllText(generatedSourceFilePath, sourceTextForNewClass.ToString());
            }
            catch (IOException ioException)
            {
                // emit exceptions as info but don't block compilation or generate error to fail tests
                context.LogInfo(
                      "SGICE005", "ZBase.Foundation Generators"
                    , ioException.ToUnityPrintableString()
                    , context.Compilation.SyntaxTrees.First().GetRoot().GetLocation()
                );
            }
        }

        /// <summary>
        ///         
        /// Returns true if running as part of csc.exe, otherwise we are likely running in the IDE.
        /// Skipping Source Generation in the IDE can be a considerable performance win as source
        /// generators can be run multiple times per keystroke. If the user doesn't rely on generated types
        /// consider skipping your Generator's Execute method when this returns false
        /// </summary>
        public static readonly bool IsBuildTime = Assembly.GetEntryAssembly() != null;

        public static bool ShouldRun(GeneratorExecutionContext context)
        {
            // Throw if we are cancelled
            context.CancellationToken.ThrowIfCancellationRequested();

            // Don't run if we don't reference ZBase.Foundation (or are ZBase.Foundation)
            // or if we are CodeGen.Tests (which need to run generators manually)
            return (context.Compilation.Assembly.Name == "ZBase.Foundation" ||
                    context.Compilation.ReferencedAssemblyNames.Any(n => n.Name == "ZBase.Foundation")) &&
                   !context.Compilation.Assembly.Name.Contains("CodeGen.Tests");
        }
    }
}
