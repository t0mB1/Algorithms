using Microcharts;

namespace Algorithms.Models.SearchAlgorithmOperations
{
    public interface ISearchOperation
    {
        Entry entry { get; set; }
        string ChangeToColour { get; set; }
        bool IsSearchItem { get; set; }
    }
}
