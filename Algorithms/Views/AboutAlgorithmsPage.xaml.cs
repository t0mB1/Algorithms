using System;
using Xamarin.Forms;
using Algorithms.Models;

namespace Algorithms.Views
{
    public partial class AboutAlgorithmsPage : ContentPage
    {
        public AboutAlgorithmsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetTableSectionTextColours();
        }

        private void NavToAlgPage(AlgorithmNameEnumeration Algorithm)
        {
            Navigation.PushAsync(new SelectedAlgorithmPage(Algorithm));
        }

        private void SetTableSectionTextColours()
        {
            if (App.TextColour != null)
            {
                SortTableSection.TextColor = Color.FromHex(App.TextColour);
                SearchTableSection.TextColor = Color.FromHex(App.TextColour);
            }
            else
            {
                SortTableSection.TextColor = Color.FromHex("#FF1493");
                SearchTableSection.TextColor = Color.FromHex("#FF1493");
            }
        }

        //                           *** Sort ***

        void BubbleSortTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.BubbleSort);
        }

        void HeapSortTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.HeapSort);
        }

        void InsertionSortTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.InsertionSort);
        }

        void MergeSortTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.MergeSort);
        }

        void QuickSortTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.QuickSort);
        }

        void SelectionSortTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.SelectionSort);
        }


        //                          *** Search ***

        void JumpSearchTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.JumpSearch);
        }

        void LinearSearchTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.LinearSearch);
        }

        void ClassicBinarySearchTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.ClassicBinarySearch);
        }

        void ModBinarySearchTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.ModBinarySearch);
        }

        void InterpolationSearchTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.InterpolationSearch);
        }

        void FibonacciSearchTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.FibonacciSearch);
        }
    }
}
