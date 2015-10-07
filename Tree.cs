using System;
using System.Collections.IEnumerable;

namespace Project
{
    public class Tree
    {
        public Tree parent;

        public Tree()
        {
            parent = null;
        }

        public Tree root()
        {
            if (parent == null)
            {
                return this;
            }
            else
            {
                return parent.root();
            }
        }

        public bool isConnected(Tree tree)
        {
            return this.root() == tree.root();
        }

        public void connect(Tree tree)
        {
            tree.root().parent = this;
        }

    }
}
