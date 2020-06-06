using System;
using Algorithms.Styles;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(Algorithms.iOS.Renderers.PageRenderer))]
namespace Algorithms.iOS.Renderers
{   
    public class PageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null ||
                Element == null)
            {
                return;
            }
            try
            {
                SetAppTheme();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\t\t\tERROR: {ex.Message}");
            }
        }

        public override void TraitCollectionDidChange(UITraitCollection previousTraitCollection)
        {
            base.TraitCollectionDidChange(previousTraitCollection);
            Console.WriteLine($"TraitCollectionDidChange: {TraitCollection.UserInterfaceStyle} != {previousTraitCollection.UserInterfaceStyle}");

            if (this.TraitCollection.UserInterfaceStyle != previousTraitCollection.UserInterfaceStyle)
            {
                SetAppTheme();
            }
        }

        void SetAppTheme()
        {
            if (this.TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Dark)
            {
                if (App.AppTheme == "dark")
                    return;

                Xamarin.Forms.Application.Current.Resources = new DarkTheme();

                App.AppTheme = "dark";
            }
            else if (this.TraitCollection.UserInterfaceStyle == UIUserInterfaceStyle.Light)
            {
                if (App.AppTheme == "light" )
                    return;

                Xamarin.Forms.Application.Current.Resources = new LightTheme();

                App.AppTheme = "light";
            }
        }
    }
}
