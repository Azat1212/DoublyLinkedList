using System;
using System.IO;
using System.Linq;

namespace DoublyLinkedList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //// Get the directories currently on the C drive.
            //DirectoryInfo[] cDirs = new DirectoryInfo(@"c:\").GetDirectories();

            //// Write each directory name to a file.
            //using (StreamWriter sw = new StreamWriter("CDriveDirs.txt"))
            //{
            //    foreach (DirectoryInfo dir in cDirs)
            //    {
            //        sw.WriteLine(dir.Name);
            //    }
            //}

            //// Read and show each line from the file.
            //string line = "";
            //using (StreamReader sr = new StreamReader("CDriveDirs.txt"))
            //{
            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        Console.WriteLine(line);
            //    }
            //}

            //listRandom.Add("50");

            //listRandom.Add("51");

            //listRandom.Add("52");
            //listRandom.Add("53");
            //listRandom.Add("54");
            //listRandom.Add("55");
            //listRandom.Add("56");
            //listRandom.Add("57");
            //listRandom.Add("58");
            //listRandom.Add("59");

            var listRandom = new ListRandom();

            var datas = "60 61 62 63 64 65 66 67 68 69".Split(' ');
            foreach(var data in datas)
            {
                listRandom.Add(data);
            }

            listRandom.MarkAllRandom();
            //listRandom.PrintData(listRandom.Head);



            string fileName = "ListRandom.txt";
            //fileName = Path.Combine(Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + @"\..\..\"), fileName);

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

        public void FillRandomById(int id)
        {
            var node = Head;

            for (int i = 0; i < id; i++)
            {
                node = node.Next;
            }

            node.Random = GetListNodeById(id);
        }

        public ListNode GetListNodeById(int id)
        {
            var node = Head;

            for (int i = 0; i < Count; i++)
            {
                if (id == i)
                {
                    break; 
                }                
                node = node.Next;
            }
            return node;
        }

        //public void PrintData(ListNode node)
        //{
        //    if (node == null)
        //        node = Head;

        //    Console.WriteLine(node.Data);

        //    if (Tail == node)
        //        return;

        //    PrintData(node.Next);
        //}
        
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

            foreach(var data in datas)
            {
                node.Random = GetListNodeById(int.Parse(data[2]));
                node = node.Next;
            }
        }
    }


}
