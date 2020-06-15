using System;
using System.Collections.Generic;
using Algorithms.Models;
using Xamarin.Forms;
using System.Linq;
using Algorithms.Services;

namespace Algorithms.Views
{
    public partial class SelectedAlgorithmPage : ContentPage
    {
        private readonly FileHandlerService FileService = new FileHandlerService();

        public SelectedAlgorithmPage(AlgorithmNameEnumeration alg)
        {
            InitializeComponent();
            SetAllInfo(alg);
        }

        private void SetAllInfo(AlgorithmNameEnumeration alg)
        {
            Title = alg.ToString();
            string AllText = FileService.GetReleventText(alg);
            if (AllText != null)
            {
                SetInfoView(AllText);
                SetBigOText(AllText);
                SetImplementationViews(AllText);
            }
        }

        private void SetInfoView(string AllText)
        {
            InfomationLabel.Text = AllText.Split(new string[] { "\n" },
                              StringSplitOptions.None)[0];
        }

        private void SetBigOText(string AllText)
        {
            BestCaseLabel.Text = AllText.Split(new string[] { "\n" },
                              StringSplitOptions.None)[1];
            WorstCaseLabel.Text = AllText.Split(new string[] { "\n" },
                              StringSplitOptions.None)[2];
            AverageCaseLabel.Text = AllText.Split(new string[] { "\n" },
                          StringSplitOptions.None)[3];
        }

        private void SetImplementationViews(string AllText)
        {
            string[] TextArr = AllText.Split(new string[] { "\n" },
                                             StringSplitOptions.None);
            string[] arr = FileService.GetAllImplementations(TextArr).ToArray();
            // set views
            if (arr.Count() == 3)
            {
                Imp1Lbl.Text = arr[0];
                Imp2Lbl.Text = arr[1];
                Imp3Lbl.Text = arr[2];
            }
        }
    }
}
