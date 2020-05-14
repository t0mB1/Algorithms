using System.Collections.Generic;
using Microcharts;

namespace Algorithms.Models.SortAlgorithmOperations
{
    public interface ISortOperation
    {
        IEnumerable<Entry> EntriesToChange { get; set; }
        IEnumerable<Entry> NewEntries { get; set; }
        string ChangeToColour { get; set; }
        bool IsSwitchOperation { get; set; }
    }
}
