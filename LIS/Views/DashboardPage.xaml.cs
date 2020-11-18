using DataAccess.Models;
using LIS.ViewModels;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
	public sealed partial class DashboardPage : Page
	{
		public DashboardPage()
		{
			this.InitializeComponent();
			//DashboardPageViewModel viewModel = new DashboardPageViewModel();
			this.GetUserDetails();
			//DataContext = viewModel;
			UserListView.ItemsSource = this.user;
			UserBooksGrid.ItemsSource = this.userBooks;
		}

		private ObservableCollection<Members> user = new ObservableCollection<Members>();
		private ObservableCollection<Book> userBooks = new ObservableCollection<Book>();
		public async void GetUserDetails()
		{
			if (string.Equals(Members.CurrentUser, "Admin"))
			{
				user.Add(new Members()
				{
					MemberId = "Admin",
					Name = "Admin",
					BooksIssued = 0,
					IssueMonthDuration = 0,
					MaxBookLimit = 0,
					PhoneNo = "NA"
				});
				return;
			}

			try
			{
				await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
				string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
				using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
				{
					db.Open();
					string userdetails = "SELECT ID, Name, PhoneNo, BooksIssued, MaxBookLimit, IssueMonthDuration " +
						$"from users WHERE ID = '{Members.CurrentUser}'";
					SqliteCommand userdetailscmd = new SqliteCommand(userdetails, db);
					SqliteDataReader userResult = userdetailscmd.ExecuteReader();
					while (userResult.Read())
					{
						user.Add(new Members()
						{
							MemberId = userResult.GetString(0),
							Name = userResult.GetString(1),
							PhoneNo = userResult.GetString(2),
							BooksIssued = int.Parse(userResult.GetString(3)),
							MaxBookLimit = int.Parse(userResult.GetString(4)),
							IssueMonthDuration = int.Parse(userResult.GetString(5))
						});
					}

					String bookCommand = "SELECT BookID, Name, Author, Price, RackNo, Status, Edition, Category, IsReserved, IssueDate" +
						$" FROM books WHERE IssuedTo = '{Members.CurrentUser}'";

					SqliteCommand bookcmd = new SqliteCommand(bookCommand, db);

					SqliteDataReader result = bookcmd.ExecuteReader();

					while (result.Read())
					{
						userBooks.Add(new Book()
						{
							BookId = result.GetString(0),
							Name = result.GetString(1),
							Author = result.GetString(2),
							Price = result.GetString(3),
							RackNo = result.GetString(4),
							Status = result.GetString(5),
							Edition = result.GetString(6),
							Category = result.GetString(7),
							IsReserved = result.GetString(8),
							IssueDate = result.GetString(9),
						});
					}
					db.Close();
				}
			}
			catch (Exception e)
			{
				await ShowDialogBox(e.Message);
			}
		}

		private async Task ShowDialogBox(string content)
		{
			ContentDialog ResultDialog = new ContentDialog
			{
				Title = "Message",
				Content = content,
				CloseButtonText = "Got It!"
			};

			await ResultDialog.ShowAsync();
		}
	}
}
