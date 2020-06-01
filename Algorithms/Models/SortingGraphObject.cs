using System.Collections.Generic;

namespace Algorithms.Models
{
    public class SortingGraphObject
    {
        public GraphCaseEnum Case { get; set; }
        public string CurrentAlg { get; set; }
        public string Speed { get; set; }
        public readonly Dictionary<string, int> SpeedDictionary = new Dictionary<string, int>
        {
            ["Super-Duper Slow"] =350,
            ["Super Slow"] =280,
            ["Slow"] =200,
            ["Medium"] =115,
            ["Fast Ish"] =75,
            ["Fast"]=40,
            ["Super Fast"] =20,
            ["Super-Duper Fast"]=10,
        };
        public readonly Dictionary<string, int> GraphElementNumDictionary = new Dictionary<string, int>
        {
            ["5"] = 5,
            ["10"] = 10,
            ["15"] = 15,
            ["20"] = 20,
            ["25"] = 25,
            ["30"] = 30,
            ["35"] = 35,
            ["40"] = 40,
            ["45"] = 45,
            ["50"] = 50
        };
    }
}
