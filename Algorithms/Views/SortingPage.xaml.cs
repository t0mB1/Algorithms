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
using Algorithms.Styles;

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

        void ResetButtonIsClicked(object sender, EventArgs e)
        {
            ResetGraph();
        }

        void WorstCaseBtnIsClicked(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            SGObj.Case = GraphCaseEnum.Worst;
            CurrentEntriesOnGraph = service.GetWostCaseEntriesForSort(SGObj.GraphElementNumber, 1).ToArray();
            DisplayGraph(CurrentEntriesOnGraph);
            UpdateCaseBtnAppearance();
        }

        void RandomCaseBtnIsClicked(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            SGObj.Case = GraphCaseEnum.Random;
            CurrentEntriesOnGraph = service.GetRandomEntries(1, SGObj.GraphElementNumber, 0).ToArray();
            DisplayGraph(CurrentEntriesOnGraph);
            UpdateCaseBtnAppearance();
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
                            List<BubbleSortOperation> operations = algorithms.BubbleSort(CurrentEntriesOnGraph.ToArray());
                            CarryOutOpsWithoutPivot(operations);
                            break;
                        }
                    case "Heap Sort":
                        {
                            IEnumerable<HeapSortOperation> operations = algorithms.HeapSort(CurrentEntriesOnGraph.ToArray());
                            CarryOutOpsWithoutPivot(operations.ToList());
                            break;
                        }
                    case "Insertion Sort":
                        {
                            List<InsertionSortOperation> operations = algorithms.InsertionSort(CurrentEntriesOnGraph.ToArray());
                            CarryOutOpsWithoutPivot(operations);
                            break;
                        }
                    case "Quick Sort":
                        {
                            List<QuickSortOperation> ops = new List<QuickSortOperation>();
                            List<QuickSortOperation> operations = algorithms.QuickSort(CurrentEntriesOnGraph.ToArray(),
                                                                                       0,
                                                                                       SGObj.GraphElementNumber-1,
                                                                                       ops);
                            CarryOutOpsWithoutPivot(operations);
                            break; 
                        }
                }
            }
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

        private async void CarryOutOpsWithoutPivot<T>(List<T> operations) where T : ISortOperation
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
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse("#FF1493");
                }
            }
            ToggleButtons();
            DisplayCaseBtns();
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
                    CurrentEntriesOnGraph = service.GetWostCaseEntriesForSort(SGObj.GraphElementNumber, 1).ToArray();
                    DisplayGraph(CurrentEntriesOnGraph);
                    break;

                case GraphCaseEnum.Random:
                    CurrentEntriesOnGraph = service.GetRandomEntries(1, SGObj.GraphElementNumber, 0).ToArray();
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
            CurrentEntriesOnGraph = service.GetRandomEntries(1, SGObj.GraphElementNumber, 0).ToArray();
            DisplayGraph(CurrentEntriesOnGraph);
            UpdateCaseBtnAppearance();
        }

        private void UpdateCaseBtnAppearance()
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            if(App.AppTheme == "dark")
            {
                randomCaseBtn.BorderColor = Color.FromHex("#4C4C4C");
                worstCaseBtn.BorderColor = Color.FromHex("#4C4C4C");
            }
            else if (App.AppTheme == "light")
            {
                randomCaseBtn.BorderColor = Color.FromHex("#CCCCCC");
                worstCaseBtn.BorderColor = Color.FromHex("#CCCCCC");
            }
            switch (SGObj.Case)
            {
                case GraphCaseEnum.Random:
                    randomCaseBtn.FontAttributes = FontAttributes.Bold;
                    worstCaseBtn.FontAttributes = FontAttributes.None;
                    randomCaseBtn.BorderColor = Color.FromHex("#007EFA");
                    break;
                case GraphCaseEnum.Worst:
                    worstCaseBtn.FontAttributes = FontAttributes.Bold;
                    randomCaseBtn.FontAttributes = FontAttributes.None;
                    worstCaseBtn.BorderColor = Color.FromHex("#007EFA");
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
        readonly SortingAlgorithms algorithms = new SortingAlgorithms();
        IEnumerable<Entry> CurrentEntriesOnGraph = new Entry[20];
    }
}
