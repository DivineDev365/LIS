using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataAccess.Models
{
	public class Transaction
	{
		public UniqueId TransId { get; set; }
		public string MemberId { get; set; }
		public string BookId { get; set; }
		public DateTime IssueTime { get; set; }

		public void CreateTransacton() { }
		public void DeleteTransaction() { }
		public void RetrieveTransaction() { }
	}
}
