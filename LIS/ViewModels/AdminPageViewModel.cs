using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using DataAccess.Models;

namespace LIS.ViewModels
{
	public class AdminPageViewModel
	{
		private string maxbook = string.Empty;
		private int issuemonth = 0;
		private bool IsSucceeded = false;
		
		private async void AddUserCommand(Members user)
		{
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				String userCommand = "INSERT INTO users (ID, Name, Password, PhoneNo, BooksIssued, MaxBookLimit, IssueMonthDuration)" +
					" VALUES" +
					$"('{user.MemberId}', '{user.Name}', '{user.Password}'," +
					$"'{user.PhoneNo}', '0', '{maxbook}', '{issuemonth}')";

				SqliteCommand cmd = new SqliteCommand(userCommand, db);

				SqliteDataReader result = cmd.ExecuteReader();

				if (!result.HasRows)
					IsSucceeded = true;
				

				db.Close();
			}

		}

		public bool AddUser(Members user, string category)
		{
			if (string.Equals(category, "Under Grad"))
			{
				maxbook = UnderGrad.MaxBookLimit.ToString();
				issuemonth = UnderGrad.IssueMonthDuration;
			}
			else if (string.Equals(category, "Post Grad"))
			{
				maxbook = PostGrad.MaxBookLimit.ToString();
				issuemonth = PostGrad.IssueMonthDuration;
			}
			else if (string.Equals(category, "Research Scholar"))
			{
				maxbook = ResearchScholar.MaxBookLimit.ToString();
				issuemonth = ResearchScholar.IssueMonthDuration;
			}
			else if (string.Equals(category, "Faculty"))
			{
				maxbook = Faculty.MaxBookLimit.ToString();
				issuemonth = Faculty.IssueMonthDuration;
			}

			AddUserCommand(user);
			return IsSucceeded;
		}
	}
}
