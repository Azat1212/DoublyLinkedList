using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

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

            using (var sr = new StreamReader(fileName))
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
                Console.WriteLine(i);
                Console.WriteLine(GetNodeId(node.Random));
                Console.WriteLine(node.Data);

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
            var nodesDictionary = new Dictionary<ListNode, int>();
            sw.WriteLine(Count);

            //Заполнение словоря для рандома
            var node = Head;
            for (var i = 0; i < Count; i++)
            {
                nodesDictionary.Add(node, i);
                node = node.Next;
            }
            //Заполнение файла
            node = Head;
            for (var i = 0; i < Count; i++)
            {
                sw.WriteLine(i);
                sw.WriteLine(nodesDictionary[node.Random]);
                sw.WriteLine(Regex.Escape(node.Data));
                node = node.Next;
            }
        }
        public void Deserialize(StreamReader sr)
        {
            var nodesDictionary = new Dictionary<string, ListNode>();
            var count = int.Parse(sr.ReadLine());
            var randomIds = new string[count];

            //Чтение из файла и создание списка
            for (var i = 0; i < count; i++)
            {
                var id = sr.ReadLine();
                randomIds[i] = sr.ReadLine();
                nodesDictionary[id] = new ListNode() { Data = sr.ReadLine()};

                Count++;
                
                if (Count > 1)
                {
                    Tail.Next = nodesDictionary[id];
                    nodesDictionary[id].Previous = Tail;
                }
                else
                {
                    Head = nodesDictionary[id];
                }

                Tail = nodesDictionary[id];
            }

            //Проставление ссылок на поле Random
            var node = Head;
            for (int i = 0; i < Count; i++)
            {
                node.Random = nodesDictionary[randomIds[i]];
                node = node.Next;
            }
        }
    }
}
