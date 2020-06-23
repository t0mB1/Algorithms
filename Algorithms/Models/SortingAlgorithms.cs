using System.Collections.Generic;
using System.Linq;
using Algorithms.Models.SortAlgorithmOperations;
using Microcharts;

namespace Algorithms.Models
{
    public class SortingAlgorithms
    {
        public List<BubbleSortOperation> BubbleSort(Entry[] entries)
        {
            List<BubbleSortOperation> operations = new List<BubbleSortOperation>();
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
                        operations.Add(new BubbleSortOperation
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

        public IEnumerable<HeapSortOperation> HeapSort(Entry[] entries)
        {
            List<HeapSortOperation> HSOperations = new List<HeapSortOperation>();
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
                HSOperations.Add(new HeapSortOperation
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

        private void Heapify(Entry[] entries, int n, int i, List<HeapSortOperation> HSOperations)
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

                HSOperations.Add(new HeapSortOperation
                {
                    EntriesToChange = entries1,
                    NewEntries = entries.Select(hr => hr).ToArray(),
                    ChangeToColour = "#FFFF00"
                });

                // Recursively heapify the affected sub-tree 
                Heapify(entries, n, largest, HSOperations);
            }
        }

        public List<InsertionSortOperation> InsertionSort(Entry[] entries)
        {
            List<InsertionSortOperation> operations = new List<InsertionSortOperation>();
            int n = entries.Length;
            for(int i = 0; i < n; i++)
            {
                Entry currentEntry = entries[i];
                int pos = i;

                operations.Add(new InsertionSortOperation
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
                operations.Add(new InsertionSortOperation
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

        public List<QuickSortOperation> QuickSort(Entry[] entries, int start, int end, List<QuickSortOperation> QSOperations)
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

        private int Partition(Entry[] entries, int left, int right, List<QuickSortOperation> QSOperations)
        {
            int pivot = (int)entries[left].Value;
            
            QSOperations.Add(new QuickSortOperation
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
                    QSOperations.Add(new QuickSortOperation
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
    }
}
