using System;
using System.Collections.Generic;
using Entry = Microcharts.Entry;
using Xamarin.Forms;
using Algorithms.Services;
using Microcharts;
using SkiaSharp;

namespace Algorithms.Views
{
    public partial class SelectedSettingsPage : ContentPage
    {
        public SelectedSettingsPage(Setting setting)
        {
            InitializeComponent();
            ViewSetting = setting;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DisplayView();
        }

        private void DisplayView()
        {
            switch (ViewSetting)
            {
                case Setting.GraphColour:
                    Title = "Graph Colour";
                    GraphColourPicker.SelectedItem = App.GraphColour;
                    ChangeGraphPickerIndex();
                    CurrentEntriesOnGraph = service.GetRandomEntries(1, 20);
                    DisplayGraph(CurrentEntriesOnGraph);
                    break;
            }
        }

        void ColourPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(GraphColourPicker.SelectedIndex > 0)
            {
                SKColor colour = service.ConvertGraphColourToSKColor(GraphColourPicker.SelectedItem.ToString());
                if (colour != App.GraphColour)
                {
                    App.GraphColour = colour;
                    UpdateColoursOnGraph();
                    DisplayGraph(CurrentEntriesOnGraph);
                }
            }
        }

        private void ChangeGraphPickerIndex()
        {
            if (App.GraphColour == SKColor.Parse("#FF1493"))
            {
                GraphColourPicker.SelectedIndex = 1;
            }
            else if (App.GraphColour == SKColor.Parse("#FF1493"))
            {
                GraphColourPicker.SelectedIndex = 2;
            }
            else if (App.GraphColour == SKColor.Parse("#FF1493"))
            {
                GraphColourPicker.SelectedIndex = 3;
            }
        }

        private void UpdateColoursOnGraph()
        {
            foreach (Entry entry in CurrentEntriesOnGraph)
            {
                entry.Color = App.GraphColour;
            }
        }

        private void DisplayGraph(IEnumerable<Entry> entries)
        {
            Graph.Chart = new BarChart { Entries = entries,
                                         BackgroundColor = SKColors.Transparent };
        }

        private readonly GraphService service = new GraphService();
        private IEnumerable<Entry> CurrentEntriesOnGraph = new Entry[20];
        private Setting ViewSetting;
    }
}
