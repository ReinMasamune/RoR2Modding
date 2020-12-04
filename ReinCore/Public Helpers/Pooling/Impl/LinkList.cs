namespace ReinCore
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;


    public static class LinkListPool<T>
    {
        public static LinkList<T> item
        {
            get => Pool<LinkList<T>, Init, Clean>.item;
            set => Pool<LinkList<T>, Init, Clean>.item = value;
        }


        internal struct Init : IInitItem<LinkList<T>>
        {
            public LinkList<T> InitItem()
            {
                return new();
            }
        }
        internal struct Clean : ICleanItem<LinkList<T>>
        {
            public void CleanItem(LinkList<T> item)
            {
                item.Clear();
            }
        }
    }


    public static class LinkListXtn
    {
        //Contact point
        public static LinkList<T>.Node InsertAfter<T>(this LinkList<T> list, LinkList<T>.Node node, T item)
        {
            var newNode = list.CreateNode(item);
            node.InsertAfter(newNode);
            return newNode;
        }

        //Contact point
        public static LinkList<T>.Node InsertBefore<T>(this LinkList<T> list, LinkList<T>.Node node, T item)
        {
            var newNode = list.CreateNode(item);
            node.InsertBefore(newNode);
            return newNode;
        }

        //Contact point
        public static void Clear<T>(this LinkList<T> list)
        {
            while(list.first is not null) list.first.Remove();
        }

        //Partial contact point
        public static LinkList<T>.Node Append<T>(this LinkList<T> list, T item)
        {
            if(list.last is null)
            {
                list.last = list.first = list.CreateNode(item);
            } else
            {
                list.last.InsertAfter(item);
            }
            return list.last;
        }
        //Partial contact point
        public static LinkList<T>.Node Prepend<T>(this LinkList<T> list, T item)
        {
            if(list.first is null)
            {
                list.first = list.last = list.CreateNode(item);
            } else
            {
                list.first.InsertBefore(item);
            }
            return list.first;
        }



        public static LinkList<T>.Node InsertAfter<T>(this LinkList<T>.Node node, T item) => node.list.InsertAfter(node, item);

        public static LinkList<T>.Node InsertBefore<T>(this LinkList<T>.Node node, T item) => node.list.InsertBefore(node, item);


        public static void InsertAfter<T>(this LinkList<T> list, LinkList<T>.Node node, params T[] items)
        {
            for(Int32 i = 0; i < items.Length; ++i)
            {
                node = node.InsertAfter<T>(items[i]);
            }
        }

        public static void InsertBefore<T>(this LinkList<T> list, LinkList<T>.Node node, params T[] items)
        {
            for(Int32 i = items.Length - 1; i >= 0; --i)
            {
                node = node.InsertBefore<T>(items[i]);
            }
        }

        public static void InsertAfter<T, TEnumerator>(this LinkList<T> list, LinkList<T>.Node node, TEnumerator items)
            where TEnumerator : IEnumerator<T>
        {
            while(items.MoveNext()) node = node.InsertAfter(items.Current);
        }

        public static void InsertBefore<T, TEnumerator>(this LinkList<T> list, LinkList<T>.Node node, TEnumerator items)
            where TEnumerator : IEnumerator<T>
        {
            while(items.MoveNext()) node = node.InsertBefore(items.Current);
        }

        public static void InsertAfter<T>(this LinkList<T> list, LinkList<T>.Node node, IEnumerable<T> items)
        {
            list.InsertAfter(node, items.GetEnumerator());
        }

        public static void InsertBefore<T>(this LinkList<T> list, LinkList<T>.Node node, IEnumerable<T> items)
        {
            list.InsertBefore(node, items.GetEnumerator());
        }

        public static void InsertAfter<T, TEnumerator>(this LinkList<T>.Node node, TEnumerator items)
            where TEnumerator : IEnumerator<T>
        {
            node.list.InsertAfter(node, items);
        }

        public static void InsertBefore<T, TEnumerator>(this LinkList<T>.Node node, TEnumerator items)
            where TEnumerator : IEnumerator<T>
        {
            node.list.InsertBefore(node, items);
        }

        public static void InsertAfter<T>(this LinkList<T>.Node node, IEnumerable<T> items)
        {
            node.list.InsertAfter(node, items);
        }

        public static void InsertBefore<T>(this LinkList<T>.Node node, IEnumerable<T> items)
        {
            node.list.InsertBefore(node, items);
        }
    }

    public class LinkList<T>
    {
        internal Node first;
        internal Node last;
        internal Int32 count;



        public T this[Node node]
        {
            get
            {
                if(node?.list != this) throw new ArgumentException(nameof(node));
                return node.item;
            }
            set
            {
                if(node?.list != this) throw new ArgumentException(nameof(node));
                node.item = value;
            }
        }

        internal Node CreateNode(T item)
        {
            var node = GetNewNode();
            node.list = this;
            node.item = item;
            return node;
        }

        internal Node _CreateNode(Node after, Node before)
        {
            var node = GetNewNode();
            node.list = this;
            node.Assign(after, before);
            
            return node;
        }

        internal Node _CreateNode(Node after, Node before, T item)
        {
            var node = GetNewNode();
            node.list = this;
            node.Assign(after, before);
            node.item = item;
            return node;
        }
        internal void DestroyNode(Node item) => ReturnNode(item);


        private static Node GetNewNode()
        {
            var node = Pool<Node, NodeInit, NodeClean>.item;
            return node;
        }
        private static void ReturnNode(Node item)
        {
            Pool<Node, NodeInit, NodeClean>.item = item;
        }

        public class Node
        {
            internal void Assign(Node prev, Node next)
            {
                this.prev = prev;
                this.next = next;
            }
            //private Node _next;
            //private Node _prev;
            internal LinkList<T> list;

            public Node next { get; internal set; }
            public Node prev { get; internal set; }


            internal void InsertAfter(Node inserted)
            {
                this.list.count++;
                if(this.next is null)
                {
                    this.list.last = inserted;
                } else
                {
                    this.next.prev = inserted;
                    inserted.next = this.next;
                }
                this.next = inserted;
                inserted.prev = this;
            }
            internal void InsertBefore(Node inserted)
            {
                this.list.count++;
                if(this.prev is null)
                {
                    this.list.first = inserted;
                } else
                {
                    this.prev.next = inserted;
                    inserted.prev = this.prev;
                }
                inserted.next = this;
                this.prev = inserted;
            }
            internal void Remove()
            {
                this.list.count--;
                if(this.prev is null)
                {
                    this.list.first = this.next;
                } else
                {
                    this.prev.next = this.next;
                }

                if(this.next is null)
                {
                    this.list.last = this.prev;
                } else
                {
                    this.next.prev = this.prev;
                }

                ReturnNode(this);
            }

            internal T item;
        }
        private struct NodeInit : IInitItem<Node>
        {
            public Node InitItem() => new();
        }
        private struct NodeClean : ICleanItem<Node>
        {
            public void CleanItem(Node item)
            {
                item.Assign(null, null);
                item.list = null;
                item.item = default;
            }
        }
    }
}
//public struct LinkListEnumerator : IEnumerator<Node>, IEnumerator<T>
//{
//    internal LinkListEnumerator(LinkList<T> list)
//    {
//        this.list = list;
//        this.Current = list.first;
//    }
//    private readonly LinkList<T> list;
//    public Node Current { get; private set; }
//    Object IEnumerator.Current => this.Current;
//    T IEnumerator<T>.Current => this.list[this.Current];
//    public void Dispose() { }
//    public Boolean MoveNext() => (this.Current = this.Current.next) is not null;
//    public void Reset() => this.Current = list.first;
//}

//public interface INodeItemType { }
//public interface INodeItemType<TItem> : INodeItemType
//{
//    internal TItem item { get; set; }
//}

//public interface INodeListType { }
//public interface INodeListType<TList> : INodeListType
//    where TList : ILinkList, ILinkListSelfType<TList>
//{
//    internal TList list { get; set; }
//}

//public interface INodeSelfType { }
//public interface INodeSelfType<TSelf> : INodeSelfType
//    where TSelf : INodeSelfType<TSelf>
//{
//    TSelf prev { get; internal set; }
//    TSelf next { get; internal set; }
//}

//public interface INode { }

//public interface INode<TItem, TSelf, TList> : INode, INodeItemType<TItem>, INodeListType<TList>, INodeSelfType<TSelf>
//    where TList : ILinkListSelfType<TList>
//{

//}



//public interface ILinkListCount
//{
//    Int32 count { get; internal set; }
//}

//public interface ILinkListNodeType { }
//public interface ILinkListNodeType<TNode> : ILinkListNodeType
//{
//    TNode first { get; internal set; }
//    TNode last { get; internal set; }

//    internal TNode CreateNode();
//    internal void DestroyNode(TNode node);
//}

//public interface ILinkListItemType { }
//public interface ILinkListItemType<TItem> : ILinkListItemType { }

//public interface ILinkListSelfType { }
//public interface ILinkListSelfType<TSelf> : ILinkListSelfType { }

//public interface ILinkListNodeAndItemType { }
//public interface ILinkListNodeAndItemType<TNode, TItem> : ILinkListNodeAndItemType, ILinkListNodeType<TNode>, ILinkListItemType<TItem>
//{
//    TItem this[TNode node] { get; set; }
//}

//public interface ILinkList : ILinkListCount, ILinkListNodeType, ILinkListItemType, ILinkListSelfType, ILinkListNodeAndItemType { }
//public interface ILinkList<TNode, TItem, TSelf> : ILinkList, ILinkListNodeAndItemType<TNode, TItem>, ILinkList



//    public interface ILinkList<T, TSelf, TNode> : ILinkListCount, ILinkListNodeType<TNode>
//        where TSelf : ILinkList<T, TSelf, TNode>
//        where TNode : ILinkList<T, TSelf, TNode>.INode
//{





//}