using System;
using System.Collections.Generic;

namespace Algorithms.Models
{
    public class SortingGraphObject
    {
        public Case Case { get; set; }
        public string CurrentAlg { get; set; }
        public string Speed { get; set; }
        public readonly Dictionary<string, int> SpeedDictionary = new Dictionary<string, int>
        {
            ["Super-Duper Slow"] =250,
            ["Super Slow"] =180,
            ["Slow"] =100,
            ["Medium"] =65,
            ["Fast Ish"] =40,
            ["Fast"]=20,
            ["Super Fast"] =10,
            ["Super-Duper Fast"]=5,
            ["If You Blink, You'll Miss It"]=1
        };
    }
}
