using Algorithms.Database;
using Xamarin.Forms;

namespace Algorithms
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            UpdateDb();
        }

        private void UpdateDb()
        {
            ColourSchemeEntity colourScheme = App.Database.GetColourSchemeDb();
            if (colourScheme != null)
            {
                // map colour scheme
                App.GraphColour = colourScheme.GraphColourHex;
                App.TextColour = colourScheme.TextColourHex;
            }
            else
            {
                string pink = "#FF1493";
                // add new entity with standard colours
                App.Database.SaveColourSchemeDb(new ColourSchemeEntity()
                {
                    GraphColourHex = pink,
                    TextColourHex = pink
                });
                App.GraphColour = pink;
                App.TextColour = pink;
            }
        }
    }
}
