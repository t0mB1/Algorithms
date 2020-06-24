using System;
using System.Collections.Generic;
using Entry = Microcharts.Entry;
using Xamarin.Forms;
using Algorithms.Services;
using Microcharts;
using SkiaSharp;
using Algorithms.Database;

namespace Algorithms.Views
{
    public partial class SelectedSettingsPage : ContentPage
    {
        public SelectedSettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DisplayView();
        }



        private void DisplayView()
        {
            TextLbl.TextColor = Color.FromHex(App.TextColour);
            CurrentEntriesOnGraph = service.GetRandomEntries(1, 20);
            DisplayGraph(CurrentEntriesOnGraph);
        }

        void GraphColourPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(GraphColourPicker.SelectedIndex > 0)
            {
                string colour = service.ConvertColourToHex(GraphColourPicker
                                        .SelectedItem
                                        .ToString());

                if (App.GraphColour != colour)
                {
                    App.GraphColour = colour;
                    UpdateColoursOnGraph();
                    DisplayGraph(CurrentEntriesOnGraph);
                    UpdateDbEntity();
                }
                DisplayAlertForRestart();
            }
        }

        private void DisplayAlertForRestart()
        {
            DisplayAlert("Alert", "You many need to restart the App to see changes", "Ok");
        }

        void TextColourPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TextColourPicker.SelectedIndex > 0)
            {
                // get hex colour
                string colour = service.ConvertColourToHex(TextColourPicker
                                      .SelectedItem
                                      .ToString());
                // update App text colour property
                if(App.TextColour != colour)
                {
                    App.TextColour = colour;
                    TextLbl.TextColor = Color.FromHex(App.TextColour);
                    UpdateDbEntity();
                }
                DisplayAlertForRestart();
            }
        }

        private void UpdateDbEntity()
        {
            ColourSchemeEntity colourScheme = App.Database.GetColourSchemeDb();
            // map properties
            colourScheme.GraphColourHex = App.GraphColour;
            colourScheme.TextColourHex = App.TextColour;
            // update
            App.Database.UpdateColourScheme(colourScheme);
        }

        private void UpdateColoursOnGraph()
        {
            foreach (Entry entry in CurrentEntriesOnGraph)
            {
                if(graph.IsEnabled == true &&
                   graph.IsVisible == true)
                {
                    entry.Color = SKColor.Parse(App.GraphColour);
                }
            }
        }

        private void DisplayGraph(IEnumerable<Entry> entries)
        {
            graph.Chart = new BarChart { Entries = entries,
                                         BackgroundColor = SKColors.Transparent };
        }

        private readonly GraphService service = new GraphService();
        private IEnumerable<Entry> CurrentEntriesOnGraph = new Entry[20];
    }
}
