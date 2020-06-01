using System;
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

        public void MergeSort(List<Entry> entries)
        {
            if(entries.Count > 1)
            {
                int mid = entries.Count / 2;
                List<Entry> lefthalf = entries.GetRange(0, mid);
                List<Entry> righthalf = entries.GetRange(mid, entries.Count - mid);
                MergeSort(lefthalf);
                MergeSort(righthalf);
                int i = 0;
                int j = 0;
                int k = 0;

                while(i < lefthalf.Count &&
                      j < righthalf.Count)
                {
                    if(lefthalf[i].Value < righthalf[j].Value)
                    {
                        entries[k] = righthalf[i];
                        i++;
                    }
                    else
                    {
                        entries[k] = righthalf[j];
                        j++;
                    }
                    k++;
                    Console.WriteLine(lefthalf + "\n" +
                                      righthalf + "\n" +
                                      entries);
                }

                while(i < lefthalf.Count)
                {
                    entries[k] = lefthalf[i];
                    i++;
                    k++;
                }

                while(j < righthalf.Count)
                {
                    entries[k] = righthalf[j];
                    j++;
                    k++;
                }
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

        private List<QuickSortOperation> QSOperations = new List<QuickSortOperation>();

        public List<QuickSortOperation> QuickSort(Entry[] entries, int start, int end)
        {
            if(start < end)
            {
                int split = Partition(entries, start, end);
                QuickSort(entries, start, split-1);
                QuickSort(entries, split + 1, end);
            }
            return QSOperations;
        }

        private int Partition(Entry[] entries, int start, int end)
        {
            int pivot = (int)entries.Select(hr => hr).ToArray()[end].Value;

            QSOperations.Add(new QuickSortOperation
            {
                EntriesToChange = new Entry[]
                {
                    entries.Select(hr => hr).ToArray()[end]
                },
                NewEntries = entries.Select(hr => hr).ToArray(),
                ChangeToColour = "#00FFFF",
                IsSwitchOperation = false
            });

            int i = start - 1;
            for (int j = start; j < end; j++)
            {
                QSOperations.Add(new QuickSortOperation
                {
                    EntriesToChange = new Entry[]
                    {
                        entries.Select(hr => hr).ToArray()[j]
                    },
                    NewEntries = entries.Select(hr => hr).ToArray(),
                    ChangeToColour = "#0000FF",
                    IsSwitchOperation = false
                });
                if (entries[j].Value < pivot)
                {
                    i++;
                    QSOperations.Add(new QuickSortOperation
                    {
                        EntriesToChange = new Entry[]
                        {
                            entries.Select(hr => hr).ToArray()[i],
                            entries.Select(hr => hr).ToArray()[j]
                        },
                        NewEntries = entries.Select(hr => hr).ToArray(),
                        ChangeToColour = "#FFFF00",
                        IsSwitchOperation = true
                    });
                    Entry temp = entries[i];
                    entries[i] = entries[i + 1];
                    entries[i+1] = temp;

                    QSOperations.Add(new QuickSortOperation
                    {
                        EntriesToChange = new Entry[]
                        {
                            entries.Select(hr => hr).ToArray()[i],
                            entries.Select(hr => hr).ToArray()[i+1]
                        },
                        NewEntries = entries.Select(hr => hr).ToArray(),
                        ChangeToColour = "#FFFF00",
                        IsSwitchOperation = true
                    });
                }
            }
            
            Entry temp1 = entries[i + 1];
            entries[i + 1] = entries[end];
            entries[end] = temp1;
            QSOperations.Add(new QuickSortOperation
            {
                EntriesToChange = new Entry[]
                {
                    entries.Select(hr => hr).ToArray()[i+1],
                    entries.Select(hr => hr).ToArray()[end]
                },
                NewEntries = entries.Select(hr => hr).ToArray(),
                ChangeToColour = "#FFFF00",
                IsSwitchOperation = true
            });
            return i + 1;

                    //QSOperations.Add(new QuickSortOperation
                    //{
                    //    EntriesToChange = new Entry[]
                    //    {
                    //        entries.Select(hr => hr).ToArray()[leftmark],
                    //        entries.Select(hr => hr).ToArray()[rightmark]
                    //    },
                    //    NewEntries = entries.Select(hr => hr).ToArray(),
                    //    ChangeToColour = "#FFFF00",
                    //    IsSwitchOperation = true
                    //});


                    //QSOperations.Add(new QuickSortOperation
                    //{
                    //    EntriesToChange = new Entry[]
                    //    {
                    //        entries.Select(hr => hr).ToArray()[leftmark],
                    //        entries.Select(hr => hr).ToArray()[rightmark]
                    //    },
                    //    NewEntries = entries.Select(hr => hr).ToArray(),
                    //    ChangeToColour = "#FFFF00",
                    //    IsSwitchOperation = true
                    //});
            //    }
            //}
            //QSOperations.Add(new QuickSortOperation
            //{
            //    EntriesToChange = new Entry[]
            //            {
            //                entries.Select(hr => hr).ToArray()[leftmark],
            //                entries.Select(hr => hr).ToArray()[rightmark]
            //            },
            //    NewEntries = entries.Select(hr => hr).ToArray(),
            //    ChangeToColour = "#FFFF00",
            //    IsSwitchOperation = true
            //});
            //Entry temp2 = entries[start];
            //entries[start] = entries[rightmark];
            //entries[rightmark] = temp2;
            //QSOperations.Add(new QuickSortOperation
            //{
            //    EntriesToChange = new Entry[]
            //            {
            //                entries.Select(hr => hr).ToArray()[leftmark],
            //                entries.Select(hr => hr).ToArray()[rightmark]
            //            },
            //    NewEntries = entries.Select(hr => hr).ToArray(),
            //    ChangeToColour = "#FFFF00",
            //    IsSwitchOperation = true
            //});
            //return rightmark;
        }

    }
}
