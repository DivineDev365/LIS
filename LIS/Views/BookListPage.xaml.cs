using DataAccess.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.ObjectModel;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace LIS.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class BookListPage : Page
	{
		public BookListPage()
		{
			this.InitializeComponent();
			GetBookList();

		}
		ObservableCollection<Book> books = new ObservableCollection<Book>();
		private async void GetBookList()
		{
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				try
				{
					String userCommand = "SELECT BookID, Name, Author, Price, RackNo, Status, Edition, Category, " +
						"IssuedTo, IsReserved, ReservedTo, date(ReserveDate) " +
						" FROM books ORDER BY Category";

					SqliteCommand cmd = new SqliteCommand(userCommand, db);

					SqliteDataReader result = cmd.ExecuteReader();

					while (result.Read())
					{
						Book b = new Book()
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
							ReservedTo = result.GetString(10),
						};
						if (b.IsReserved.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
							b.ReserveDate = result.GetString(11);

						books.Add(b);
					}
				}
				catch(Exception e)
				{
					ContentDialog errorDialog = new ContentDialog
					{ 
						Title = "Message",
						Content = e.Message,
						CloseButtonText = "Got It!"
					};
					await errorDialog.ShowAsync();
				}

				db.Close();
				BookListGrid.ItemsSource = books;
			}
		}
	}
}
