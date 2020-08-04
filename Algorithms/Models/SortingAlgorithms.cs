using System.Collections.Generic;
using System.Linq;
using Algorithms.Models.SortAlgorithmOperations;
using Microcharts;

namespace Algorithms.Models
{
    public class SortingAlgorithms
    {
        public IEnumerable<SortOperation> BubbleSort(Entry[] entries)
        {
            List<SortOperation> operations = new List<SortOperation>();
            int n = entries.Count();

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1 ; j++)
                {
                    // check for swap
                    if (entries[j].Value > entries[j+1].Value)
                    {
                        Entry[] EntriesToChange = new Entry[]
                        {
                            entries[j],
                            entries[j + 1]
                        };
                        // swap
                        Entry temp = entries[j];
                        entries[j] = entries[j + 1];
                        entries[j + 1] = temp;
                        // add swap operation
                        operations.Add(new SortOperation
                        {
                            EntriesToChange = EntriesToChange,
                            NewEntries = entries.Select(hr => hr).ToArray(),
                            ChangeToColour = "#FFFF00"
                        });
                    }
                }
            }
            return operations;
        }

        public IEnumerable<SortOperation> HeapSort(Entry[] entries)
        {
            List<SortOperation> HSOperations = new List<SortOperation>();
            Entry[] tings = entries.Select(hr => hr).ToArray();
            int n = entries.Length;
            // Build heap (rearrange array) 
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(entries, n, i, HSOperations);
            }
            // One by one extract an element from heap 
            for (int i = n - 1; i > 0; i--)
            {
                Entry[] entries1 = new Entry[]{
                    entries.Select(hr => hr).ToArray()[0],
                    entries.Select(hr => hr).ToArray()[i]
                };
                // Move current root to end
                Entry temp = entries[0];
                entries[0] = entries[i];
                entries[i] = temp;
                HSOperations.Add(new SortOperation
                {
                    EntriesToChange = entries1,
                    NewEntries = entries.Select(hr => hr).ToArray(),
                    ChangeToColour = "#FFFF00"
                });

                // call max heapify on the reduced heap 
                Heapify(entries, i, 0, HSOperations);
            }
            return HSOperations.Select(hr => hr);
        }

        private void Heapify(Entry[] entries, int n, int i, List<SortOperation> HSOperations)
        {
            int largest = i; // Initialize largest as root
            int left = 2 * i + 1; // left = 2*i + 1
            int right = 2 * i + 2; // right = 2*i + 2

            // If left child is larger than root 
            if (left < n &&
                entries[left].Value > entries[largest].Value)
            {
                largest = left;
            }   

            // If right child is larger than largest so far 
            if (right < n &&
                entries[right].Value > entries[largest].Value)
            {
                largest = right;
            }

            // If largest is not root 
            if (largest != i)
            {
                Entry[] entries1 = new Entry[]{
                    entries.Select(hr => hr).ToArray()[i],
                    entries.Select(hr => hr).ToArray()[largest]
                };

                Entry temp = entries[i];
                entries[i] = entries[largest];
                entries[largest] = temp;

                HSOperations.Add(new SortOperation
                {
                    EntriesToChange = entries1,
                    NewEntries = entries.Select(hr => hr).ToArray(),
                    ChangeToColour = "#FFFF00"
                });

                // Recursively heapify the affected sub-tree 
                Heapify(entries, n, largest, HSOperations);
            }
        }

        public IEnumerable<SortOperation> InsertionSort(Entry[] entries)
        {
            List<SortOperation> operations = new List<SortOperation>();
            int n = entries.Length;
            for(int i = 0; i < n; i++)
            {
                Entry currentEntry = entries[i];
                int pos = i;

                operations.Add(new SortOperation
                {
                    EntriesToChange = new Entry[]
                    {
                        entries.Select(hr => hr).ToArray()[i]
                    },
                    NewEntries = entries.Select(hr => hr).ToArray(),
                    ChangeToColour = "#FFFF00",
                    IsSwitchOperation = true
                });

                while (pos > 0 && entries[pos-1].Value > currentEntry.Value)
                {
                    entries[pos] = entries[pos-1];
                    pos--;
                }
                entries[pos] = currentEntry;
                operations.Add(new SortOperation
                {
                    EntriesToChange = new Entry[]
                    {
                        entries.Select(hr => hr).ToArray()[pos]
                    },
                    NewEntries = entries.Select(hr => hr).ToArray(),
                    ChangeToColour = "#FFFF00",
                    IsSwitchOperation = true
                });
            }
            return operations;
        }

        public IEnumerable<SortOperation> SelectionSort(Entry[] entries)
        {
            List<SortOperation> operations = new List<SortOperation>();
            int n = entries.Length;
            for (int i = 0; i < n - 1; i++)
            {
                operations.Add(new SortOperation
                {
                    EntriesToChange = new Entry[]
                    {
                        entries.Select(hr => hr).ToArray()[i]
                    },
                    NewEntries = entries.Select(hr => hr).ToArray(),
                    ChangeToColour = "#00FFFF",
                    IsSwitchOperation = false
                });

                // Find the minimum element in unsorted array 
                int min_index = i;
                for (int j = i + 1; j < n; j++)
                {
                    if (entries[j].Value < entries[min_index].Value)
                    {
                        min_index = j;
                    }
                }
                
                Entry[] EntriesToChange = new Entry[]
                {
                    entries.Select(hr => hr).ToArray()[min_index],
                    entries.Select(hr => hr).ToArray()[i]
                };
                // Swap the found minimum element with the first 
                // element 
                Entry temp = entries[min_index];
                entries[min_index] = entries[i];
                entries[i] = temp;
                // add operation
                operations.Add(new SortOperation
                {
                    EntriesToChange = EntriesToChange,
                    NewEntries = entries.Select(hr => hr).ToArray(),
                    ChangeToColour = "#FFFF00",
                    IsSwitchOperation = true
                });
            }
            return operations;
        }

        public List<SortOperation> QuickSort(Entry[] entries, int start, int end, List<SortOperation> QSOperations)
        {
            int pivot;
            if (start < end)
            {
                pivot = Partition(entries, start, end, QSOperations);
                if (pivot > 1)
                {
                    QuickSort(entries, start, pivot - 1, QSOperations);
                }
                if (pivot + 1 < end)
                {
                    QuickSort(entries, pivot + 1, end, QSOperations);
                }
            }
            return QSOperations.Select(hr => hr).ToList();
        }

        private int Partition(Entry[] entries, int left, int right, List<SortOperation> QSOperations)
        {
            int pivot = (int)entries[left].Value;
            
            QSOperations.Add(new SortOperation
            {
                EntriesToChange = new Entry[]
                {
                    entries.Select(hr => hr).ToArray()[left]
                },
                NewEntries = entries.Select(hr => hr).ToArray(),
                ChangeToColour = "#00FFFF",
                IsSwitchOperation = false
            });

            while (true)
            {
                while (entries[left].Value < pivot)
                {
                    left++;
                }
                while (entries[right].Value > pivot)
                {
                    right--;
                }
                if (left < right)
                {
                    Entry[] EntriesToChange = new Entry[]
                    {
                        entries.Select(hr => hr).ToArray()[right],
                        entries.Select(hr => hr).ToArray()[left]
                    };
                    Entry temp = entries[right];
                    entries[right] = entries[left];
                    entries[left] = temp;
                    QSOperations.Add(new SortOperation
                    {
                        EntriesToChange = EntriesToChange,
                        NewEntries = entries.Select(hr => hr).ToArray(),
                        ChangeToColour = "#FFFF00",
                        IsSwitchOperation = true
                    });
                }
                else
                {
                    return right;
                }
            }
        }




        private List<MergeSortOperation> MergeSortOperations = new List<MergeSortOperation>();

        public IEnumerable<MergeSortOperation> MergeSort(Entry[] entries)
        {
            MergeSortOperations.Clear();
            CarryOutMergeSort(entries);
            return MergeSortOperations;
        }

        private IEnumerable<Entry> CarryOutMergeSort(Entry[] entries)
        {
            // base case
            if (entries.Length <= 1)
            {
                return entries;
            }

            int midPoint = entries.Length / 2;
            Entry[] left = new Entry[midPoint];
            Entry[] right;

            // create right array
            if (entries.Length % 2 == 0)
            {
                right = new Entry[midPoint];
            }
            else
            {
                right = new Entry[midPoint + 1];
            }

            //populate left array
            for (int i = 0; i < midPoint; i++)
            {
                left[i] = entries[i];
            }
            //populate right array
            int x = 0;
            for (int i = midPoint; i < entries.Length; i++)
            {
                right[x] = entries[i];
                x++;
            }
            MergeSortOperations.Add(new MergeSortOperation
            {
                LeftEntries = left,
                RightEntries = right
            });

            // recursively sort left array
            left = CarryOutMergeSort(left).ToArray();
            // recursively sort right array
            right = CarryOutMergeSort(right).ToArray();
            // merge the two sorted arrays
            Entry[] result = Merge(left, right);
            return result;
        }

        private Entry[] Merge(Entry[] left, Entry[] right)
        {
            int resultLength = right.Length + left.Length;
            Entry[] resultArr = new Entry[resultLength];
            int leftIndex = 0;
            int rightIndex = 0;
            int indexResult = 0;

            while (leftIndex < left.Length ||
                   rightIndex < right.Length)
            {
                if (leftIndex < left.Length &&
                    rightIndex < right.Length)
                {
                    // if item on left array is less than item on right array
                    if (left[leftIndex].Value <= right[rightIndex].Value)
                    {
                        // add item to result array
                        resultArr[indexResult] = left[leftIndex];
                        MergeSortOperations.Add(new MergeSortOperation
                        {
                            ResultsEntries = resultArr,
                            IsFinalMergeOperation = true
                        });
                        leftIndex++;
                        indexResult++;
                    }
                    else
                    {
                        // right array will be added to results array
                        resultArr[indexResult] = right[rightIndex];
                        MergeSortOperations.Add(new MergeSortOperation
                        {
                            ResultsEntries = resultArr,
                            IsFinalMergeOperation = true
                        });
                        rightIndex++;
                        indexResult++;
                    }
                }
                else if (leftIndex < left.Length)
                {
                    // add all its items to the results array
                    resultArr[indexResult] = left[leftIndex];
                    MergeSortOperations.Add(new MergeSortOperation
                    {
                        ResultsEntries = resultArr,
                        IsFinalMergeOperation = true
                    });
                    leftIndex++;
                    indexResult++;
                }
                else if (rightIndex < right.Length)
                {
                    // add all its items to the results array
                    resultArr[indexResult] = right[rightIndex];
                    MergeSortOperations.Add(new MergeSortOperation
                    {
                        ResultsEntries = resultArr,
                        IsFinalMergeOperation = true
                    });
                    rightIndex++;
                    indexResult++;
                }
            }
            return resultArr;
        }
    }
}
