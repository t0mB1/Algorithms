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

        private void NavToAlgPage(AlgorithmNameEnumeration Algorithm)
        {
            Navigation.PushAsync(new SelectedAlgorithmPage(Algorithm));
        }



        //                           *** Sort ***

        void BubbleSortTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.BubbleSort);
        }

        void MergeSortTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.MergeSort);
        }

        void InsertionSortTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.InsertionSort);
        }

        void QuickSortTextCell_Tapped(object sender, EventArgs e)
        {
            NavToAlgPage(AlgorithmNameEnumeration.QuickSort);
        }



        //                              *** Search ***

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
    }
}
