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

        private void ResetGraph()                                        // change
        {
            List<Entry> entries = new List<Entry>();
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
                        entries = service.GetRandomEntries(1, 20, SGObj.SearchItemValue);
                        break;
                    }
                case Case.Best:
                    {
                        entries = service.GetBestCaseEntries(20, 1, 1);
                        break;
                    }
                case Case.Worst:
                    {
                        entries = service.GetWostCaseEntries(1, 20);
                        break;
                    }
            }
            DisplayGraph(entries);
        }

        void ResetButtonIsClicked(object sender, EventArgs e)
        {
            ResetGraph();
        }

        private void DisplayGraph(List<Entry> entries)
        {
            SearchGraph.Chart = new BarChart { Entries = entries };
            CurrentEntriesOnGraph = entries;
        }

        void algorithmPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            goBtn.IsVisible = true;
            if (algorithmPicker.SelectedIndex != 0)
            {
                int index = algorithmPicker.SelectedIndex;
                SGObj.CurrentAlg = algorithmPicker.Items[index].ToString();
                Title = SGObj.CurrentAlg;
                if (SGObj.CurrentAlg == "Classic Binary Search" ||
                SGObj.CurrentAlg == "Modified Binary Search" ||
                SGObj.CurrentAlg == "Jump Search")
                {
                    ChangeGraphToBestCase();
                    searchItemPicker.IsEnabled = false;
                    randomCaseBtn.IsEnabled = false;
                }
            }
            else
            {
                Title = "Searches";
                SGObj.CurrentAlg = "";
                goBtn.IsVisible = false;
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
                switch (SGObj.CurrentAlg)
                {
                    case "Linear Search":
                        {
                            ToggleAllButtonsEnabled();
                            List<LinearSearchOperation> operations = algorithms.LinearSearch(CurrentEntriesOnGraph,
                                                                                             SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                    case "Jump Search":
                        {
                            ToggleSomeButtonsEnabled();
                            break;
                        }
                    case "Classic Binary Search":
                        {
                            ToggleSomeButtonsEnabled();
                            List<BinarySearchOperation> operations = algorithms.ClassicBinarySearch(CurrentEntriesOnGraph,
                                                                                                    SGObj.SearchItemValue);
                            CarryOutOperations(operations);
                            break;
                        }
                    case "Modified Binary Search":
                        {
                            ToggleSomeButtonsEnabled();
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

        private void ToggleAllButtonsEnabled()
        {
            bool temp = algorithmPicker.IsEnabled;
            randomCaseBtn.IsEnabled = !temp;
            bestCaseBtn.IsEnabled = !temp;
            worstCaseBtn.IsEnabled = !temp;
            ResetToolBarItem.IsEnabled = !temp;
            goBtn.IsEnabled = !temp;
            algorithmPicker.IsEnabled = !temp;
            searchItemPicker.IsEnabled = !temp;
        }

        private void ToggleSomeButtonsEnabled()
        {
            bool temp = algorithmPicker.IsEnabled;
            algorithmPicker.IsEnabled = !temp;
            ResetToolBarItem.IsEnabled = !temp;
            bestCaseBtn.IsEnabled = !temp;
            worstCaseBtn.IsEnabled = !temp;
        }

        private async void CarryOutOperations<T>(List<T> operations) where T : IOperation
        {
            foreach (T operation in operations)
            {
                int index = CurrentEntriesOnGraph.IndexOf(operation.entry);
                if (operation.IsSearchItem is false)
                {
                    CurrentEntriesOnGraph[index].Color = SKColor.Parse(operation.ChangeToColour);
                    DisplayGraph(CurrentEntriesOnGraph);
                    await Task.Delay(500);
                    CurrentEntriesOnGraph[index].Color = SKColor.Parse("#FF1493");
                }
                else
                {
                    CurrentEntriesOnGraph[index].Color = SKColor.Parse(operation.ChangeToColour);
                }
                DisplayGraph(CurrentEntriesOnGraph);
            }
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            if (SGObj.CurrentAlg == "Linear Search")
            {
                ToggleAllButtonsEnabled();
            }
            else
            {
                ToggleSomeButtonsEnabled();
            }
        }

        void RandomCaseBtnIsClicked(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            searchItemPicker.IsVisible = true;
            List<Entry> entries = new List<Entry>();
            if (SGObj.CurrentAlg == "Classic Binary Search" ||
                SGObj.CurrentAlg == "Modified Binary Search" ||
                SGObj.CurrentAlg == "Jump Search")
            {
                entries = service.GetBestCaseEntries(1, 20, SGObj.SearchItemValue);
            }
            else
            {
                entries = service.GetRandomEntries(1, 20, SGObj.SearchItemValue);
            }
            DisplayGraph(entries);
            SGObj.Case = Case.Random;
            randomCaseBtn.FontAttributes = FontAttributes.Bold;
            bestCaseBtn.FontAttributes = FontAttributes.None;
            worstCaseBtn.FontAttributes = FontAttributes.None;
        }


        void BestCaseBtnIsClicked(object sender, EventArgs e)
        {
            ChangeGraphToBestCase();
        }

        void ChangeGraphToBestCase()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            List<Entry> entries = service.GetBestCaseEntries(1, 20, 1);
            DisplayGraph(entries);
            ChangeLowestEntryValue();
            SetSearchItemPickerToLargestVal();
            SGObj.SearchItemValue = 1;
            SGObj.Case = Case.Best;
            bestCaseBtn.FontAttributes = FontAttributes.Bold;
            randomCaseBtn.FontAttributes = FontAttributes.None;
            worstCaseBtn.FontAttributes = FontAttributes.None;
        }

        void WorstCaseBtnIsClicked(object sender, EventArgs e)
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            List<Entry> entries = service.GetWostCaseEntries(1, 20);
            DisplayGraph(entries);
            ChangeLargestEntryValue();
            SetSearchItemPickerToLargestVal();
            SGObj.SearchItemValue = 20;
            SGObj.Case = Case.Worst;
            worstCaseBtn.FontAttributes = FontAttributes.Bold;
            randomCaseBtn.FontAttributes = FontAttributes.None;
            bestCaseBtn.FontAttributes = FontAttributes.None;
        }

        private void ChangeLargestEntryValue()
        {
            foreach(Entry entry in CurrentEntriesOnGraph)
            {
                if(entry.Color == SKColor.Parse("#0000FF"))
                {
                    entry.Color = SKColor.Parse("#FF1493");
                }
            }
            Entry largeEntry = CurrentEntriesOnGraph.Where(p => p.Value == 20)
                                               .FirstOrDefault();
            int index = CurrentEntriesOnGraph.IndexOf(largeEntry);
            CurrentEntriesOnGraph[index].Color = SKColor.Parse("#0000FF");
            DisplayGraph(CurrentEntriesOnGraph);
        }

        private void ChangeLowestEntryValue()
        {
            foreach (Entry entry in CurrentEntriesOnGraph)
            {
                if (entry.Color == SKColor.Parse("#0000FF"))
                {
                    entry.Color = SKColor.Parse("#FF1493");
                }
            }
            Entry largeEntry = CurrentEntriesOnGraph.Where(p => p.Value == 1)
                                               .FirstOrDefault();
            int index = CurrentEntriesOnGraph.IndexOf(largeEntry);
            CurrentEntriesOnGraph[index].Color = SKColor.Parse("#0000FF");
            DisplayGraph(CurrentEntriesOnGraph);
        }

        private void SetSearchItemPickerToLargestVal()
        {
            SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
            SGObj.SearchItemValue = 20;
            searchItemPicker.IsVisible = false;
        }
    }
}
