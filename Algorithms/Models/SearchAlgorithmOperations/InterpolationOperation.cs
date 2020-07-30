using Microcharts;

namespace Algorithms.Models.SearchAlgorithmOperations
{
    public class InterpolationOperation : IInterpolationOperation
    {
        public Entry Entry { get; set; }
        public Entry[] Markers { get; set; }
        public bool IsSearchItem { get; set; }
        public string ChangeToColour { get; set; }
        public bool SearchItemIsMarker { get; set; }
    }
}
