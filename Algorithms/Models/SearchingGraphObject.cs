using System;

namespace Algorithms.Models
{
    public enum Case
    {
        Random,
        Best,
        Worst
    }

    public class SearchingGraphObject : IGraphStateObject
    {
        public Case Case { get; set; }
        public int SearchItemValue { get; set; }
        public string CurrentAlg { get; set; }
    }
}
