using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ZBase.Foundation.SourceGen.Enums
{
    [Generator]
    public class EnumGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
            => context.RegisterForSyntaxNotifications(() => new EnumSyntaxReceiver(context.CancellationToken));

        public void Execute(GeneratorExecutionContext context)
        {
            if (!SourceGenHelpers.IsBuildTime || !SourceGenHelpers.ShouldRun(context))
                return;

            SourceGenHelpers.Setup(context);

            // TODO: Disabled running parallel for now so we can shake out race conditions.
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = 1 };

            SourceGenHelpers.LogInfo($"Source generating assembly {context.Compilation.Assembly.Name}...");

            var stopwatch = Stopwatch.StartNew();
        }
    }
}
