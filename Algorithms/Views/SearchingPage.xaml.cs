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

        void ResetButtonIsClicked(object sender, EventArgs e)
        {
            ResetGraph();
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
            DisplayGraph(service.GetRandomEntries(1, 20, 0));
        }

        void AlgorithmPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (algorithmPicker.SelectedIndex != -1 &&
                algorithmPicker.SelectedItem.ToString() != "")
            {
                ToggleSearchBtn();
                // gets selected item in picker
                SGObj.CurrentAlg = algorithmPicker.SelectedItem.ToString();
                Title = SGObj.CurrentAlg;
                switch (SGObj.Case)
                {
                    case GraphCaseEnum.Best:
                        if (SGObj.CurrentAlg == "Linear Search" ||
                            SGObj.CurrentAlg == "Jump Search")
                        {
                            SGObj.SearchItemValue = 1;
                        }
                        else if (SGObj.CurrentAlg == "Classic Binary Search" ||
                                 SGObj.CurrentAlg == "Modified Binary Search")
                        {
                            SGObj.SearchItemValue = 10;
                        }
                        OrderEntriesOnGraph();
                        break;

                    case GraphCaseEnum.Worst:
                        ChangeGraphToWorstCase();
                        break;

                    case GraphCaseEnum.Random:

                        if (!(SGObj.CurrentAlg == "Linear Search"))
                        {
                            ChangeToRandomCase();
                            OrderEntriesOnGraph();
                        }
                        else
                        {
                            ResetGraph();
                            ChangeToRandomCase();
                        }
                        break;
                }
            }
            else
            {
                Title = "Searches";
                SGObj.CurrentAlg = "";
            }
        }

        void SearchItemPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ToggleSearchBtn();
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            Entry oldEntry = CurrentEntriesOnGraph.Where(p => p.Value == SGObj.SearchItemValue)
                                                  .FirstOrDefault();
            if (!(oldEntry is null))
            {
                oldEntry.Color = SKColor.Parse("#FF1493");
            }
            SGObj.SearchItemValue = searchItemPicker.SelectedIndex;
            Entry newEntry = CurrentEntriesOnGraph.Where(p => p.Value == SGObj.SearchItemValue)
                                                  .FirstOrDefault();
            int index = CurrentEntriesOnGraph.IndexOf(newEntry);
            if (SGObj.SearchItemValue > 0)
            {
                CurrentEntriesOnGraph[index].Color = SKColor.Parse("#0000FF");
            }
            DisplayGraph(CurrentEntriesOnGraph);
        }

        void SpeedPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            ToggleSearchBtn();
            if (SpeedPicker.SelectedIndex > 0)
            {
                SGObj.Speed = SpeedPicker.SelectedItem.ToString();
                if (!(algorithmPicker.SelectedIndex > 0 &&
                    searchItemPicker.SelectedIndex > 0))
                {
                    SGObj.Speed = "";
                }
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
                            List<LinearSearchOperation> operations = algorithms.LinearSearch(CurrentEntriesOnGraph,
                                                                                             SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                    case "Jump Search":
                        {
                            CheckCase();
                            List<JumpSearchOperation> operations = algorithms.JumpSearch(CurrentEntriesOnGraph,
                                                                                         SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                    case "Classic Binary Search":
                        {
                            CheckCase();
                            List<BinarySearchOperation> operations = algorithms.ClassicBinarySearch(CurrentEntriesOnGraph,
                                                                                                    SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                    case "Modified Binary Search":
                        {
                            CheckCase();
                            List<BinarySearchOperation> operations = algorithms.ModifiedBinarySearch(CurrentEntriesOnGraph,
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

        void RandomCaseBtnIsClicked(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.Case = GraphCaseEnum.Random;
            SGObj.SearchItemValue = 0;

            if (SGObj.CurrentAlg == "Classic Binary Search" ||
                SGObj.CurrentAlg == "Modified Binary Search" ||
                SGObj.CurrentAlg == "Jump Search")
            {
                DisplayGraph(service.GetBestCaseEntries(1, 20, SGObj.SearchItemValue));
            }
            else
            {
                DisplayGraph(service.GetRandomEntries(1, 20, SGObj.SearchItemValue));
            }
            ChangeToRandomCase();
        }

        void BestCaseBtnIsClicked(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.SearchItemValue = 0;
            if (algorithmPicker.SelectedItem != null)
            {
                if (SGObj.CurrentAlg == "Linear Search" ||
                    SGObj.CurrentAlg == "Jump Search")
                {
                    SGObj.SearchItemValue = 1;
                }
                else if (SGObj.CurrentAlg == "Binary Search" ||
                         SGObj.CurrentAlg == "Modified Binary Search")
                {
                    SGObj.SearchItemValue = 10;
                }
            }
            ChangeGraphToBestCase();
            UpdateCaseBtnAppearance();
        }

        void WorstCaseBtnIsClicked(object sender, EventArgs e)
        {
            ChangeGraphToWorstCase();
        }

        private async void CarryOutOperations<T>(List<T> operations) where T : ISearchOperation
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            int speed = SGObj.SpeedDictionary[SGObj.Speed];
            foreach (T operation in operations)
            {
                int index = CurrentEntriesOnGraph.IndexOf(operation.entry);
                if (operation.IsSearchItem is false)
                {
                    // change to blue
                    CurrentEntriesOnGraph[index].Color = SKColor.Parse(operation.ChangeToColour);
                    DisplayGraph(CurrentEntriesOnGraph);
                    await Task.Delay(speed);
                    // change back to red
                    CurrentEntriesOnGraph[index].Color = SKColor.Parse("#FF1493");
                }
                else
                {
                    // change to green
                    CurrentEntriesOnGraph[index].Color = SKColor.Parse(operation.ChangeToColour);
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
            foreach (Entry entry in CurrentEntriesOnGraph)
            {
                if (entry.Color == SKColor.Parse("#00FF00"))
                {
                    entry.Color = SKColor.Parse("#0000FF");
                    DisplayGraph(CurrentEntriesOnGraph);

                }
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

        private void CheckCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SGObj.Case == GraphCaseEnum.Best)
            {
                if (SGObj.CurrentAlg == "Jump Search" ||
                    SGObj.CurrentAlg == "Linear Search")
                {
                    SGObj.SearchItemValue = 1;
                }
                else
                {
                    SGObj.SearchItemValue = 10;
                }
            }
            else if (SGObj.Case == GraphCaseEnum.Worst)
            {
                SGObj.SearchItemValue = 20;
            }
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

        private void ChangeToRandomCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            ToggleSearchItemPicker();
            SGObj.Case = GraphCaseEnum.Random;
            UpdateCaseBtnAppearance();
        }

        private void UpdateCaseBtnAppearance()
        {
            randomCaseBtn.FontAttributes = FontAttributes.None;
            bestCaseBtn.FontAttributes = FontAttributes.None;
            worstCaseBtn.FontAttributes = FontAttributes.None;
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (App.AppTheme == "dark")
            {
                randomCaseBtn.BorderColor = Color.FromHex("#4C4C4C");
                worstCaseBtn.BorderColor = Color.FromHex("#4C4C4C");
                bestCaseBtn.BorderColor = Color.FromHex("#4C4C4C");
            }
            else if (App.AppTheme == "light")
            {
                randomCaseBtn.BorderColor = Color.FromHex("#CCCCCC");
                worstCaseBtn.BorderColor = Color.FromHex("#CCCCCC");
                bestCaseBtn.BorderColor = Color.FromHex("#CCCCCC");
            }
            switch (SGObj.Case)
            {
                case GraphCaseEnum.Random:
                    randomCaseBtn.FontAttributes = FontAttributes.Bold; ;
                    randomCaseBtn.BorderColor = Color.FromHex("#007EFA");
                    break;
                case GraphCaseEnum.Best:
                    bestCaseBtn.FontAttributes = FontAttributes.Bold;
                    bestCaseBtn.BorderColor = Color.FromHex("#007EFA");
                    break;
                case GraphCaseEnum.Worst:
                    worstCaseBtn.FontAttributes = FontAttributes.Bold;
                    worstCaseBtn.BorderColor = Color.FromHex("#007EFA");
                    break;
            }
        }

        private void ChangeGraphToBestCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.Case = GraphCaseEnum.Best;
            CheckCase();
            OrderEntriesOnGraph();
            ToggleSearchItemPicker();
        }

        private void ChangeGraphToWorstCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.SearchItemValue = 20;
            SGObj.Case = GraphCaseEnum.Worst;
            CheckCase();
            ToggleSearchItemPicker();
            DisplayGraph(service.GetWostCaseEntriesForSearch(1, 20));
            UpdateCaseBtnAppearance();
        }

        private void OrderEntriesOnGraph()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            DisplayGraph(service.GetBestCaseEntries(1, 20, SGObj.SearchItemValue));
        }

        private void DisplayGraph(List<Entry> entries)
        {

            SearchGraph.Chart = new BarChart { Entries = entries, BackgroundColor = SKColors.Transparent };
            CurrentEntriesOnGraph = entries;
        }

        private readonly GraphService service = new GraphService();
        readonly SearchingAlgorithms algorithms = new SearchingAlgorithms();
        List<Entry> CurrentEntriesOnGraph = new List<Entry>();
    }
}
