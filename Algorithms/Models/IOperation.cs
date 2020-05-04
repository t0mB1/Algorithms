using Microcharts;

namespace Algorithms.Models
{
    public interface IOperation
    {
        Entry entry { get; set; }
        bool IsSearchItem { get; set; }
        string ChangeToColour { get; set; }
    }
}
