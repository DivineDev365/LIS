using DataAccess.Models;
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
	public sealed partial class SearchPage : Page
	{
		public SearchPage()
		{
			this.InitializeComponent();
		}
		ObservableCollection<Book> QueryResult = new ObservableCollection<Book>();

		private async void SearchClicked(object sender, RoutedEventArgs e)
		{
			if(string.IsNullOrEmpty(SearchBox.Text))
			{
				await ShowDialogBox("Please enter book ID to search");
				return;
			}
			QueryResult.Clear();
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				try
				{
					String userCommand = "SELECT BookID, Name, Author, Price, RackNo, Status, Edition, Category, IssuedTo, IsReserved, ReservedTo" +
						$" FROM books WHERE Name LIKE '%{SearchBox.Text}%' OR Author LIKE '%{SearchBox.Text}%'";

					SqliteCommand cmd = new SqliteCommand(userCommand, db);

					SqliteDataReader result = cmd.ExecuteReader();

					while (result.Read())
					{
						QueryResult.Add(new Book()
						{
							BookId = result.GetString(0),
							Name = result.GetString(1),
							Author = result.GetString(2),
							Price = result.GetString(3),
							RackNo = result.GetString(4),
							Status = result.GetString(5),
							Edition = result.GetString(6),
							Category = result.GetString(7),
							IssuedTo = result.GetString(8),
							IsReserved = result.GetString(9),
							ReservedTo = result.GetString(10)
						});
					}
				}
				catch (Exception ex)
				{
					await ShowDialogBox(ex.Message);
				}

				db.Close();
				ResultsDataGrid.ItemsSource = QueryResult;
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

		private async void ReserveClicked(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(ReserveBox.Text))
			{
				await ShowDialogBox("Please Enter Book ID to reserve");
				return;
			}
			string bookstatus = string.Empty, reservestatus = string.Empty;

			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				try
				{
					string BookStatusCommand = "Select Status, IsReserved from books" +
								$" WHERE BookID = '{ReserveBox.Text}'";
					SqliteCommand bookcmd = new SqliteCommand(BookStatusCommand, db);
					SqliteDataReader BookResult = bookcmd.ExecuteReader();

					while (BookResult.Read())
					{
						bookstatus = BookResult.GetString(0);
						reservestatus = BookResult.GetString(1);
					}
					if(bookstatus.Equals("Issued",StringComparison.InvariantCultureIgnoreCase) 
						&& reservestatus.Equals("No",StringComparison.InvariantCultureIgnoreCase))
					{
						//reserve book
						string reservebookquery = $"UPDATE books SET IsReserved = 'Yes', ReservedTo = {Members.CurrentUser}," +
						" ReserveDate = datetime('now','localtime')" +
						$" WHERE BookID = '{ReserveBox.Text}'";
						SqliteCommand reservebookcmd = new SqliteCommand(reservebookquery, db);
						SqliteDataReader ReserveResult = reservebookcmd.ExecuteReader();

						if (ReserveResult.RecordsAffected > 0)
							await ShowDialogBox("Book Successfully Reserved");
					}
					else if (bookstatus.Equals("Issued", StringComparison.InvariantCultureIgnoreCase)
						&& reservestatus.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
					{
						await ShowDialogBox("Book is already reserved to someone.");
					}
					else
					{
						await ShowDialogBox("Book can not be Reserved.\nIt is available!\nGet it from the Library.");
					}
				}
				catch (Exception ex)
				{
					await ShowDialogBox(ex.Message);
				}

				db.Close();
			}
		}
	}
}
