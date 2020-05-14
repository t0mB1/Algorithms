using System.Collections.Generic;
using Microcharts;

namespace Algorithms.Models.SortAlgorithmOperations
{
    public class BubbleSortOperation : ISortOperation
    {
        public IEnumerable<Entry> NewEntries { get; set; }
        public IEnumerable<Entry> EntriesToChange { get; set; }
        public string ChangeToColour { get; set; }
        public bool IsSwitchOperation { get; set; }
    }
}
