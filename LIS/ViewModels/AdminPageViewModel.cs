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
		//private bool IsSucceeded = false;
		public string IssueBookStatus { get; set; }
		
		private async void AddUserCommand(Members user)
		{
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				String userCommand = "INSERT OR REPLACE INTO users (ID, Name, Password, PhoneNo, BooksIssued, MaxBookLimit, IssueMonthDuration)" +
					" VALUES" +
					$"('{user.MemberId}', '{user.Name}', '{user.Password}'," +
					$"'{user.PhoneNo}', '0', '{maxbook}', '{issuemonth}')";

				SqliteCommand cmd = new SqliteCommand(userCommand, db);

				SqliteDataReader result = cmd.ExecuteReader();

				db.Close();
			}

		}

		public void AddUser(Members user, string category)
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
			
		}

		public async void DeleteUserCommand(string id)
		{
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				String userCommand = "DELETE FROM users" +
					$" WHERE ID = '{id}'";

				SqliteCommand cmd = new SqliteCommand(userCommand, db);

				int result = cmd.ExecuteNonQuery();

				db.Close();
			}
		}
		
		public async void AddBookCommand(Book book)
		{
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				String userCommand = "INSERT OR REPLACE INTO books (BookID, Name, Author, Price, RackNo, Status, Edition, Category, IssuedTo, IsReserved)" +
					" VALUES" +
					$"('{book.BookId}', '{book.Name}', '{book.Author}', '{book.Price}', '{book.RackNo}', " +
					$"'{book.Status}', '{book.Edition}', '{book.Category}', 'None', 'No')";

				SqliteCommand cmd = new SqliteCommand(userCommand, db);

				SqliteDataReader result = cmd.ExecuteReader();

				db.Close();
			}
		}

		public async void DeleteBookCommand(string bookid)
		{
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				String userCommand = "DELETE FROM books" +
					$" WHERE BookID = '{bookid}'";

				SqliteCommand cmd = new SqliteCommand(userCommand, db);

				int result = cmd.ExecuteNonQuery();

				db.Close();
			}
		}

		public string IssueBook(string bookid, string memid)
		{
			IssueBookCommand(bookid, memid);
			return IssueBookStatus;
		}

		public async void IssueBookCommand(string bookid, string memid)
		{
			int booksissueed = 0, maxbooklimit = 0;
			string bookstatus = string.Empty, reservestatus = string.Empty;
			await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
			string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
			using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
			{
				db.Open();

				String GetUserCommand = "Select BooksIssued, MaxBookLimit from users" +
					$" WHERE ID = '{memid}'";

				SqliteCommand usercmd = new SqliteCommand(GetUserCommand, db);

				SqliteDataReader UserResult = usercmd.ExecuteReader();
				while(UserResult.Read())
				{
					booksissueed = int.Parse(UserResult.GetString(0));
					maxbooklimit = int.Parse(UserResult.GetString(1));
				}
				if(maxbooklimit - booksissueed >0)
				{
					string BookStatusCommand = "Select Status, IsReserved, RackNo from books" +
						$" WHERE BookID = '{bookid}'";
					SqliteCommand bookcmd = new SqliteCommand(BookStatusCommand, db);
					SqliteDataReader BookResult = bookcmd.ExecuteReader();

					while(BookResult.Read())
					{
						bookstatus = BookResult.GetString(0);
						reservestatus = BookResult.GetString(1);
					}
					if(string.Equals(bookstatus, "Available") && string.Equals(reservestatus, "No"))
					{
						string issuebookquery = $"UPDATE books SET Status = 'Issued', IssuedTo = '{memid}'," +
							$" IssueDate = datetime('now','localtime')" +
							$" WHERE BookID = '{bookid}'";
						SqliteCommand issuebookcmd = new SqliteCommand(issuebookquery, db);
						SqliteDataReader IssueResult = issuebookcmd.ExecuteReader();

						string incuserbooknoquery = $"UPDATE users SET BooksIssued = {booksissueed+1} " +
							$" WHERE ID = '{memid}'";
						SqliteCommand incuserbookcmd = new SqliteCommand(incuserbooknoquery, db);
						SqliteDataReader IncMembookResult = incuserbookcmd.ExecuteReader();

						if (IssueResult.HasRows && IncMembookResult.HasRows)
							IssueBookStatus = "Successfully Issued";
						else
							IssueBookStatus = "Else of Successfully issued";
					}
					else
					{
						IssueBookStatus = "Sorry, the Book is Issued or Reserved.";
					}
				}
				else
				{
					IssueBookStatus = "Max Book Limit Exceed. Return a book and try again.";
				}

				db.Close();
			}
		}
	}
}
