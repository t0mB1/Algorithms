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

        void ResetButtonIsClicked(object sender, EventArgs e)
        {
            ResetGraph();
        }

        void BestCaseBtnIsClicked(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            SGObj.Case = Case.Best;
            ChangeGraphToBestCase();
            UpdateCaseBtnFonts();
        }

        void WorstCaseBtnIsClicked(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            SGObj.Case = Case.Worst;

            UpdateCaseBtnFonts();
        }

        void RandomCaseBtnIsClicked(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            SGObj.Case = Case.Random;
            DisplayGraph(service.GetRandomEntries(1, 20, 0));
            UpdateCaseBtnFonts();
        }

        void SpeedPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            if (SpeedPicker.SelectedIndex > 0)
            {
                SGObj.Speed = SpeedPicker.SelectedItem.ToString();
                if(algorithmPicker.SelectedIndex > 0)
                {
                    SortBtn.IsVisible = true;
                    SortBtn.IsEnabled = true;
                }
                else
                {
                    SortBtn.IsVisible = false;
                    SortBtn.IsEnabled = false;
                    SGObj.Speed = "";
                }
            }
        }

        void AlgorithmPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            if (algorithmPicker.SelectedIndex > 0)
            {
                SGObj.CurrentAlg = algorithmPicker.SelectedItem.ToString();
                if (SpeedPicker.SelectedIndex > 0)
                {
                    SortBtn.IsVisible = true;
                    SortBtn.IsEnabled = true;
                }
                Title = SGObj.CurrentAlg;
            }
            else
            {
                Title = "Searches";
                SGObj.CurrentAlg = "";
                SortBtn.IsVisible = false;
                SortBtn.IsEnabled = false;
            }
        }

        void SortBtnIsClicked(object sender, EventArgs e)
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            if (IsSorted(CurrentEntriesOnGraph) is true)
            {
                if (SGObj.Case == Case.Random)
                {
                    CurrentEntriesOnGraph = service.GetRandomEntries(1, 20, 0).ToArray();
                }
                else
                {
                    CurrentEntriesOnGraph = service.GetWostCaseEntries(20, 1).ToArray();
                }
                
                DisplayGraph(CurrentEntriesOnGraph);
                Task.Delay(20);
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
                            IEnumerable<HeapSortOperation > operations = algorithms.HeapSort(CurrentEntriesOnGraph.ToArray());
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
                            List<QuickSortOperation> operations = algorithms.QuickSort(CurrentEntriesOnGraph.ToArray(), 0, 19);
                            CarryOutOpsWithPivot(operations);
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
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(operation.ChangeToColour);
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

        private async void CarryOutOpsWithPivot<T>(List<T> operations) where T : ISortOperation
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            int speed = SGObj.SpeedDictionary[SGObj.Speed];
            foreach (T operation in operations)
            {
                if (operation.IsSwitchOperation is false)
                {
                    foreach (Entry entry in CurrentEntriesOnGraph.Where(hr =>
                                            hr.Color == SKColor.Parse("#00FFFF")))
                    {
                        entry.Color = SKColor.Parse("#FF1493");
                    }
                    DisplayGraph(CurrentEntriesOnGraph);
                    await Task.Delay(speed);
                }
                int index;
                foreach (Entry entry in operation.EntriesToChange)
                {
                    index = CurrentEntriesOnGraph.IndexOf(entry);
                    CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse(operation.ChangeToColour);
                }
                CurrentEntriesOnGraph = (Entry[])operation.NewEntries;
                DisplayGraph(CurrentEntriesOnGraph);
                await Task.Delay(speed);

                if (operation.IsSwitchOperation is true)
                {
                    foreach (Entry entry in operation.EntriesToChange)
                    {
                        // change colour back to red
                        index = CurrentEntriesOnGraph.IndexOf(entry);
                        CurrentEntriesOnGraph.ToArray()[index].Color = SKColor.Parse("#FF1493");
                    }
                    DisplayGraph(CurrentEntriesOnGraph);
                }
            }
            ToggleButtons();
            DisplayCaseBtns();
        }

        private void SetBindingContext()
        {
            BindingContext = new SortingGraphObject
            {
                Case = Case.Random
            };
        }

        private void ResetGraph()
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            switch (SGObj.Case)
            {
                case Case.Best:

                    break;

                case Case.Worst:

                    break;

                case Case.Random:
                    CurrentEntriesOnGraph = service.GetRandomEntries(1, 20, 0).ToArray();
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
            bestCaseBtn.IsVisible = !temp;
            randomCaseBtn.IsVisible = !temp;
            worstCaseBtn.IsVisible = !temp;
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
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
                    break;
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

        private void ChangeToRandomCase()
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            SGObj.Case = Case.Random;
            CurrentEntriesOnGraph = service.GetRandomEntries(1, 20, 0).ToArray();
            DisplayGraph(CurrentEntriesOnGraph);
            UpdateCaseBtnFonts();
        }

        private void UpdateCaseBtnFonts()
        {
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
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
            SortingGraphObject SGObj = (SortingGraphObject)BindingContext;
            SGObj.Case = Case.Best;
            OrderEntriesOnGraph();
        }

        private void OrderEntriesOnGraph()
        {
            DisplayGraph(service.GetBestCaseEntries(1, 20, 0));
        }

        private void DisplayGraph(IEnumerable<Entry> entries)
        {
            SortGraph.Chart = new BarChart { Entries = entries };
        }

        private readonly GraphService service = new GraphService();
        readonly SortingAlgorithms algorithms = new SortingAlgorithms();
        IEnumerable<Entry> CurrentEntriesOnGraph = new Entry[20];
    }
}
