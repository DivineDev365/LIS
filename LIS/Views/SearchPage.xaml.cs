using DataAccess.Models;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				try
				{
					String userCommand = "SELECT BookID, Name, Author, Price, RackNo, Status, Edition, Category, IssuedTo, IsReserved" +
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
							IsReserved = result.GetString(9)
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
				ResultsDataGrid.ItemsSource = QueryResult;
			}
		}
	}
}
