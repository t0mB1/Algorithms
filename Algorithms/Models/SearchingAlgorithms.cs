using System.Collections.Generic;
using Microcharts;
using System;
using Algorithms.Models.SearchAlgorithmOperations;

namespace Algorithms.Models
{
    public class SearchingAlgorithms
    {
        public List<LinearSearchOperation> LinearSearch(Entry[] entries, int searchItem)
        {
            List<LinearSearchOperation> operations = new List<LinearSearchOperation>();
            int i = 0;
            while (i < entries.Length)
            {
                // highlight Entry Blue
                operations.Add(new LinearSearchOperation
                {
                    Entry = entries[i],
                    ChangeToColour = GetColourWhenSearchItemIsFalse(),
                    IsSearchItem = false
                });
                if (entries[i].Value == searchItem)
                {
                    // highlight Entry Green
                    operations.Add(new LinearSearchOperation
                    {
                        Entry = entries[i],
                        ChangeToColour = "#00FF00",
                        IsSearchItem = true
                    });
                    return operations;
                }
                i += 1;
            }
            return operations;
        }

        public List<BinarySearchOperation> ClassicBinarySearch(Entry[] entries, int searchItem)
        {
            // *** requires a sorted array ***
            int first = 0;
            int last = entries.Length - 1;
            List<BinarySearchOperation> BSOperations = new List<BinarySearchOperation>();
            while (first <= last)
            {
                int midpoint = (first + last) / 2;
                // highlight Entry Blue
                BSOperations.Add(new BinarySearchOperation()
                {
                    Entry = entries[midpoint],
                    IsSearchItem = false,
                    ChangeToColour = GetColourWhenSearchItemIsFalse()
                });
                if (entries[midpoint].Value == searchItem)
                {
                    // highlight Entry green
                    BSOperations.Add(new BinarySearchOperation()
                    {
                        Entry = entries[midpoint],
                        IsSearchItem = true,
                        ChangeToColour = "#00FF00"
                    });
                    return BSOperations;
                }
                else
                {
                    if (entries[midpoint].Value < searchItem)
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

        public List<BinarySearchOperation> ModifiedBinarySearch(Entry[] entries, int searchItem)
        {
            // *** requires a sorted array ***
            int first = 0;
            int last = entries.Length - 1;
            List<BinarySearchOperation> BSOperations = new List<BinarySearchOperation>();
            while (entries[first].Value <= searchItem &&
                  searchItem <= entries[last].Value)
            {
                int mid = first + ((last - first) / 2);
                // highlight entries[mid] Blue
                BSOperations.Add(new BinarySearchOperation()
                {
                    Entry = entries[mid],
                    IsSearchItem = false,
                    ChangeToColour = GetColourWhenSearchItemIsFalse()
                });
                if (searchItem == entries[mid].Value)
                {
                    // highlight entries[mid] Green
                    BSOperations.Add(new BinarySearchOperation()
                    {
                        Entry = entries[mid],
                        IsSearchItem = true,
                        ChangeToColour = "#00FF00"
                    });
                    return BSOperations;
                }
                else if (searchItem < entries[mid].Value)
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

        public List<JumpSearchOperation> JumpSearch(Entry[] entries, int searchItem)
        {
            // *** requires a sorted array ***
            List<JumpSearchOperation> JSOperations = new List<JumpSearchOperation>();
            int n = entries.Length;
            // finds block size to jump
            int step = (int)Math.Floor(Math.Sqrt(n));
            // finding the block where the search item is
            // present (if it is present)
            int prev = 0;

            JSOperations.Add(new JumpSearchOperation
            {
                Entry = entries[prev],
                IsSearchItem = false,
                ChangeToColour = GetColourWhenSearchItemIsFalse()
            });

            if (entries[prev].Value == searchItem)
            {
                JSOperations.Add(new JumpSearchOperation
                {
                    Entry = entries[prev],
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
                    Entry = entries[Math.Min(step, n) - 1],
                    IsSearchItem = false,
                    ChangeToColour = GetColourWhenSearchItemIsFalse()
                });

                prev = step;
                step += (int)Math.Floor(Math.Sqrt(n));
                if (prev >= n)
                {
                    // return false;
                }
            }
            // linear search for search item in block
            // beginning with prev
            while (!(entries[prev].Value == searchItem))
            {
                // highlight entries[prev] Blue
                JSOperations.Add(new JumpSearchOperation
                {
                    Entry = entries[prev],
                    IsSearchItem = false,
                    ChangeToColour = GetColourWhenSearchItemIsFalse()
                });
                prev++;
                int ting = Math.Min(step, n);
                if (prev == Math.Min(step, n))
                {
                    // return false;
                }
            }
            // if search item is found
            if (entries[prev].Value == searchItem)
            {
                // highlight entries[prev] Green
                JSOperations.Add(new JumpSearchOperation
                {
                    Entry = entries[prev],
                    IsSearchItem = true,
                    ChangeToColour = "#00FF00"
                });
                // return true;
            }
            return JSOperations;
        }

        public IEnumerable<SearchOperation> InterpolationSearch(Entry[] entries, int searchItem)
        {
            // *** requires a sorted array ***
            List<SearchOperation> operations = new List<SearchOperation>();
            // Find indexes of 
            // two corners 
            int left = 0;
            int right = entries.Length - 1;

            // Since array is sorted,  
            // an element present in 
            // array must be in range
            // defined by corner 
            while (searchItem >= entries[left].Value &&
                   searchItem <= entries[right].Value &&
                   left <= right)
            {
                if (left == right)
                {
                    operations.Add(new SearchOperation
                    {
                        Entry = entries[left],
                        ChangeToColour = "#00FF00",
                        IsSearchItem = true
                    });
                    return operations;
                }
                operations.Add(new SearchOperation
                {
                    Entry = entries[left],
                    ChangeToColour = GetColourWhenSearchItemIsFalse(),
                    IsSearchItem = false
                });
                operations.Add(new SearchOperation
                {
                    Entry = entries[right],
                    ChangeToColour = GetColourWhenSearchItemIsFalse(),
                    IsSearchItem = false
                });
                // Probing the position
                // with keeping uniform
                // distribution in mind
                int pos = (int)(left + ((right - left) /
                          (entries[right].Value - entries[left].Value) *
                          (searchItem - entries[left].Value)));

                operations.Add(new SearchOperation
                {
                    Entry = entries[pos],
                    ChangeToColour = GetColourWhenSearchItemIsFalse(),
                    IsSearchItem = false
                });

                // search item is found 
                if (entries[pos].Value == searchItem)
                {
                    operations.Add(new SearchOperation
                    {
                        Entry = entries[pos],
                        ChangeToColour = "#00FF00",
                        IsSearchItem = true
                    });
                    return operations;
                }
                if (entries[pos].Value < searchItem)
                {
                    // search item is in upper part 
                    left = pos + 1;
                }
                else
                {
                    // search item in lower part
                    right = pos - 1;
                }
            }
            return operations;
        }

        private string GetColourWhenSearchItemIsFalse()
        {
            // blue, item is yellow
            if (App.GraphColour == "#0000FF")
            {
                return "#FFFF00";
            }
            // blue, item is yellow
            if (App.GraphColour == "#0000FF")
            {
                return "#0000FF";
            }
            // pink, item is yellow
            return "#FFFF00";
        }
    }
}


