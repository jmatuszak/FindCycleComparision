using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

public class MainClass
{
    public static void Main()
    {
        var ListOfCyclesDfs = new List<List<int>>();
        var ListOfCyclesMultiply = new List<List<int>>();
        var ListOfCyclesNaive = new List<List<int>>();
        
        for(int i = 1; i < 20; i++)
        {
            int n = 10 * i;
            var matrix = GenerateMatrix(n);
            int[,] matrixCopy = matrix.Clone() as int[,];


            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            var multipliedMatrix = MultiplyMatrix(matrix);
            ListOfCyclesMultiply = FindCycleMultiply(multipliedMatrix, matrix);
            watch.Stop();
            Console.Write(n+";"+watch.ElapsedMilliseconds+";"+ListOfCyclesMultiply.Count+";");

            watch.Start();
            ListOfCyclesNaive = FindCycleNaive(matrixCopy);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds+";"+ListOfCyclesNaive.Count);

        }
 
        //Naive
        List<List<int>> FindCycleNaive(int[,] matrix)
        {
            List<List<int>> ListOfCycles= new List<List<int>>();
            var n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            if (matrix[j, k] != 0 && matrix[k, i] != 0)
                            {
                                var C3= new List<int>() { i,j,k };
                                if(!CheckIfContainsCycle(ListOfCycles, C3))
                                    ListOfCycles.Add(C3);
                            }
                        }
                    }

                }
            }
            return ListOfCycles;
        }
        //Multiply
        List<List<int>> FindCycleMultiply(int[,] multipliedMatrix, int[,] matrix)
        {
            List<List<int>> ListOfCycles = new List<List<int>>();
            List<int> C3 = new List<int>();
            int count = 0;
            var n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    for (int k = 0; k < n; k++)
                    {
                        if (multipliedMatrix[i, j] >= 1 && multipliedMatrix[j, k] >= 1 && multipliedMatrix[k, i] >= 1 &&
                            i != k && k != j && i != j &&
                            matrix[i, j] == 1 && matrix[j, k] == 1 && matrix[k, i] == 1)
                        {
                            count++;
                            C3 = new List<int>() { i, j, k };
                            if (!CheckIfContainsCycle(ListOfCycles, C3))
                            {
                                ListOfCycles.Add(C3);
                            }
                        }
                    }
                }
            }
            return ListOfCycles;
        }

/*
        //Dfs
        List<List<int>> FindCycleDfs(int[,] matrix, int v, int w, List<int> Stack, List<int> Visited, List<List<int>> ListOfCycles)
        {
            var C3 = new List<int>();

            //oznaczamy bieżący wierzchołek jako odwiedzony
            if (!Visited.Contains(w)) Visited.Add(w);
            //na stosie umieszczamy bieżący wierzchołek
            Stack.Add(w);
            //przeglądamy kolejnych sąsiadów wierzchołka w
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (matrix[i, w] >= 1 && i != w)
                {
                    //znaleźliśmy cykl>2, kończymy rekurencję
                    if (i == v && Visited.Count() > 2)
                    {
                        C3 = new List<int>() { i, v, w };
                        if (!CheckIfContainsCycle(ListOfCycles, C3) && i != w && i != v && v != w)
                        {
                            ListOfCycles.Add(C3);
                            return ListOfCycles;
                        }
                    }
                    if (!Visited.Contains(i))
                    {

                        if (FindCycleDfs(matrix, v, i, Stack, Visited, ListOfCycles).Count >= ListOfCycles.Count)
                        {
                            ListOfCycles = FindCycleDfs(matrix, v, i, Stack, Visited, ListOfCycles);
                            C3 = new List<int>() { i, v, w };
                            if (!CheckIfContainsCycle(ListOfCycles, C3) && i != w && i != v && v != w)
                            {
                                ListOfCycles.Add(C3);
                                return ListOfCycles;
                            }
                        }
                    }
                }
                Stack.Remove(Stack.Count - 1);
            }
            return ListOfCycles;
        }*/
        bool CheckIfContainsCycle(List<List<int>> ListOfCycles, List<int> C3)
        {
            for (int i = 0; i < ListOfCycles.Count; i++)
            {
                if (ListOfCycles.ElementAt(i).Contains(C3.ElementAt(0)) &&
                    ListOfCycles.ElementAt(i).Contains(C3.ElementAt(1)) &&
                    ListOfCycles.ElementAt(i).Contains(C3.ElementAt(2)))
                    return true;
            }
            return false;
        }

        int[,] ReadMatrix()
        {
            var line = Console.ReadLine();

            var trimmed = line.Split(' ');
            int n = trimmed.Length;
            int[,] matrix = new int[n, n];
            for (int j = 0; j < n; j++)
            {
                matrix[0, j] = int.Parse(trimmed[j]);
            }
            for (int i = 1; i < n; i++)
            {
                line = Console.ReadLine();
                trimmed = line.Split(' ');
                for (int j = 0; j < n; j++)
                {
                    matrix[i, j] = int.Parse(trimmed[j]);
                }

            }
            return matrix;
        }
        void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {

                    Console.Write(matrix[i, j]);
                    if (j < matrix.GetLength(1) - 1) Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
        int[,] ReadFromFile(string txt)
        {
            String input = File.ReadAllText(txt);
            int n = input.Count(x=>x=='\n')+1;
            int i = 0, j = 0;
            int[,] result = new int[n, n];
            foreach (var row in input.Split('\n'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(' '))
                {
                    result[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }
            return result;
        }
        int[,] MultiplyMatrix(int[,] matrix)
        {
            var n = matrix.GetLength(0);
            var result = new int[n, n];

            for (int h = 0; h < n; h++)
            {
                for (int i = 0; i < n; i++)
                {
                    var sum = 0;
                    for (int j = 0; j < n; j++)
                    {
                        sum += matrix[h, j] * matrix[j, i];
                    }
                    result[h, i] = sum;
                }
            }
            return result;
        }
        int[,] GenerateMatrix(int n)
        {
            var random = new Random();
            int[,] matrix = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j) matrix[i, j] = 0;
                    else
                    {
                        matrix[i, j] = random.Next(0, 2);
                    }
                }
            }
            //PrintMatrix(matrix);
            return matrix;
        }
    }
}