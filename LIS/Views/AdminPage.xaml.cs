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
	public sealed partial class AdminPage : Page
	{
		public AdminPage()
		{
			this.InitializeComponent();
			DataContext = viewModel;
		}
		private AdminPageViewModel viewModel { get; set; } = new AdminPageViewModel();
		private string bookcategory = string.Empty;
		private string ststuscategory = string.Empty;
		private string membercategory = string.Empty;
		

		private void CategorySelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			bookcategory = e.AddedItems[0].ToString();
		}

		private void StatusSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ststuscategory = e.AddedItems[0].ToString();
		}

		private void AddBookClicked(object sender, RoutedEventArgs e)
		{

		}

		private void DeleteBookClicked(object sender, RoutedEventArgs e)
		{

		}

		private void MemCategorySelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			membercategory = e.AddedItems[0].ToString();
		}

		private async void AddMemClicked(object sender, RoutedEventArgs e)
		{
			Members user = new Members();
			user.MemberId = MemIDBox.Text;
			user.Name = MemNameBox.Text;
			user.Password = PwdBox.Password;
			user.PhoneNo = PhoneNoBox.Text;

			bool result = viewModel.AddUser(user, membercategory);
			if(result)
			{
				ContentDialog ResultDialog = new ContentDialog
				{
					Title = "Successful",
					Content = "User added to the database.",
					CloseButtonText = "Got It!"
				};

				await ResultDialog.ShowAsync();
			}
			else
			{
				ContentDialog ResultDialog = new ContentDialog
				{
					Title = "Failure",
					Content = "Unable to add user",
					CloseButtonText = "Got It!"
				};

				await ResultDialog.ShowAsync();
			}
			
		}

		private void DeleteMemClicked(object sender, RoutedEventArgs e)
		{

		}

		private void IssueBookClicked(object sender, RoutedEventArgs e)
		{

		}

		private void ReturnBookClicked(object sender, RoutedEventArgs e)
		{

		}
	}
}
