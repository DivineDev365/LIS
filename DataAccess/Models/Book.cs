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
		[Key]
		[Required]
		public UniqueId BookId { get; set; }
		[Required]
		public string Name { get; set; }
		[Required]
		public string Author { get; set; }
		[Required]
		public float price { get; set; }

		public string RackNo { get; set; }
		[Required]
		public string status { get; set; }
		public string Edition { get; set; }
		[Required]
		public string Category { get; set; }

		public void DisplayBookDetails() { }
		public void UpdateStatus() { }
	}
}
