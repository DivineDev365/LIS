using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DataAccess.Models
{
	public class Book
	{
		
		public string BookId { get; set; }
		
		public string Name { get; set; }
		
		public string Author { get; set; }
		
		public string Price { get; set; }

		public string RackNo { get; set; }
		
		public string Status { get; set; }
		public string Edition { get; set; }
		
		public string Category { get; set; }

		public string IssuedTo { get; set; }
		public string IsReserved { get; set; }
		public string IssueDate { get; set; }

		public void DisplayBookDetails() { }
		public void UpdateStatus() { }
	}
}
