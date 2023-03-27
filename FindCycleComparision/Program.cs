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
            int[,] matrix1 = matrix.Clone() as int[,];
            int[,] matrix2 = matrix.Clone() as int[,];
            //PrintMatrix(matrixCopy);

            var watch = new System.Diagnostics.Stopwatch();

            watch.Start();
            ListOfCyclesNaive = FindCycleNaive(matrix);
            watch.Stop();
            Console.Write(n+";"+watch.ElapsedMilliseconds+";"+ListOfCyclesNaive.Count+";");



            watch.Start();
            ListOfCyclesDfs = FindCycleDfs(matrix1);
            watch.Stop();
            Console.Write(watch.ElapsedMilliseconds+";"+ListOfCyclesDfs.Count + ";");

           
            watch.Start();
            var multipliedMatrix = MultiplyMatrix(matrix2);
            ListOfCyclesMultiply = FindCycleMultiply(multipliedMatrix, matrix);
            watch.Stop();
            Console.WriteLine(";"+watch.ElapsedMilliseconds+";"+ListOfCyclesMultiply.Count+";");
            

        }

        //Naive
        List<List<int>> FindCycleNaive(int[,] matrix)
        {
            List<List<int>> ListOfCycles = new List<List<int>>();
            var n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j) break;
                    if (matrix[i, j] != 0)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            //if (i == k || j == k) break;
                            if (matrix[j, k] != 0 && matrix[k, i] != 0)
                            {
                                if (CheckIfContainsCycle(ListOfCycles, i, j, k) == false)
                                    ListOfCycles.Add(new List<int> { i, j, k });
                            }
                        }
                    }

                }
            }
            return ListOfCycles;
        }

        //DFS
        List<List<int>> FindCycleDfs(int[,] matrix)
        {

            List<List<int>> ListOfCycles = new List<List<int>>();
            //DfsOrder.Add(start);
            var n = matrix.GetLength(0);
            for(int start = 0; start < n; start++)
            {
                List<int> DfsOrder = new List<int>();
                Stack<int> DfsStack = new Stack<int>();
                DfsStack.Push(start);
                while (DfsStack.Count > 0)
                {
                    int v = DfsStack.Pop();
                    if (!DfsOrder.Contains(v))
                    {
                        for (int i = n - 1; i >= 0; i--)
                        {
                            if (matrix[v, i] >= 1)
                            {
                                DfsStack.Push(i);
                                for (int j = 0; j < n; j++)
                                {
                                    if (matrix[i, j] >= 1 && matrix[j, v] >= 1)
                                        if (!CheckIfContainsCycle(ListOfCycles, i, j, v))
                                            ListOfCycles.Add(new List<int> { i, j, v });
                                }
                            }
                        }
                        DfsOrder.Add(v);
                    }
                }
            }

            return ListOfCycles;
        }

        //Multiply
        List<List<int>> FindCycleMultiply(int[,] multipliedMatrix, int[,]matrix)
{
            var ListOfCycles = new List<List<int>>();
            var n = matrix.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j) break;
                    if (multipliedMatrix[i, j] >= 1 || multipliedMatrix[j, i] >= 1)
                    {
                        for (int k = 0; k < n; k++)
                        {
                            if (i == k || j == k) break;
                            if ((matrix[i, k] >= 1 && matrix[k, j] == 1 && matrix[j, i] >= 1)|| 
                                (matrix[k, i] >= 1 && matrix[j, k] == 1 && matrix[i, j] >= 1))
                            {

                                if (!CheckIfContainsCycle(ListOfCycles, i, j, k))
                                    ListOfCycles.Add(new List<int> { i, j, k });
                            }
                        }
                    }

                }
            }
            return ListOfCycles;
        }


        bool CheckIfContainsCycle(List<List<int>> ListOfCycles, int i, int j, int k)
        {
            if (ListOfCycles.Count == 0) return false;
            else
            {
                for (int c = 0; c < ListOfCycles.Count; c++)
                {
                    if (ListOfCycles[c].Contains(i) &&
                        ListOfCycles[c].Contains(j) &&
                        ListOfCycles[c].Contains(k))
                    {
                        return true;
                    }
                }
                return false;
            }
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
            var numbers = new int[4] { 0, 0, 0, 1, };
            var random = new Random();
            int[,] matrix = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j) matrix[i, j] = 0;
                    else
                    {
                        matrix[i, j] = numbers[random.Next(0, 4)];
                    }
                }
            }
            //PrintMatrix(matrix);
            return matrix;
        }
    }
}