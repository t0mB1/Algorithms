using SkiaSharp;
using SQLite;
using Xamarin.Forms;

namespace Algorithms.Database
{
    public class ColourSchemeEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string GraphColourHex { get; set; }
        public string TextColourHex { get; set; }
        public bool DarkThemeIsOn { get; set; }
    }
}
