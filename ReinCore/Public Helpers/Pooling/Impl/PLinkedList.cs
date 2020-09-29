namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    public class LinkList<T>
    {
        private static Node GetNewNode()
        {
            var node = Pool<Node, NodeInit, NodeClean>.item;
            return node;
        }


        public class Node
        {
            internal void Assign()
            {

            }
            internal Node next;
            internal Node prev;
            internal LinkList<T> list;

        }

        private struct NodeInit : IInitItem<Node>
        {
            public Node InitItem() => new();
        }
        private struct NodeClean : ICleanItem<Node>
        {
            public void CleanItem(Node item)
            {
                throw new NotImplementedException();
            }
        }
    }
}
