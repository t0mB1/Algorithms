using System.Collections.Generic;
using Microcharts;

namespace Algorithms.Models.SortAlgorithmOperations
{
    public class MergeSortOperation
    {
        public IEnumerable<Entry> LeftEntries { get; set; }
        public IEnumerable<Entry> RightEntries { get; set; }
        public IEnumerable<Entry> ResultsEntries { get; set; }
        public bool IsFinalMergeOperation { get; set; }
    }
}
