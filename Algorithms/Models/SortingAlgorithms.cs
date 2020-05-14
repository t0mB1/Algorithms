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

            for (int i = 0; i < n-1; i++)
            {
                for (int j = 0; j < n - i - 1 ; j++)
                {
                    operations.Add(new BubbleSortOperation
                    {
                        EntriesToChange = new Entry[]
                        {
                            entries.Select(hr => hr).ToArray()[j]
                        },
                        NewEntries = entries.Select(hr => hr).ToArray(),
                        ChangeToColour = "#FFFF00"
                    });
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

        public void InsertionSort(List<Entry> entries)
        {
            int n = entries.Count;
            for(int i = 1; i < n-1; i++)
            {
                Entry currentEntry = entries[i];
                int pos = i;

                while(pos > 0 && entries[pos-1].Value > currentEntry.Value)
                {
                    entries[pos] = entries[pos-1];
                    pos--;
                }
                entries[pos] = currentEntry;
            }
        }

        public void QuickSort(List<Entry> entries, int start, int end)
        {
            if(start < end)
            {
                int split = Partition(entries, start, end);
                QuickSort(entries, start, split-1);
                QuickSort(entries, split + 1, end);
                // done
            }
        }

        private int Partition(List<Entry> entries, int start, int end)
        {
            int pivot = (int)entries[start].Value;
            int leftmark = start + 1;
            int rightmark = end;
            while (true)
            {
                while(leftmark <= rightmark &&
                      entries[leftmark].Value <= pivot)
                {
                    leftmark++;
                }
                while(entries[rightmark].Value >= pivot &&
                      rightmark >= leftmark)
                {
                    rightmark--;
                }
                if(rightmark < leftmark)
                {
                    break;
                }
                else
                {
                    Entry temp1 = entries[leftmark];
                    entries[leftmark] = entries[rightmark];
                    entries[rightmark] = temp1;
                }
            }
            Entry temp2 = entries[start];
            entries[start] = entries[rightmark];
            entries[rightmark] = temp2;
            return rightmark;
        }

    }
}
