using System;
using System.IO;

namespace DoublyLinkedList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var listRandom = new ListRandom();

            var datas = "60 61 62 63 64 65 66 67 68 69".Split(' ');
            foreach (var data in datas)
            {
                listRandom.Add(data);
            }

            listRandom.MarkAllRandom();

            string fileName = "ListRandom.txt";

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                listRandom.Serialize(sw);
            }

            var newListRandom = new ListRandom();

            using (StreamReader sr = new StreamReader(fileName))
            {
                newListRandom.Deserialize(sr);
            }

            newListRandom.PrintData();

        }
    }
    class ListNode
    {
        public ListNode Previous;
        public ListNode Next;
        public ListNode Random; // произвольный элемент внутри списка
        public string Data;
    }


    class ListRandom
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Add(string data)
        {
            var node = new ListNode() { Data = data };
            Count++;

            if (Count == 1)
            {
                Head = node;
                Tail = node;
                return;
            }

            Tail.Next = node;
            node.Previous = Tail;

            Tail = node;
        }

        public void MarkAllRandom()
        {
            var node = Head;

            for (int i = 0; i < Count; i++)
            {
                node.Random = GetListNodeById(new Random().Next(Count));
                node = node.Next;
            }
        }

        public ListNode GetListNodeById(int id)
        {
            var node = Head;

            for (int i = 0; i < id; i++)
            {
                node = node.Next;
            }

            return node;
        }

        public void PrintData()
        {
            var node = Head;

            for (var i = 0; i < Count; i++)
            {
                Console.WriteLine(i + " " + node.Data + " " + GetNodeId(node.Random));

                if (Tail == node)
                    return;

                node = node.Next;
            }
        }

        private int GetNodeId(ListNode node)
        {
            var tmpNode = Head;
            int count = 0;

            for (var i = 0; i < Count; i++)
            {
                if (tmpNode == node)
                {
                    count = i;
                    break;
                }
                tmpNode = tmpNode.Next;
            }
            return count;
        }

        public void Serialize(StreamWriter sw)
        {
            var node = Head;

            sw.WriteLine(Count);

            for (var i = 0; i < Count; i++)
            {
                sw.WriteLine(i + " " + node.Data + " " + GetNodeId(node.Random));

                if (Tail == node)
                    return;

                node = node.Next;
            }
        }

        public void Deserialize(StreamReader sr)
        {
            var count = int.Parse(sr.ReadLine());
            string[][] datas = new string[count][];

            for (var i = 0; i < count; i++)
            {
                datas[i] = sr.ReadLine().Split(' ');
            }

            foreach (var data in datas)
            {
                this.Add(data[1]);
            }

            var node = Head;

            foreach (var data in datas)
            {
                node.Random = GetListNodeById(int.Parse(data[2]));
                node = node.Next;
            }
        }
    }
}
