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
using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;

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
		//private string bookcategory = string.Empty;
		//private string statuscategory = string.Empty;
		private string membercategory = string.Empty;
		
		private async void AddBookClicked(object sender, RoutedEventArgs e)
		{
			Book book = new Book() 
			{
				BookId = BookIDBox.Text,
				Name = BookNameBox.Text,
				Author = AuthorBox.Text,
				Price = PriceBox.Text,
				RackNo = RackNoBox.Text,
				Status = StatusComboBox.SelectedItem.ToString(),
				Edition = EditionBox.Text,
				Category = CategoryComboBox.SelectedItem.ToString()
			};

			viewModel.AddBookCommand(book);
			
			ContentDialog ResultDialog = new ContentDialog
			{
				Title = "Done!",
				Content = "Operation Successful",
				CloseButtonText = "Got It!"
			};

			await ResultDialog.ShowAsync();
		}

		private async void DeleteBookClicked(object sender, RoutedEventArgs e)
		{
			viewModel.DeleteBookCommand(DeleteBookIDBox.Text);

			ContentDialog ResultDialog = new ContentDialog
			{
				Title = "Done!",
				Content = "Operation Successful",
				CloseButtonText = "Got It!"
			};

			await ResultDialog.ShowAsync();
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

			viewModel.AddUser(user, membercategory);
			
			
			ContentDialog ResultDialog = new ContentDialog
			{
				Title = "Done!",
				Content = "Operation Successful",
				CloseButtonText = "Got It!"
			};

			await ResultDialog.ShowAsync();
		}

		private async void DeleteMemClicked(object sender, RoutedEventArgs e)
		{
			viewModel.DeleteUserCommand(DeleteMemIDBox.Text);

			ContentDialog ResultDialog = new ContentDialog
			{
				Title = "Done!",
				Content = "Operation Successful",
				CloseButtonText = "Got It!"
			};

			await ResultDialog.ShowAsync();
			
		}

		private async void IssueBookClicked(object sender, RoutedEventArgs e)
		{
			viewModel.IssueBookCommand(IssueBookIDBox.Text, IssueMemIDBox.Text);
			ContentDialog ResultDialog = new ContentDialog
			{
				Title = "Message",
				Content = $"{viewModel.IssueBookStatus}",
				CloseButtonText = "Got It!"
			};

			await ResultDialog.ShowAsync();
		}

		private void ReturnBookClicked(object sender, RoutedEventArgs e)
		{
			viewModel.ReturnBook(ReturnBookIDBox.Text);
			
		}

		private async void ShowUsersClicked(object sender, RoutedEventArgs e)
		{
			ObservableCollection<Members> members = new ObservableCollection<Members>();
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				String userCommand = "SELECT * FROM users";

				SqliteCommand cmd = new SqliteCommand(userCommand, db);
				try
				{
					SqliteDataReader result = cmd.ExecuteReader();

					while (result.Read())
					{
						members.Add(new Members()
						{
							MemberId = result.GetString(0),
							Name = result.GetString(1),
							PhoneNo = result.GetString(3),
							BooksIssued = int.Parse(result.GetString(4)),
							MaxBookLimit = int.Parse(result.GetString(5)),
							IssueMonthDuration = int.Parse(result.GetString(6))
						});
					}
				}
				catch (Exception ex)
				{
					ContentDialog errorDialog = new ContentDialog
					{
						Title = "Message",
						Content = ex.Message,
						CloseButtonText = "Got It!"
					};
					await errorDialog.ShowAsync();
				}

				db.Close();
				UsersGrid.ItemsSource = members;
			}
		}

		private void ClearUserClicked(object sender, RoutedEventArgs e)
		{
			MemIDBox.Text = string.Empty;
			MemNameBox.Text = string.Empty;
			PwdBox.Password = string.Empty;
			PhoneNoBox.Text = string.Empty;
			MemCategoryComboBox.Text = string.Empty;
		}

		private void ClearBookClicked(object sender, RoutedEventArgs e)
		{
			BookIDBox.Text = string.Empty;
			BookNameBox.Text = string.Empty;
			AuthorBox.Text = string.Empty;
			PriceBox.Text = string.Empty;
			RackNoBox.Text = string.Empty;
			StatusComboBox.Text = string.Empty;
			EditionBox.Text = string.Empty;
			CategoryComboBox.Text = string.Empty;
		}
	}
}
