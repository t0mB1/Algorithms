using System;
using Xamarin.Forms;

namespace Algorithms.Views
{
    public enum Setting
    {
        GraphColour,
        TextColour
    }

    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        void GraphColourTextCell_Tapped(object sender, EventArgs e)
        {
            NavToSelectedSettingsPage(Setting.GraphColour);
        }

        void TextColourTextCell_Tapped(object sender, EventArgs e)
        {
            NavToSelectedSettingsPage(Setting.TextColour);
        }
        

        private void NavToSelectedSettingsPage(Setting setting)
        {
            Navigation.PushAsync(new SelectedSettingsPage(setting));
        }
    }
}
