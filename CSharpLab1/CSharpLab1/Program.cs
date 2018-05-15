using System;
using System.Collections.Generic;

namespace CSharplab1
{
    class Program
    {
        private static int Nmax = 10, N_St = Nmax * (Nmax - 1) / 2;

        private static int[,] A;
        private static int[,] A_Eiler;
        private static int[] Stack = new int[0];
        private static int[][] Ribs;

        static void Main(string[] args)
        {
            Init();
            FindTree(A_Eiler);
            PrintMatrix();
            FindWay(0);
            OutPut();
            Console.ReadKey();
        }

        private static void Init()
        {
            Console.WriteLine("Input number of abonents");
            Nmax = Convert.ToInt32(Console.ReadLine());
            N_St = Nmax * (Nmax - 1) / 2;
            A = new int[Nmax, Nmax];
            A_Eiler = new int[Nmax, Nmax];
            Ribs = new int[Nmax - 1][];
            Console.WriteLine("1 - Generate matrix\n2 - Input matrix");
            int choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 2) InputMatrix(Nmax, A);
            else GenerateMatrix(Nmax, A);
        }

        private static void InputMatrix(int size, int[,] A)
        {
            for (int i = 0; i < size; i++)
                for (int j = i + 1; j < size; j++)
                {
                    Console.WriteLine("Enter a payment between x{0} and x{1}", i + 1, j + 1);
                    A[i, j] = Convert.ToInt32(Console.ReadLine());
                    A[j, i] = A[i, j];
                }
        }

        private static void GenerateMatrix(int size, int[,] A)
        {
            Random random = new Random();
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < i; j++) A[i, j] = A[j, i];
                int maxCon = size - i - 1;
                int totalCon = random.Next(0, maxCon);
                int[] conIndexses = new int[totalCon];
                for (int j = 0; j < totalCon; j++)
                    while (true)
                    {
                        int index = random.Next(i + 1, size);
                        if (Array.IndexOf(conIndexses, index) != -1) continue;
                        conIndexses[j] = index;
                        break;
                    }
                foreach (int index in conIndexses) A[i, index] = random.Next(1, 100);
                if (i != size - 1 && A[i, i + 1] == 0) A[i, i + 1] = random.Next(1, 100);
            }
        }

        private static void FindTree(int[,] A_Eiler)
        {
            ISet<int> Sp = new HashSet<int>();
            int min = 100;
            int l = 0, t = 0;
            for (int i = 0; i < Nmax - 1; i++)
                for (int j = 1; j < Nmax; j++)
                    if ((A[i, j] < min) && (A[i, j] != 0))
                    {
                        min = A[i, j];
                        l = i;
                        t = j;
                    }
            A_Eiler[l, t] = A[l, t];
            A_Eiler[t, l] = A[t, l];
            Sp.Add(l + 1);
            Sp.Add(t + 1);
            int ribIndex = 0;
            Ribs[ribIndex] = new int[2];
            Ribs[ribIndex][0] = l + 1;
            Ribs[ribIndex][1] = t + 1;
            ribIndex++;
            System.Collections.ArrayList tArray = new System.Collections.ArrayList();
            for (int i = 1; i <= Nmax; i++)
                tArray.Add(i);
            int[] array = new int[tArray.Count];
            for (int i = 0; i < array.Length; i++)
                array[i] = int.Parse(tArray[i].ToString());

            while (!Contains(array, Sp))
            {
                min = 100;
                l = 0; t = 0;
                for (int i = 0; i < Nmax; i++)
                    if (Sp.Contains(i + 1))
                        for (int j = 0; j < Nmax; j++)
                            if (!Sp.Contains(j + 1) && (A[i, j] < min) && (A[i, j] != 0))
                            {
                                min = A[i, j];
                                l = i;
                                t = j;
                            }
                A_Eiler[l, t] = A[l, t];
                A_Eiler[t, l] = A[t, l];
                Sp.Add(l + 1);
                Sp.Add(t + 1);

                Ribs[ribIndex] = new int[2];
                Ribs[ribIndex][0] = l + 1;
                Ribs[ribIndex][1] = t + 1;
                ribIndex++;
            }
        }

        public static bool Contains(int[] array, ISet<int> set)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (!set.Contains(array[i]))
                    return (false);
            }
            return (true);
        }


        private static void FindWay(int v)
        {
            for (int i = 0; i < Nmax; i++)
                if (A_Eiler[v, i] != 0)
                {
                    A_Eiler[v, i] = 0;
                    FindWay(i);
                }
            int[] temp = (int[])Stack.Clone();
            Stack = new int[Stack.Length + 1];
            for (int i = 0; i < temp.Length; i++)
                Stack[i] = temp[i];
            Stack[Stack.Length - 1] = v + 1;
        }

        private static void OutPut()
        {
            ISet<int> Way = new HashSet<int>();
            int i, pred_v, Cost = 0;
            Console.Write("Path: ");
            Console.Write(" ");
            Console.Write("{0} ", Stack[0]);
            Way.Add(Stack[0]);
            pred_v = Stack[0];
            for (i = 1; i < Stack.Length; i++)
                if (!Way.Contains(Stack[i]))
                {
                    Console.Write("{0} ", Stack[i]);
                    Way.Add(Stack[i]);
                    Cost += A[pred_v - 1, Stack[i] - 1];
                    pred_v = Stack[i];
                }
            Console.WriteLine("{0} ", Stack[0]);
            Cost += A[pred_v - 1, Stack[0] - 1];
            Console.Write("Cost of the path: ");
            Console.Write("{0}", Cost);
        }

        private static void PrintMatrix()
        {
            for (int i = 0; i < Nmax; i++) Console.Write($"\t[{i + 1}]");
            Console.WriteLine();
            for (int i = 0; i < Nmax; i++)
            {
                Console.Write($"[{i + 1}]\t");
                for (int j = 0; j < Nmax; j++) Console.Write($"{A[i, j]}\t");
                Console.WriteLine();
            }
        }
    }
}