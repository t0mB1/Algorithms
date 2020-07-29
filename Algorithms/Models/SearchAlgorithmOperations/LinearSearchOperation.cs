using Microcharts;

namespace Algorithms.Models.SearchAlgorithmOperations
{
    public class LinearSearchOperation : ISearchOperation
    {
        public Entry Entry { get; set; }
        public bool IsSearchItem { get; set; }
        public string ChangeToColour { get; set; }
    }
}
