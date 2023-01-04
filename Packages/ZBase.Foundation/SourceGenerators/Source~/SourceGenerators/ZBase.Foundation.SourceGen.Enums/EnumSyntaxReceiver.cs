using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace ZBase.Foundation.SourceGen.Enums
{
    public class EnumSyntaxReceiver : ISyntaxReceiver
    {
        private readonly CancellationToken _cancelToken;

        public EnumSyntaxReceiver(CancellationToken cancelToken)
        {
            _cancelToken = cancelToken;
        }

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
        }
    }
}
