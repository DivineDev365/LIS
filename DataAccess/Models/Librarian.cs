using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	public class Librarian
	{
		public string Name { get; set; }
		public string Password { get; set; }

		public void VerifyMember() { }
		public void IssueBook() { }
		public void CalculateFine() { }
		public void CreateBill() { }
		public void ReturnBook() { }
	}
}
