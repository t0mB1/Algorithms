using Microcharts;

namespace Algorithms.Models.SearchAlgorithmOperations
{
    public interface ISearchOperation
    {
        Entry Entry { get; set; }
        string ChangeToColour { get; set; }
        bool IsSearchItem { get; set; }
    }
}
