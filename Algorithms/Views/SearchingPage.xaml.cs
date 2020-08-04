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
                            IEnumerable<SearchOperation> operations = algorithms.LinearSearch(CurrentEntriesOnGraph.ToArray(),
                                                                                             SGObj.SearchItemValue);
                            CarryOutSearchOperations(operations.ToList());
                            break;
                        }
                    case "Jump Search":
                        {
                            IEnumerable<SearchOperation> operations = algorithms.JumpSearch(CurrentEntriesOnGraph.ToArray(),
                                                                                         SGObj.SearchItemValue);
                            CarryOutSearchOperations(operations.ToList());
                            break;
                        }
                    case "Classic Binary Search":
                        {
                            IEnumerable<SearchOperation> operations = algorithms.ClassicBinarySearch(CurrentEntriesOnGraph.ToArray(),
                                                                                                    SGObj.SearchItemValue);
                            CarryOutSearchOperations(operations.ToList());
                            break;
                        }
                    case "Modified Binary Search":
                        {
                            IEnumerable<SearchOperation> operations = algorithms.ModifiedBinarySearch(CurrentEntriesOnGraph.ToArray(),
                                                                                                    SGObj.SearchItemValue);
                            CarryOutSearchOperations(operations.ToList());
                            break;
                        }

                    case "Interpolation Search":
                        {
                            IEnumerable<InterpolationOperation> operations = algorithms.InterpolationSearch(CurrentEntriesOnGraph.ToArray(),
                                                                                                     SGObj.SearchItemValue);
                            CarryOutInterpolationOperations(operations.ToList());
                            break;
                        }
                    case "Fibonacci Search":
                        {
                            IEnumerable<SearchOperation> operations = algorithms.FibonacciSearch(CurrentEntriesOnGraph.ToArray(),
                                                                                                     SGObj.SearchItemValue);
                            CarryOutSearchOperations(operations.ToList());
                            break;
                        }
                    default:
                        break;
                }
            }
            else
            {
                await DisplayAlert("Error",
                                   "Select a Search Item",
                                   "Ok");
            }
        }

        private async void CarryOutSearchOperations(List<SearchOperation> operations) 
        {
            foreach (SearchOperation operation in operations)
            {
                if (operation.Entry != null)
                {
                    await ChangeOperationEntry(operation);
                }
            }
            ToggleButtons();
            DisplayCaseBtns();
        }

        private async void CarryOutInterpolationOperations(List<InterpolationOperation> operations)
        {
            foreach (InterpolationOperation operation in operations)
            {
                if (operation.Markers != null &&
                    operation.Markers.Count() != 0)
                {
                    await HandleMarkers(operation);
                }
                if (operation.Entry != null)
                {
                    await ChangeOperationEntry(operation);
                }
            }
            // clear current markers from graph
            ClearMarkersOnGraph(CurrentMarkers);
            ToggleButtons();
            DisplayCaseBtns();
        }

        private async Task ChangeOperationEntry<T>(T operation) where T : ISearchOperation
        {
            if (operation.Entry != null)
            {
                SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
                int index = CurrentEntriesOnGraph.ToList().IndexOf(operation.Entry);
                if (operation.IsSearchItem is false)
                {
                    Console.WriteLine(index);
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(operation.ChangeToColour);
                    DisplayGraph(CurrentEntriesOnGraph);
                    await Task.Delay(SGObj.SpeedDictionary[SGObj.Speed]);
                    // change back to original colour
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(App.GraphColour);
                }
                else if (operation.IsSearchItem is true)
                {
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(operation.ChangeToColour);
                }
                DisplayGraph(CurrentEntriesOnGraph);
            }
        }

        private async Task HandleMarkers(InterpolationOperation operation)
        {
            if (operation.Markers != null &&
                operation.Markers.Length != 0)
            {
                SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
                if (CurrentMarkers.Count() != 0)
                {
                    // change colour of current markers on graph
                    ClearMarkersOnGraph(CurrentMarkers);
                    await Task.Delay(SGObj.SpeedDictionary[SGObj.Speed]);
                }

                // add new markers
                foreach (Entry entry in operation.Markers)
                {
                    int index = CurrentEntriesOnGraph.ToList().IndexOf(entry);
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(operation.ChangeToColour);
                    CurrentMarkers.Add(entry);
                }
                DisplayGraph(CurrentEntriesOnGraph);
                await Task.Delay(SGObj.SpeedDictionary[SGObj.Speed]);
            }
        }

        private async void ClearMarkersOnGraph(IEnumerable<Entry> markers)
        {
            if(markers != null &&
               markers.Count() != 0)
            {
                SearchingGraphObject SGObj = (SearchingGraphObject)BindingContext;
                // change colour of current markers back
                foreach (Entry marker in markers)
                {
                    if (marker.Value != SGObj.SearchItemValue)
                    {
                        int index = CurrentEntriesOnGraph.ToList().IndexOf(marker);
                        CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(App.GraphColour);
                        DisplayGraph(CurrentEntriesOnGraph);
                        await Task.Delay(SGObj.SpeedDictionary[SGObj.Speed]);
                    }
                }
                CurrentMarkers.Clear();
            }
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
                    if(SGObj.CurrentAlg == "Interpolation Search")
                    {
                        SGObj.SearchItemValue = 10;
                        searchItemPicker.SelectedItem = "10";
                        CurrentEntriesOnGraph = service.GetWostCaseEntriesForSearch(1, 20, 10).ToArray();
                    }
                    else
                    {
                        SGObj.SearchItemValue = 20;
                        searchItemPicker.SelectedItem = "20";
                        CurrentEntriesOnGraph = service.GetWostCaseEntriesForSearch(1, 20).ToArray();
                    }
                    DisplayGraph(CurrentEntriesOnGraph);
                    break;
                case GraphCaseEnum.Random:
                    if (SGObj.CurrentAlg == "Classic Binary Search" ||
                        SGObj.CurrentAlg == "Modified Binary Search" ||
                        SGObj.CurrentAlg == "Jump Search" ||
                        SGObj.CurrentAlg == "Interpolation Search" ||
                        SGObj.CurrentAlg == "Fibonacci Search")
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
                            SGObj.CurrentAlg == "Jump Search" ||
                            SGObj.CurrentAlg == "Interpolation Search")
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
                        else if(SGObj.CurrentAlg == "Fibonacci Search")
                        {
                            SGObj.SearchItemValue = 8;
                            searchItemPicker.SelectedItem = "8";
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
            ResetGraph();
            ToggleSearchItemPicker();
            UpdateCaseBtnAppearance();
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
        private List<Entry> CurrentMarkers = new List<Entry>();
    }
}
