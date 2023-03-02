///**********************************************************///
/// Licensed under the Unity Companion License               ///
/// https://unity.com/legal/licenses/unity-companion-license ///
///**********************************************************///

using System;

namespace ZBase.Foundation.SourceGen
{
    public static class ExceptionExtensions
    {
        public static string ToUnityPrintableString(this Exception exception)
            => exception.ToString().Replace(Environment.NewLine, " |--| ");
    }
}
