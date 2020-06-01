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
            DisplayGraph(service.GetRandomEntries(1, 20, 0));
        }

        void ResetButtonIsClicked(object sender, EventArgs e)
        {
            ResetGraph();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (algorithmPicker.SelectedIndex != -1)
            {
                SGObj.CurrentAlg = algorithmPicker.SelectedItem.ToString();
                Title = SGObj.CurrentAlg;
                
            }
            else
            {
                Title = "Searches";
                SGObj.CurrentAlg = "";
            }
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
                    case Case.Best:
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

                    case Case.Worst:
                        ChangeGraphToWorstCase();
                        break;

                    case Case.Random:

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
            SGObj.Case = Case.Random;
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
            UpdateCaseBtnFonts();
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
                Case = Case.Random
            };
        }

        private void ResetGraph()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SGObj.Case == Case.Best ||
                SGObj.Case == Case.Worst ||
                SGObj.Case == Case.Random)
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
        }

        private void ToggleSearchBtn()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SGObj.Case == Case.Random)
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
            if (SGObj.Case == Case.Best)
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
            else if (SGObj.Case == Case.Worst)
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
            ToggleItemPicker();
            switch (SGObj.Case)
            {
                case Case.Best:
                    bestCaseBtn.IsVisible = temp;
                    bestCaseBtn.IsEnabled = !temp;
                    break;

                case Case.Worst:
                    worstCaseBtn.IsVisible = temp;
                    worstCaseBtn.IsEnabled = !temp;
                    break;

                case Case.Random:
                    randomCaseBtn.IsVisible = temp;
                    randomCaseBtn.IsEnabled = !temp;
                    searchItemPicker.IsEnabled = !temp;
                    break;
            }
        }

        private void ToggleItemPicker()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (!(SGObj.Case == Case.Random))
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
            searchItemPicker.IsVisible = true;
            searchItemPicker.IsEnabled = true;
            SGObj.Case = Case.Random;
            UpdateCaseBtnFonts();
        }

        private void UpdateCaseBtnFonts()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SGObj.Case == Case.Random)
            {
                randomCaseBtn.FontAttributes = FontAttributes.Bold;
                bestCaseBtn.FontAttributes = FontAttributes.None;
                worstCaseBtn.FontAttributes = FontAttributes.None;
            }
            else if (SGObj.Case == Case.Best)
            {
                randomCaseBtn.FontAttributes = FontAttributes.None;
                bestCaseBtn.FontAttributes = FontAttributes.Bold;
                worstCaseBtn.FontAttributes = FontAttributes.None;
            }
            else if (SGObj.Case == Case.Worst)
            {
                randomCaseBtn.FontAttributes = FontAttributes.None;
                bestCaseBtn.FontAttributes = FontAttributes.None;
                worstCaseBtn.FontAttributes = FontAttributes.Bold;
            }
        }

        private void ChangeGraphToBestCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.Case = Case.Best;
            CheckCase();
            OrderEntriesOnGraph();
            searchItemPicker.IsEnabled = false;
            searchItemPicker.IsVisible = false;
            searchItemPicker.SelectedIndex = SGObj.SearchItemValue;
        }

        private void ChangeGraphToWorstCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.SearchItemValue = 20;
            SGObj.Case = Case.Worst;
            CheckCase();
            searchItemPicker.IsEnabled = false;
            searchItemPicker.IsVisible = false;
            searchItemPicker.SelectedIndex = SGObj.SearchItemValue;
            DisplayGraph(service.GetWostCaseEntries(1, 20));
            UpdateCaseBtnFonts();
        }

        private void OrderEntriesOnGraph()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            DisplayGraph(service.GetBestCaseEntries(1, 20, SGObj.SearchItemValue));
        }

        private void DisplayGraph(List<Entry> entries)
        {
            SearchGraph.Chart = new BarChart { Entries = entries };
            CurrentEntriesOnGraph = entries;
        }

        private readonly GraphService service = new GraphService();
        readonly SearchingAlgorithms algorithms = new SearchingAlgorithms();
        List<Entry> CurrentEntriesOnGraph = new List<Entry>();
    }
}
