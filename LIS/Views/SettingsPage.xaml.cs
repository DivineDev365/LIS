using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LIS.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SettingsPage : Page
	{
		public SettingsPage()
		{
			this.InitializeComponent();
			Loaded += OnSettingsPageLoaded;
		}

		private void OnSettingsPageLoaded(object sender, RoutedEventArgs e)
		{
			var currentTheme = (string)ApplicationData.Current.LocalSettings.Values["themeSettings"];
			(ThemePanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme)).IsChecked = true;
		}

		private void ThemeRadioButtonChecked(object sender, RoutedEventArgs e)
		{
			var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();

			ApplicationData.Current.LocalSettings.Values["themeSettings"] = selectedTheme;

			ShowDialog();
		}

		private async void ShowDialog()
		{
			ContentDialog dialog = new ContentDialog()
			{
				Content = "Theme options will take effect only after restarting the app.",
				PrimaryButtonText = "Got It!"
			};
			await dialog.ShowAsync();
		}
	}
}
