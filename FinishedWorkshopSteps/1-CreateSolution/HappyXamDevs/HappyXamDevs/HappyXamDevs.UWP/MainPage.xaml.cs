using System;
using Windows.UI.Xaml.Navigation;

namespace HappyXamDevs.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new HappyXamDevs.App());
        }
    }
}