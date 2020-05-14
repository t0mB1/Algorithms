using System;
using System.Collections.Generic;

namespace Algorithms.Models
{
    public enum Case
    {
        Random,
        Best,
        Worst
    }

    public class SearchingGraphObject
    {
        public Case Case { get; set; }
        public int SearchItemValue { get; set; }
        public string CurrentAlg { get; set; }
        public string Speed { get; set; }
        public readonly Dictionary<string, int> SpeedDictionary = new Dictionary<string, int>
        {
            ["Super-Duper Slow"] = 1500,
            ["Super Slow"] = 1000,
            ["Slow"] = 750,
            ["Medium"] = 500,
            ["Fast Ish"] = 280,
            ["Fast"] = 200,
            ["Super Fast"] = 140,
            ["Super-Duper Fast"] = 80,
            ["If You Blink, You'll Miss It"] = 40
        };
    }
}
