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
            ResetGraph();
        }

        private void SetBindingContext()
        {
            BindingContext = new SearchingGraphObject
            {
                Case = Case.Random
            };
        }

        private GraphService service = new GraphService();

        void ResetButtonIsClicked(object sender, EventArgs e)
        {
            ResetGraph();
        }

        private void ResetGraph()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SGObj.SearchItemValue == 0)
            {
                Random rnd = new Random();
                SGObj.SearchItemValue = rnd.Next(1, 20);
            }
            switch (SGObj.Case)
            {
                case Case.Random:
                    {
                        if(SGObj.CurrentAlg == "Classic Binary Search" ||
                           SGObj.CurrentAlg == "Modified Binary Search" ||
                           SGObj.CurrentAlg == "Jump Search")
                        {
                            DisplayGraph(service.GetBestCaseEntries(1, 20, SGObj.SearchItemValue));
                        }
                        else
                        {
                            DisplayGraph(service.GetRandomEntries(1, 20, SGObj.SearchItemValue));
                        }
                        break;
                    }
                case Case.Best:
                    { 
                        DisplayGraph(service.GetBestCaseEntries(1, 20, SGObj.SearchItemValue));
                        break;
                    }
                case Case.Worst:
                    {
                        DisplayGraph(service.GetWostCaseEntries(1, 20));
                        break;
                    }
            }
        }

        private void DisplayGraph(List<Entry> entries)
        {
            SearchGraph.Chart = new BarChart { Entries = entries };
            CurrentEntriesOnGraph = entries;
        }

        void AlgorithmPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (algorithmPicker.SelectedIndex != 0)
            {
                SearchBtn.IsVisible = true;
                int index = algorithmPicker.SelectedIndex;
                SGObj.CurrentAlg = algorithmPicker.Items[index].ToString();
                Title = SGObj.CurrentAlg;
                switch (SGObj.Case)
                {
                    case Case.Best:
                        if(SGObj.CurrentAlg == "Linear Search")
                        {
                            SGObj.SearchItemValue = 1;
                        }
                        else
                        {
                            SGObj.SearchItemValue = 10;
                        }
                        ChangeGraphToBestCaseShape();
                        break;

                    case Case.Worst:
                        ChangeGraphToWorstCase();
                        break;

                    case Case.Random:
                        if (!(SGObj.CurrentAlg == "Linear Search"))
                        {
                            ChangeGraphToRandomCase();
                            // SGObj.SearchItemValue = 0;
                            ChangeGraphToBestCaseShape();
                        }
                        else
                        {
                            ResetGraph();
                            ChangeGraphToRandomCase();
                            SGObj.SearchItemValue = 1;
                        }
                        break;
                }
            }
            else
            {
                Title = "Searches";
                SGObj.CurrentAlg = "";
                SearchBtn.IsVisible = false;
                searchItemPicker.IsEnabled = true;
                randomCaseBtn.IsEnabled = true;
            }
        }

        void searchItemPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
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
            if (SGObj.SearchItemValue != 0)
            {
                CurrentEntriesOnGraph[index].Color = SKColor.Parse("#0000FF");
            }
            DisplayGraph(CurrentEntriesOnGraph);
        }

        readonly SearchingAlgorithms algorithms = new SearchingAlgorithms();
        List<Entry> CurrentEntriesOnGraph = new List<Entry>();

        async void SearchBtnIsClicked(object sender, EventArgs e) 
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SGObj.SearchItemValue != 0)
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
                            CheckCaseOnSearch(SGObj);
                            List<JumpSearchOperation> operations = algorithms.JumpSearch(CurrentEntriesOnGraph,
                                                                                         SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                    case "Classic Binary Search":
                        {
                            CheckCaseOnSearch(SGObj);
                            List<BinarySearchOperation> operations = algorithms.ClassicBinarySearch(CurrentEntriesOnGraph,
                                                                                                    SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                    case "Modified Binary Search":
                        {
                            CheckCaseOnSearch(SGObj);
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

        private async void CarryOutOperations<T>(List<T> operations) where T : IOperation
        {
            foreach (T operation in operations)
            {
                int index = CurrentEntriesOnGraph.IndexOf(operation.entry);
                if (operation.IsSearchItem is false)
                {
                    // change to blue
                    CurrentEntriesOnGraph[index].Color = SKColor.Parse(operation.ChangeToColour);
                    DisplayGraph(CurrentEntriesOnGraph);
                    await Task.Delay(500);
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

        private void DisplayCaseBtns()
        {
            bestCaseBtn.IsVisible = true;
            bestCaseBtn.IsEnabled = true;
            worstCaseBtn.IsVisible = true;
            worstCaseBtn.IsEnabled = true;
            randomCaseBtn.IsVisible = true;
            randomCaseBtn.IsEnabled = true;
        }

        private void CheckCaseOnSearch(SearchingGraphObject SGObj)
        {
            if (SGObj.Case == Case.Best)
            {
                SGObj.SearchItemValue = 10;
            }
            else if(SGObj.Case == Case.Worst)
            {
                SGObj.SearchItemValue = 20;
            }
        }

        private void ToggleButtons()
        {
            bool temp = algorithmPicker.IsEnabled;
            ResetToolBarItem.IsEnabled = !temp;
            SearchBtn.IsEnabled = !temp;
            algorithmPicker.IsEnabled = !temp;
            
            bestCaseBtn.IsVisible = !temp;
            randomCaseBtn.IsVisible = !temp;
            worstCaseBtn.IsVisible = !temp;
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
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

        void RandomCaseBtnIsClicked(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.Case = Case.Random;
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
            ChangeGraphToRandomCase();
        }

        void BestCaseBtnIsClicked(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SGObj.CurrentAlg == "Linear Search")
            {
                SGObj.SearchItemValue = 1;
            }
            else
            {
                SGObj.SearchItemValue = 10;
            }
            bestCaseBtn.FontAttributes = FontAttributes.Bold;
            randomCaseBtn.FontAttributes = FontAttributes.None;
            worstCaseBtn.FontAttributes = FontAttributes.None;
            ChangeGraphToBestCase();
        }

        void WorstCaseBtnIsClicked(object sender, EventArgs e)
        {
            ChangeGraphToWorstCase();
        }

        private void ChangeGraphToRandomCase()
        {
            searchItemPicker.IsVisible = true;
            searchItemPicker.IsEnabled = true;
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.Case = Case.Random;
            randomCaseBtn.FontAttributes = FontAttributes.Bold;
            bestCaseBtn.FontAttributes = FontAttributes.None;
            worstCaseBtn.FontAttributes = FontAttributes.None;
        }

        private void ChangeGraphToBestCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.Case = Case.Best;
            ChangeGraphToBestCaseShape();
            searchItemPicker.IsEnabled = false;
            searchItemPicker.SelectedIndex = 0;
        }

        private void ChangeGraphToBestCaseShape()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            DisplayGraph(service.GetBestCaseEntries(1, 20, SGObj.SearchItemValue));
        }

        private void ChangeGraphToWorstCase()
        {
            searchItemPicker.IsEnabled = false;
            searchItemPicker.SelectedIndex = 0;
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            DisplayGraph(service.GetWostCaseEntries(1, 20));
            SGObj.SearchItemValue = 20;
            SGObj.Case = Case.Worst;
            worstCaseBtn.FontAttributes = FontAttributes.Bold;
            randomCaseBtn.FontAttributes = FontAttributes.None;
            bestCaseBtn.FontAttributes = FontAttributes.None;
        }
    }
}
