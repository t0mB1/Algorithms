using System.Collections.Generic;
using Entry = Microcharts.Entry;
using Xamarin.Forms;
using Microcharts;
using System;
using Algorithms.Models;
using Algorithms.Services;
using Algorithms.Models.SearchAlgorithmOperations;
using System.Threading.Tasks;
using SkiaSharp;
using System.Linq;

namespace Algorithms.Views
{
    public partial class SearchingPage : ContentPage
    {
        public SearchingPage()
        {
            InitializeComponent();
            SetBindingContext();
            ChangeToRandomCase();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (algorithmPicker.SelectedIndex > 0)
            {
                SGObj.CurrentAlg = algorithmPicker.SelectedItem.ToString();
                Title = SGObj.CurrentAlg;
            }
            else
            {
                Title = "Searches";
                SGObj.CurrentAlg = "";
            }
            ResetGraph();
        }

        void ResetButtonIsClicked(object sender, EventArgs e)
        {
            ResetGraph();
        }

        private void RandomCaseBtnIsClicked(object sender, EventArgs e)
        {
            ChangeToRandomCase();
        }

        private void BestCaseBtnIsClicked(object sender, EventArgs e)
        {
            ChangeGraphToBestCase();
        }

        private void WorstCaseBtnIsClicked(object sender, EventArgs e)
        {
            ChangeGraphToWorstCase();
        }

        void AlgorithmPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (algorithmPicker.SelectedIndex > 0)
            {
                ToggleSearchBtn();
                // gets selected item in picker
                SGObj.CurrentAlg = algorithmPicker.SelectedItem.ToString();
                Title = SGObj.CurrentAlg;
            }
            else
            {
                Title = "Searches";
                SGObj.CurrentAlg = "";
            }
            ResetGraph();
        }

        void SearchItemPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleSearchBtn();
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            // change colour of old entry
            Entry oldEntry = CurrentEntriesOnGraph.Where(p => p.Value == SGObj.SearchItemValue)
                                                  .FirstOrDefault();
            if (!(oldEntry is null))
            {
                oldEntry.Color = SKColor.Parse("#FF1493");
            }

            SGObj.SearchItemValue = searchItemPicker.SelectedIndex;
            Entry searchItemEntry = CurrentEntriesOnGraph.Where(p => p.Value == SGObj.SearchItemValue)
                                                         .FirstOrDefault();

            int index = CurrentEntriesOnGraph.ToList().IndexOf(searchItemEntry);
            if (SGObj.SearchItemValue > 0)
            {
                CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse("#0000FF");
            }
            DisplayGraph(CurrentEntriesOnGraph);
        }

        void SpeedPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleSearchBtn();
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SpeedPicker.SelectedIndex > 0)
            {
                SGObj.Speed = SpeedPicker.SelectedItem.ToString();
            }
            else
            {
                SGObj.Speed = "";
            }
        }

        async void SearchBtnIsClicked(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SGObj.SearchItemValue > 0)
            {
                ToggleButtons();
                switch (SGObj.CurrentAlg)
                {
                    case "Linear Search":
                        {
                            List<LinearSearchOperation> operations = algorithms.LinearSearch(CurrentEntriesOnGraph.ToArray(),
                                                                                             SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                    case "Jump Search":
                        {
                            List<JumpSearchOperation> operations = algorithms.JumpSearch(CurrentEntriesOnGraph.ToArray(),
                                                                                         SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                    case "Classic Binary Search":
                        {
                            List<BinarySearchOperation> operations = algorithms.ClassicBinarySearch(CurrentEntriesOnGraph.ToArray(),
                                                                                                    SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                    case "Modified Binary Search":
                        {
                            List<BinarySearchOperation> operations = algorithms.ModifiedBinarySearch(CurrentEntriesOnGraph.ToArray(),
                                                                                                    SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                }
            }
            else
            {
                await DisplayAlert("Error",
                                   "Select a Search Item",
                                   "Ok");
            }
        }

        private async void CarryOutOperations<T>(List<T> operations) where T : ISearchOperation
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            int speed = SGObj.SpeedDictionary[SGObj.Speed];
            foreach (T operation in operations)
            {
                int index = CurrentEntriesOnGraph.ToList().IndexOf(operation.entry);
                if (operation.IsSearchItem is false)
                {
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(operation.ChangeToColour);
                    DisplayGraph(CurrentEntriesOnGraph);
                    await Task.Delay(speed);
                    // change back to original colour
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(App.GraphColour);
                }
                else
                {
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(operation.ChangeToColour);
                }
                DisplayGraph(CurrentEntriesOnGraph);
            }
            ToggleButtons();
            DisplayCaseBtns();
        }

        private void SetBindingContext()
        {
            BindingContext = new SearchingGraphObject
            {
                Case = GraphCaseEnum.Random
            };
        }

        private void ResetGraph()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            switch (SGObj.Case)
            {
                case GraphCaseEnum.Worst:
                    CurrentEntriesOnGraph = service.GetWostCaseEntriesForSearch(1, 20).ToArray();
                    DisplayGraph(CurrentEntriesOnGraph);
                    break;
                case GraphCaseEnum.Random:
                    if (SGObj.CurrentAlg == "Classic Binary Search" ||
                        SGObj.CurrentAlg == "Modified Binary Search" ||
                        SGObj.CurrentAlg == "Jump Search")
                    {
                        if(SGObj.SearchItemValue == 0)
                        {
                            DisplayGraph(service.GetBestCaseEntries(1, 20));
                        }
                        else
                        {
                            DisplayGraph(service.GetBestCaseEntries(1, 20, SGObj.SearchItemValue));
                        }
                    }
                    else
                    {
                        if (SGObj.SearchItemValue == 0)
                        {
                            DisplayGraph(service.GetRandomEntries(1, 20));
                        }
                        else
                        {
                            DisplayGraph(service.GetRandomEntries(1, 20, SGObj.SearchItemValue));
                        }
                    }
                    DisplayGraph(CurrentEntriesOnGraph);
                    break;
                case GraphCaseEnum.Best:
                    SGObj.SearchItemValue = 0;
                    if (algorithmPicker.SelectedIndex > 0)
                    {
                        if (SGObj.CurrentAlg == "Linear Search" ||
                            SGObj.CurrentAlg == "Jump Search")
                        {
                            SGObj.SearchItemValue = 1;
                            searchItemPicker.SelectedItem = "1";
                        }
                        else if (SGObj.CurrentAlg == "Classic Binary Search" ||
                                 SGObj.CurrentAlg == "Modified Binary Search")
                        {
                            SGObj.SearchItemValue = 10;
                            searchItemPicker.SelectedItem = "10";
                        }
                    }
                    CurrentEntriesOnGraph = service.GetBestCaseEntries(1, 20, SGObj.SearchItemValue).ToArray();
                    DisplayGraph(CurrentEntriesOnGraph);
                    break;
            }
        }

        private void ToggleSearchBtn()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SGObj.Case == GraphCaseEnum.Random)
            {
                if (searchItemPicker.SelectedIndex > 0 &&
                    SpeedPicker.SelectedIndex > 0 &&
                    algorithmPicker.SelectedIndex > 0)
                {
                    SearchBtn.IsVisible = true;
                    SearchBtn.IsEnabled = true;
                }
            }
            else
            {
                if(algorithmPicker.SelectedIndex > 0 &&
                   SpeedPicker.SelectedIndex > 0)
                {
                    SearchBtn.IsVisible = true;
                    SearchBtn.IsEnabled = true;
                }
            }
        }

        private void DisplayCaseBtns()
        {
            bestCaseBtn.IsVisible = true;
            bestCaseBtn.IsEnabled = true;
            worstCaseBtn.IsVisible = true;
            worstCaseBtn.IsEnabled = true;
            randomCaseBtn.IsVisible = true;
            randomCaseBtn.IsEnabled = true;
        }

        private void ToggleButtons()
        {
            bool temp = algorithmPicker.IsEnabled;
            ResetToolBarItem.IsEnabled = !temp;
            SpeedPicker.IsEnabled = !temp;
            SearchBtn.IsEnabled = !temp;
            algorithmPicker.IsEnabled = !temp;
            bestCaseBtn.IsVisible = !temp;
            randomCaseBtn.IsVisible = !temp;
            worstCaseBtn.IsVisible = !temp;
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            ToggleSearchItemPicker();
            switch (SGObj.Case)
            {
                case GraphCaseEnum.Best:
                    bestCaseBtn.IsVisible = temp;
                    bestCaseBtn.IsEnabled = !temp;
                    break;

                case GraphCaseEnum.Worst:
                    worstCaseBtn.IsVisible = temp;
                    worstCaseBtn.IsEnabled = !temp;
                    break;

                case GraphCaseEnum.Random:
                    randomCaseBtn.IsVisible = temp;
                    randomCaseBtn.IsEnabled = !temp;
                    searchItemPicker.IsEnabled = !temp;
                    break;
            }
        }

        private void ToggleSearchItemPicker()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (!(SGObj.Case == GraphCaseEnum.Random))
            {
                searchItemPicker.IsVisible = false;
                searchItemPicker.IsEnabled = false;
            }
            else
            {
                searchItemPicker.IsVisible = true;
                searchItemPicker.IsEnabled = true;
            }
        }

        private void UpdateCaseBtnAppearance()
        {
            ResetCaseBtnAppearance();
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            switch (SGObj.Case)
            {
                case GraphCaseEnum.Random:
                    randomCaseBtn.FontAttributes = FontAttributes.Bold; ;
                    randomCaseBtn.TextColor = Color.FromHex("#007EFA");
                    break;
                case GraphCaseEnum.Best:
                    bestCaseBtn.FontAttributes = FontAttributes.Bold;
                    bestCaseBtn.TextColor = Color.FromHex("#007EFA");
                    break;
                case GraphCaseEnum.Worst:
                    worstCaseBtn.FontAttributes = FontAttributes.Bold;
                    worstCaseBtn.TextColor = Color.FromHex("#007EFA");
                    break;
            }
        }

        private void ResetCaseBtnAppearance()
        {
            randomCaseBtn.FontAttributes = FontAttributes.None;
            bestCaseBtn.FontAttributes = FontAttributes.None;
            worstCaseBtn.FontAttributes = FontAttributes.None;
            randomCaseBtn.TextColor = Color.FromHex("#AAAAAA");
            worstCaseBtn.TextColor = Color.FromHex("#AAAAAA");
            bestCaseBtn.TextColor = Color.FromHex("#AAAAAA");
        }

        private void ChangeGraphToBestCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.Case = GraphCaseEnum.Best;
            SGObj.SearchItemValue = 0;
            searchItemPicker.SelectedIndex = 0;
            searchItemPicker.SelectedItem = 0;
            ResetGraph();
            UpdateCaseBtnAppearance();
            ToggleSearchItemPicker();
        }

        private void ChangeGraphToWorstCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.Case = GraphCaseEnum.Worst;
            SGObj.SearchItemValue = 20;
            searchItemPicker.SelectedItem = "20";
            ToggleSearchItemPicker();
            UpdateCaseBtnAppearance();
            DisplayGraph(service.GetWostCaseEntriesForSearch(1, 20).ToArray());
        }

        private void ChangeToRandomCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.Case = GraphCaseEnum.Random;
            ResetGraph();
            ToggleSearchItemPicker();
            UpdateCaseBtnAppearance();
        }

        private void DisplayGraph(IEnumerable<Entry> entries)
        {
            SearchGraph.Chart = new BarChart { Entries = entries, BackgroundColor = SKColors.Transparent };
            CurrentEntriesOnGraph = entries;
        }

        private readonly GraphService service = new GraphService();
        private readonly SearchingAlgorithms algorithms = new SearchingAlgorithms();
        private IEnumerable<Entry> CurrentEntriesOnGraph = new Entry[20];
    }
}
