using System;
using JetBrains.Annotations;

namespace Microsoft.Qwiq
{
    internal class AreaOrIterationComparer : GenericComparer<IWorkItemClassificationNode>
    {
        internal new static readonly AreaOrIterationComparer Default = Nested.Instance;

        private AreaOrIterationComparer()
        {
        }

        public override bool Equals(IWorkItemClassificationNode x, IWorkItemClassificationNode y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;

            return x.Id == y.Id
                   && x.IsAreaNode == y.IsAreaNode
                   && x.IsIterationNode == y.IsIterationNode
                   && string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(x.Path, y.Path, StringComparison.OrdinalIgnoreCase);
        }

        public override int GetHashCode([CanBeNull] IWorkItemClassificationNode obj)
        {
            if (ReferenceEquals(obj, null)) return 0;

            unchecked
            {
                var hash = 27;

                hash = (hash * 13) ^ obj.Id;
                hash = (hash * 13) ^ obj.IsAreaNode.GetHashCode();
                hash = (hash * 13) ^ obj.IsIterationNode.GetHashCode();
                hash = (hash * 13) ^ (obj.Name != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name) : 0);
                hash = (hash * 13) ^ (obj.Path != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Path) : 0);

                return hash;
            }
        }

        private class Nested
        {
            // ReSharper disable MemberHidesStaticFromOuterClass
            internal static readonly AreaOrIterationComparer Instance = new AreaOrIterationComparer();

            // ReSharper restore MemberHidesStaticFromOuterClass

            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
            static Nested()
            {
            }
        }
    }
}