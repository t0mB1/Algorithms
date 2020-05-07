using Microcharts;

namespace Algorithms.Models.SearchAlgorithmOperations
{
    public class JumpSearchOperation : IOperation
    {
        public Entry entry { get; set; }
        public bool IsSearchItem { get; set; }
        public string ChangeToColour { get; set; }
    }
}
