using System.Collections.Generic;
using Entry = Microcharts.Entry;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;

namespace Algorithms.Views
{
    public partial class SortingPage : ContentPage
    {
        public SortingPage()
        {
            InitializeComponent();
            List<Entry> entries = GetEntries(10);
            DisplayGraph(entries);
        }

        private List<Entry> GetEntries(int count)
        {
            List<Entry> entries = new List<Entry>();
            for(int i = 1; i < count; i++)
            {
                entries.Add(GenerateEntry(i));
            }
            return entries;
        }

        private Entry GenerateEntry(int value)
        {
            return new Entry(value)
            {
                Color = SKColor.Parse("#FF1493")
            };
        }

        private void DisplayGraph(List<Entry> entries)
        {
            SortGraph.Chart = new BarChart { Entries = entries };
        }
    }
}
