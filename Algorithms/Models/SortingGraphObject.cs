using System;

namespace Algorithms.Models
{
    public class SortingGraphObject : IGraphStateObject
    {
        public enum CurrentSortingAlgorithm
        {
            
        }

        public Case Case { get; set; }
        public int SearchItemValue { get; set; }
        public CurrentSortingAlgorithm CurrentAlg { get; set; }
    }
}
