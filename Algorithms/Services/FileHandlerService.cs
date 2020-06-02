﻿using System.IO;
using System.Reflection;
using Algorithms.Models;
using System.Collections.Generic;
using Algorithms.Views;

namespace Algorithms.Services
{
    public class FileHandlerService
    {
        public string GetReleventText(AlgorithmNameEnumeration alg)
        {
            string _filename = "";
            switch (alg)
            {
                case AlgorithmNameEnumeration.BubbleSort:
                    _filename = "Algorithms.AlgorithmInfomation.BubbleSortInfo.txt";
                    break;
                case AlgorithmNameEnumeration.HeapSort:
                    _filename = "Algorithms.AlgorithmInfomation.HeapSortInfo.txt";
                    break;
                case AlgorithmNameEnumeration.InsertionSort:
                    _filename = "Algorithms.AlgorithmInfomation.InsertionSortInfo.txt";
                    break;
                case AlgorithmNameEnumeration.QuickSort:
                    _filename = "Algorithms.AlgorithmInfomation.QuickSortInfo.txt";
                    break;
                case AlgorithmNameEnumeration.ClassicBinarySearch:
                    _filename = "Algorithms.AlgorithmInfomation.ClassicBinarySearchInfo.txt";
                    break;
                case AlgorithmNameEnumeration.ModBinarySearch:
                    _filename = "Algorithms.AlgorithmInfomation.ModifiedBinarySearchInfo.txt";
                    break;
                case AlgorithmNameEnumeration.LinearSearch:
                    _filename = "Algorithms.AlgorithmInfomation.LinearSearchInfo.txt";
                    break;
                case AlgorithmNameEnumeration.JumpSearch:
                    _filename = "Algorithms.AlgorithmInfomation.JumpSearchInfo.txt";
                    break;
            }
            if (_filename != "")
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(SelectedAlgorithmPage)).Assembly;
                Stream stream = assembly.GetManifestResourceStream(_filename);

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            return null;
        }

        public string[] GetAllImplementations(List<string> TextArr)
        {
            List<string> implementations = new List<string>();
            for (int i = 0; i < TextArr.Count - 1; i++)
            {
                // C# Implementation
                if (TextArr[i] == "C#")
                {
                    string CSharpImp = "";
                    for (int j = i + 1; j < TextArr.Count - 1; j++)
                    {
                        if (TextArr[j] == "Java")
                        {
                            break;
                        }
                        CSharpImp += TextArr[j];
                        CSharpImp += "\n";
                    }
                    implementations.Add(CSharpImp.TrimEnd());
                }
                // Java Implementation
                if (TextArr[i] == "Java")
                {
                    string JavaImp = "";
                    for (int j = i + 1; j < TextArr.Count - 1; j++)
                    {
                        if (TextArr[j] == "Python")
                        {
                            break;
                        }
                        JavaImp += TextArr[j];
                        JavaImp += "\n";
                    }
                    implementations.Add(JavaImp.TrimEnd());
                }
                // Python Implementation
                if (TextArr[i] == "Python")
                {
                    string PythonImp = "";
                    for (int j = i + 1; j < TextArr.Count - 1; j++)
                    {
                        PythonImp += TextArr[j];
                        PythonImp += "\n";
                    }
                    implementations.Add(PythonImp.TrimEnd());
                }
            }
            return implementations.ToArray();
        }
    }
}
