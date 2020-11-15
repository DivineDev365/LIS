using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using LIS.ViewModels;
using DataAccess.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LIS.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class LoginPage : Page
	{
		public LoginPage()
		{
			this.InitializeComponent();
			DataContext = ViewModel;
		}

		public LoginPageViewModel ViewModel { get; set; } = new LoginPageViewModel();

		private void ForgotPwd_Clicked(object sender, RoutedEventArgs e)
		{

		}

		private async void Login_Clicked(object sender, RoutedEventArgs e)
		{
			bool empty = false;
			string field = string.Empty;
			if (string.IsNullOrEmpty(PwdBox.Password))
			{
				field = "Password"; empty = true;
			}
			if (string.IsNullOrEmpty(UserNameBox.Text))
			{
				field = "User Name"; empty = true;
			}
			if (empty)
			{
				ContentDialog boxEmptyDialog = new ContentDialog
				{
					Title = "Missing Fields",
					Content = $"Please Enter Your {field}",
					CloseButtonText = "Got It!"
				};

				await boxEmptyDialog.ShowAsync();
			}
			else
			{
				//verify user. DON'T CALL IF DB IS EMPTY
				//ViewModel.VerifyUser(UserNameBox.Text, PwdBox.Password);

				Members.CurrentUser = UserNameBox.Text;
				Frame.Navigate(typeof(Views.NavPage));
			}
		}

		private void SignUpClicked(object sender, RoutedEventArgs e)
		{

		}
	}
}
