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
using Windows.Storage;

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
			if (string.IsNullOrEmpty(UserIDBox.Text))
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
				await ViewModel.VerifyUserAsync(UserIDBox.Text, PwdBox.Password);
				if (ViewModel.UserExists)
				{
					//add user login info to local app data
					ApplicationData.Current.LocalSettings.Values["UserLoggedIn"] = true;
					ApplicationData.Current.LocalSettings.Values["LoggedInUserId"] = Members.CurrentUser;


					Frame.Navigate(typeof(Views.NavPage));
				}
				else
				{
					ViewModel.UserExists = false;
					ContentDialog ResultDialog = new ContentDialog
						{
							Title = "Error",
							Content = "Incorrect User ID or Password",
							CloseButtonText = "Got It!"
						};

						await ResultDialog.ShowAsync();
					
				}
			}
		}

		private void SignUpClicked(object sender, RoutedEventArgs e)
		{

		}
	}
}
