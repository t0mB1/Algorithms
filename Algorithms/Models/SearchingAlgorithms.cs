using System.Collections.Generic;
using Microcharts;
using System;
using Algorithms.Models.SearchAlgorithmOperations;

namespace Algorithms.Models
{
    public class SearchingAlgorithms
    {
        public List<LinearSearchOperation> LinearSearch(List<Entry> entries, int searchItem)
        {
            List<LinearSearchOperation> operations = new List<LinearSearchOperation>();
            int i = 0;
            while (i < entries.Count)
            {
                // highlight Entry Blue
                operations.Add(new LinearSearchOperation {
                    entry = entries[i],
                    ChangeToColour = "#FFFF00",
                    IsSearchItem = false
                });
                if (entries[i].Value == searchItem)
                {
                    // highlight Entry Green
                    operations.Add(new LinearSearchOperation
                    {
                        entry = entries[i],
                        ChangeToColour = "#00FF00",
                        IsSearchItem = true
                    });
                    return operations;
                }
                i += 1;
            }
            return operations;
        }

        public List<BinarySearchOperation> ClassicBinarySearch(List<Entry> entries, int searchItem)
        {
            int first = 0;
            int last = entries.Count - 1;
            List<BinarySearchOperation> BSOperations = new List<BinarySearchOperation>();
            while (first <= last)
            {
                int midpoint = (first + last) / 2;
                // highlight Entry Blue
                BSOperations.Add(new BinarySearchOperation() {
                                 entry = entries[midpoint],
                                 IsSearchItem = false,
                                 ChangeToColour = "#FFFF00"
                });
                if (entries[midpoint].Value == searchItem)
                {
                    // highlight Entry green
                    BSOperations.Add(new BinarySearchOperation()
                    {
                        entry = entries[midpoint],
                        IsSearchItem = true,
                        ChangeToColour = "#00FF00"
                    });
                    return BSOperations;
                }
                else
                {
                    if(entries[midpoint].Value < searchItem)
                    {
                        first = midpoint + 1;
                    }
                    else
                    {
                        last = midpoint - 1;
                    }
                }
            }
            return BSOperations;
        }

        public List<BinarySearchOperation> ModifiedBinarySearch(List<Entry> entries, int searchItem)
        {
            int first = 0;
            int last = entries.Count - 1;
            List<BinarySearchOperation> BSOperations = new List<BinarySearchOperation>();
            while (entries[first].Value <= searchItem &&
                  searchItem <= entries[last].Value)
            {
                int mid = first + ((last - first) / 2);
                // highlight entries[mid] Blue
                BSOperations.Add(new BinarySearchOperation()
                {
                    entry = entries[mid],
                    IsSearchItem = false,
                    ChangeToColour = "#FFFF00"
                });
                if (searchItem == entries[mid].Value)
                {
                    // highlight entries[mid] Green
                    BSOperations.Add(new BinarySearchOperation()
                    {
                        entry = entries[mid],
                        IsSearchItem = true,
                        ChangeToColour = "#00FF00"
                    });
                    return BSOperations;
                }
                else if(searchItem < entries[mid].Value)
                {
                    last = mid - 1;
                }
                else
                {
                    first = mid + 1;
                }
            }
            return BSOperations;
        }

        public List<JumpSearchOperation> JumpSearch(List<Entry> entries, int searchItem)
        {
            List<JumpSearchOperation> JSOperations = new List<JumpSearchOperation>();
            int n = entries.Count;
            // finds block size to jump
            int step = (int)Math.Floor(Math.Sqrt(n));
            // finding the block where the search item is
            // present (if it is present)
            int prev = 0;

            JSOperations.Add(new JumpSearchOperation
            {
                entry = entries[prev],
                IsSearchItem = false,
                ChangeToColour = "#FFFF00"
            });

            if (entries[prev].Value == searchItem)
            {
                JSOperations.Add(new JumpSearchOperation
                {
                    entry = entries[prev],
                    IsSearchItem = true,
                    ChangeToColour = "#00FF00"
                });
                return JSOperations;
            }

            while (entries[Math.Min(step, n) - 1].Value < searchItem)
            {
                // highlight entries[Math.Min(step, n)-1] Blue
                JSOperations.Add(new JumpSearchOperation
                {
                    entry = entries[Math.Min(step, n) - 1],
                    IsSearchItem = false,
                    ChangeToColour = "#FFFF00"
                });

                prev = step;
                step += (int)Math.Floor(Math.Sqrt(n));
                if(prev >= n)
                {
                    // return false;
                }
            }
            // linear search for search item in block
            // beginning with prev
            while(!(entries[prev].Value == searchItem))
            {
                // highlight entries[prev] Blue
                JSOperations.Add(new JumpSearchOperation
                {
                    entry = entries[prev],
                    IsSearchItem = false,
                    ChangeToColour = "#FFFF00"
                });
                prev++;
                int ting = Math.Min(step, n);
                if (prev == Math.Min(step, n))
                {
                    // return false;
                }
            }
            // if search item is found
            if(entries[prev].Value == searchItem)
            {
                // highlight entries[prev] Green
                JSOperations.Add(new JumpSearchOperation
                {
                    entry = entries[prev],
                    IsSearchItem = true,
                    ChangeToColour = "#00FF00"
                });
                // return true;
            }
            return JSOperations;
        }
    }
}
