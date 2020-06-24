using System;
using Xamarin.Forms;

namespace Algorithms.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SetTableSectionsColours();
        }

        private void SetTableSectionsColours()
        {
            if(App.TextColour != null)
            {
                ColoursSection.TextColor = Color.FromHex(App.TextColour);
                DeveloperSection.TextColor = Color.FromHex(App.TextColour);
            }
            else
            {
                ColoursSection.TextColor = Color.FromHex("#FF1493");
                DeveloperSection.TextColor = Color.FromHex("#FF1493");
            }
        }

        void GraphColourTextCell_Tapped(object sender, EventArgs e)
        {
            NavToSelectedSettingsPage();
        }

        void DarkModeSwitch_OnChanged(object sender, EventArgs e)
        {
            
        }

        private void NavToSelectedSettingsPage()
        {
            Navigation.PushAsync(new SelectedSettingsPage());
        }
    }
}
