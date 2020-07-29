using Microcharts;

namespace Algorithms.Models.SearchAlgorithmOperations
{
    public class SearchOperation : ISearchOperation
    {
        public Entry Entry { get; set; }
        public string ChangeToColour { get; set; }
        public bool IsSearchItem { get; set; }
    }
}
