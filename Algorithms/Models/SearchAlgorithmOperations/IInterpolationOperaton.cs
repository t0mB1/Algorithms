using Microcharts;

namespace Algorithms.Models.SearchAlgorithmOperations
{
    public interface IInterpolationOperation : ISearchOperation
    {
        Entry[] Markers { get; set; }
        bool SearchItemIsMarker { get; set; }
    }
}
