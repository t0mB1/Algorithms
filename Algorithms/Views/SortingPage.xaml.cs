﻿using System.Collections.Generic;
using Entry = Microcharts.Entry;
using Xamarin.Forms;
using Microcharts;
using SkiaSharp;
using System;
using Algorithms.Models;
using Algorithms.Services;
using Algorithms.Models.SortAlgorithmOperations;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using System.Linq;

namespace Algorithms.Views
{
    public partial class SortingPage : ContentPage
    {
        public SortingPage()
        {
            InitializeComponent();
            SetBindingContext();
            ChangeToRandomCase();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ResetGraph();
        }

        void ResetButtonIsClicked(object sender, EventArgs e)
        {
            ResetGraph();
        }

        void WorstCaseBtnIsClicked(object sender, EventArgs e)
        {
            ChangeToWorstCase();
        }

        void RandomCaseBtnIsClicked(object sender, EventArgs e)
        {
            ChangeToRandomCase();
        }

        void SpeedPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            if (SpeedPicker.SelectedIndex > 0)
            {
                SGObj.Speed = SpeedPicker.SelectedItem.ToString();
            }
            else
            {
                SGObj.Speed = "";
            }
            ToggleSortBtn();
        }

        void AlgorithmPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
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
            ToggleSortBtn();
            ResetGraph();
        }

        void GraphElementsPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            if (GraphElementsPicker.SelectedIndex > 0)
            {
                SGObj.GraphElementNumber = SGObj.GraphElementNumDictionary[
                                                 GraphElementsPicker.SelectedItem.
                                                 ToString()];
                ResetGraph();
            }
            else
            {
                SGObj.GraphElementNumber = 0;
                
            }
            ToggleSortBtn();
        }

        void SortBtnIsClicked(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            if (IsSorted(CurrentEntriesOnGraph) is true)
            {
                ResetGraph();
            }
            if (SGObj.CurrentAlg != "" ||
                SGObj.CurrentAlg != null)
            {
                ToggleButtons();
                switch (SGObj.CurrentAlg)
                {
                    case "Bubble Sort":
                        {
                            IEnumerable<SortOperation> operations = algorithms.BubbleSort(CurrentEntriesOnGraph.ToArray());
                            CarryOutOperations(operations.ToList());
                            break;
                        }
                    case "Heap Sort":
                        {
                            IEnumerable<SortOperation> operations = algorithms.HeapSort(CurrentEntriesOnGraph.ToArray());
                            CarryOutOperations(operations.ToList());
                            break;
                        }
                    case "Insertion Sort":
                        {
                            IEnumerable<SortOperation> operations = algorithms.InsertionSort(CurrentEntriesOnGraph.ToArray());
                            CarryOutOperations(operations.ToList());
                            break;
                        }
                    case "Quick Sort":
                        {
                            List<SortOperation> ops = new List<SortOperation>();
                            List<SortOperation> operations = algorithms.QuickSort(CurrentEntriesOnGraph.ToArray(),
                                                                                       0,
                                                                                       SGObj.GraphElementNumber-1,
                                                                                       ops);
                            CarryOutOperations(operations);
                            break; 
                        }
                    case "Selection Sort":
                        {
                            IEnumerable<SortOperation> operations = algorithms.SelectionSort(CurrentEntriesOnGraph.ToArray());
                            CarryOutOperations(operations.ToList());
                            break;
                        }
                }
            }
        }

        private async void CarryOutOperations<T>(List<T> operations) where T : ISortOperation
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            int speed = SGObj.SpeedDictionary[SGObj.Speed];
            foreach (T operation in operations)
            {
                int index;
                foreach (Entry entry in operation.EntriesToChange)
                {
                    index = CurrentEntriesOnGraph.IndexOf(entry);
                    if (index > 0)
                    {
                        CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(operation.ChangeToColour);
                    }
                }
                DisplayGraph(operation.NewEntries);
                await Task.Delay(speed);

                CurrentEntriesOnGraph = (Entry[])operation.NewEntries;
                DisplayGraph(CurrentEntriesOnGraph);
                foreach (Entry entry in operation.EntriesToChange)
                {
                    // change colour back to red
                    index = CurrentEntriesOnGraph.IndexOf(entry);
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(App.GraphColour);
                }
            }
            ToggleButtons();
            DisplayCaseBtns();
        }

        private static bool IsSorted(IEnumerable<Entry> arr)
        {
            for (int i = 1; i < arr.Count(); i++)
            {
                if (arr.ToArray()[i - 1].Value > arr.ToArray()[i].Value)
                {
                    return false;
                }
            }
            return true;
        }

        private void SetBindingContext()
        {
            BindingContext = new SortingGraphObject
            {
                Case = GraphCaseEnum.Random,
                GraphElementNumber = 20
            };
        }

        private void ResetGraph()
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            switch (SGObj.Case)
            {
                case GraphCaseEnum.Worst:
                    if (SGObj.GraphElementNumber > 0)
                    {
                        CurrentEntriesOnGraph = service.GetWostCaseEntriesForSort(1, SGObj.GraphElementNumber);
                    }
                    else
                    {
                        CurrentEntriesOnGraph = service.GetWostCaseEntriesForSort(1, 20);
                    }
                    DisplayGraph(CurrentEntriesOnGraph);
                    break;

                case GraphCaseEnum.Random:
                    if (SGObj.GraphElementNumber > 0)
                    {
                        CurrentEntriesOnGraph = service.GetRandomEntries(1, SGObj.GraphElementNumber, 0);
                    }
                    else
                    {
                        CurrentEntriesOnGraph = service.GetRandomEntries(1, 20, 0);
                    }
                    DisplayGraph(CurrentEntriesOnGraph);
                    break;
            }
        }

        private void ToggleButtons()
        {
            bool temp = algorithmPicker.IsEnabled;
            algorithmPicker.IsEnabled = !temp;
            SpeedPicker.IsEnabled = !temp;
            ResetToolBarItem.IsEnabled = !temp;
            SortBtn.IsEnabled = !temp;
            SpeedPicker.IsEnabled = !temp;
            randomCaseBtn.IsVisible = !temp;
            worstCaseBtn.IsVisible = !temp;
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            switch (SGObj.Case)
            {
                case GraphCaseEnum.Worst:
                    worstCaseBtn.IsVisible = temp;
                    worstCaseBtn.IsEnabled = !temp;
                    break;

                case GraphCaseEnum.Random:
                    randomCaseBtn.IsVisible = temp;
                    randomCaseBtn.IsEnabled = !temp;
                    break;
            }
        }

        private void ToggleSortBtn()
        {
            if (algorithmPicker.SelectedIndex > 0 &&
                SpeedPicker.SelectedIndex > 0 &&
                GraphElementsPicker.SelectedIndex > 0)
            {
                SortBtn.IsVisible = true;
                SortBtn.IsEnabled = true;
            }
            else
            {
                SortBtn.IsVisible = false;
                SortBtn.IsEnabled = false;
            }
        }

        private void DisplayCaseBtns()
        {
            worstCaseBtn.IsVisible = true;
            worstCaseBtn.IsEnabled = true;
            randomCaseBtn.IsVisible = true;
            randomCaseBtn.IsEnabled = true;
        }

        private void ChangeToRandomCase()
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            SGObj.Case = GraphCaseEnum.Random;
            if (SGObj.GraphElementNumber > 0)
            {
                CurrentEntriesOnGraph = service.GetRandomEntries(1, SGObj.GraphElementNumber, 0);
            }
            else
            {
                CurrentEntriesOnGraph = service.GetRandomEntries(1, 20, 0);
            }
            DisplayGraph(CurrentEntriesOnGraph);
            UpdateCaseBtnAppearance();
        }

        private void ChangeToWorstCase()
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            SGObj.Case = GraphCaseEnum.Worst;
            if (SGObj.GraphElementNumber > 0)
            {
                CurrentEntriesOnGraph = service.GetWostCaseEntriesForSort(1, SGObj.GraphElementNumber);
            }
            else
            {
                CurrentEntriesOnGraph = service.GetWostCaseEntriesForSort(1, 20);
            }
            DisplayGraph(CurrentEntriesOnGraph);
            UpdateCaseBtnAppearance();
        }

        private void UpdateCaseBtnAppearance()
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            randomCaseBtn.TextColor = Color.FromHex("#AAAAAA");
            worstCaseBtn.TextColor = Color.FromHex("#AAAAAA");
            switch (SGObj.Case)
            {
                case GraphCaseEnum.Random:
                    randomCaseBtn.FontAttributes = FontAttributes.Bold;
                    worstCaseBtn.FontAttributes = FontAttributes.None;
                    randomCaseBtn.TextColor = Color.FromHex("#007EFA");
                    break;
                case GraphCaseEnum.Worst:
                    worstCaseBtn.FontAttributes = FontAttributes.Bold;
                    randomCaseBtn.FontAttributes = FontAttributes.None;
                    worstCaseBtn.TextColor = Color.FromHex("#007EFA");
                    break;
            }
        }

        private void DisplayGraph(IEnumerable<Entry> entries)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            int margin = 0;
            switch (SGObj.GraphElementNumber)
            {
                case 5:
                    margin = 30;
                    break;
                case 25:
                    margin = 15;
                    break;
                case 30:
                    margin = 15;
                    break;
                case 35:
                    margin = 12;
                    break;
                case 40:
                    margin = 10;
                    break;
                case 45:
                    margin = 8;
                    break;
                case 50:
                    margin = 6;
                    break;
            }
            if(margin == 0)
            {
                SortGraph.Chart = new BarChart
                {
                    Entries = entries,
                    BackgroundColor = SKColors.Transparent
                };
            }
            else
            {
                SortGraph.Chart = new BarChart
                {
                    Entries = entries,
                    Margin = margin,
                    BackgroundColor = SKColors.Transparent
                };
            }
        }

        private readonly GraphService service = new GraphService();
        private readonly SortingAlgorithms algorithms = new SortingAlgorithms();
        private IEnumerable<Entry> CurrentEntriesOnGraph = new Entry[20];
    }
}
