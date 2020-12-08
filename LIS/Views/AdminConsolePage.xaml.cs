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
	public sealed partial class AdminConsolePage : Page
	{
		public AdminConsolePage()
		{
			this.InitializeComponent();
		}

		ObservableCollection<string> resultslist = new ObservableCollection<string>();

		private async void ExecuteClicked(object sender, RoutedEventArgs e)
		{
			//resultslist.Clear();
			try
			{
					await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
					string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
					using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
					{
						db.Open();

					//String userCommand = QueryBox.Text;
				
					SqliteCommand cmd = new SqliteCommand(QueryBox.Text, db);
				
					SqliteDataReader result = cmd.ExecuteReader();

					//for (int i=0;  result.Read(); i++)
					//{
					//	resultslist.Add(result.GetString(i));
					//}
					resultslist.Add("Successful!");

					db.Close();
					ResultList.ItemsSource = resultslist;
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

				
		}

		private void ClearClicked(object sender, RoutedEventArgs e)
		{
			resultslist.Clear();
		}
	}
}
