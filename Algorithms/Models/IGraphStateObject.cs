using System;

namespace Algorithms.Models
{
    public interface IGraphStateObject
    {
        Case Case { get; set; }
        int SearchItemValue { get; set; }
    }
}
