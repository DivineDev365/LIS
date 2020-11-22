using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DataAccess
{
	public static class InitializeDB
	{
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("University.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "University.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String usertableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS users (ID TEXT PRIMARY KEY, " +
                    "Name TEXT," +
                    "Password TEXT," +
                    "PhoneNo INT," +
                    "BooksIssued INT," +
                    "MaxBookLimit INT," +
                    "IssueMonthDuration INT)";

                SqliteCommand createuserTable = new SqliteCommand(usertableCommand, db);

                createuserTable.ExecuteReader();

                String booktableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS books (BookID TEXT PRIMARY KEY, " +
                    "Name TEXT," +
                    "Author TEXT," +
                    "Price INT," + 
                    "RackNo TEXT," +
                    "Edition TEXT," +
                    "Category TEXT," +
                    "Status TEXT," +
                    "IssuedTo TEXT," +
                    "IsReserved TEXT," +
                    "IssueDate datetime," +
                    "ReservedTo TEXT, " +
                    "IssueCount INT," +
                    "ReserveDate datetime)";

                SqliteCommand createbookTable = new SqliteCommand(booktableCommand, db);

                createbookTable.ExecuteReader();
            
                String librariantableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS librarian (ID TEXT PRIMARY KEY," +
                    "Name TEXT," +
                    "Password TEXT )";

                SqliteCommand createlibrarianTable = new SqliteCommand(librariantableCommand, db);

                createlibrarianTable.ExecuteReader();

                string AddLibcommand = "INSERT OR IGNORE INTO librarian VALUES ('Admin', 'Admin', 'Admin')";
                SqliteCommand updatelibrarian = new SqliteCommand(AddLibcommand,db);
                updatelibrarian.ExecuteReader();
         
                String transtableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS trans (TransID TEXT PRIMARY KEY, " +
                    "MemberID TEXT," +
                    "BookID TEXT," +
                    "DateOfIssue TEXT," +
                    "DueDate TEXT )";

                SqliteCommand createtransTable = new SqliteCommand(transtableCommand, db);

                createtransTable.ExecuteReader();
           
                String billtableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS bill (BillNo TEXT PRIMARY KEY, " +
                    "Date TEXT," +
                    "MemberId TEXT," +
                    "Amount INT )";  //store in paisa and divide by 100 to convert to Rs


                SqliteCommand createbillTable = new SqliteCommand(billtableCommand, db);

                createbillTable.ExecuteReader();
            }
        }
    }
}
