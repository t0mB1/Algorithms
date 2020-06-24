using Algorithms.Styles;
using Xamarin.Forms;
using Algorithms.Database;
using System.IO;
using System;

namespace Algorithms
{
    public partial class App : Application
    {
        public static string AppTheme { get; set; }
        public static string GraphColour { get; set; }
        public static string TextColour { get; set; }

        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {   
        }

        protected override void OnResume()
        {
            if (AppTheme == "dark")
            {
                Current.Resources = new DarkTheme();
            }
            else if (AppTheme == "light")
            {
                Current.Resources = new LightTheme();
            }
        }

        static ColourSchemeDatabase database;

        public static ColourSchemeDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ColourSchemeDatabase(
                                        Path.Combine(
                                             Environment.GetFolderPath(
                                             Environment.SpecialFolder
                                                        .LocalApplicationData),
                                             "Notes.db3"));
                }
                return database;
            }
        }
    }
}
