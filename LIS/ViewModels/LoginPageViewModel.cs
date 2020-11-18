using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using DataAccess.Models;
using Windows.UI.Xaml.Controls;

namespace LIS.ViewModels
{
	public class LoginPageViewModel
	{
		//public static string user = string.Empty;
		public bool UserExists = false;

		public async Task VerifyUserAsync(String uid, String pwd)
		{
			
			string tablename = string.Empty;

			if (uid.Equals("admin", StringComparison.InvariantCultureIgnoreCase))
			{
				//await LoginCommand("Admin", pwd, "librarian");
				uid = "Admin";
				tablename = "librarian";
				Members.CurrentUser = "Admin";
			}
			else
			{
				//await LoginCommand(uid, pwd, "users");
				tablename = "users";
				Members.CurrentUser = uid;
			}

			try
			{
				await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
				string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
				using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
				{
					db.Open();

					String userCommand = $"SELECT EXISTS ( SELECT * FROM {tablename} " +
						$"WHERE ID = '{uid}' AND Password = '{pwd}')";

					SqliteCommand cmd = new SqliteCommand(userCommand, db);

					SqliteDataReader result = cmd.ExecuteReader();
					while (result.Read())
					{
						if (result.GetString(0).Equals("1"))
							UserExists = true;
						else
							UserExists = false;
						
					}

					db.Close();
				}
			}
			catch (Exception e)
			{
				await ShowDialogBox(e.Message);
			}

			//if (UserExists)
			//	return true;
   //         return false;
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
