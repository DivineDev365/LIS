using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace LIS.ViewModels
{
	public class LoginPageViewModel
	{
		public static string user = string.Empty;
		private bool UserExists = false;

		private async void LoginCommand(string uname, string pwd, string tablename)
		{
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				String userCommand = $"SELECT EXISTS ( SELECT* FROM {tablename} " +
					$"WHERE ID = {uname} AND password = {pwd})";

				SqliteCommand cmd = new SqliteCommand(userCommand, db);

				SqliteDataReader result = cmd.ExecuteReader();
				while(result.Read())
				{
					UserExists = bool.Parse(result.GetString(0));
				}

				db.Close();
			}

		}

		public bool VerifyUser(String uname, String pwd)
		{
			if (string.Equals("admin", uname))
			{
				LoginCommand(uname, pwd, "librarian");
				user = "Admin";
			}
			else
			{
				LoginCommand(uname, pwd, "users");
				user = "other";
			}

			if (UserExists)
				return true;
            return false;
		}
	}
}
