using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	public class Members
	{
		[Key]
		public string MemberId { get; set; }
		public string Name { get; set; }
		public string Password { get; set; }
		public int PhoneNo { get; set; }
		public int BooksIssued { get; set; }
	}

	public class UnderGrad : Members
	{
		public int MaxBookLimit { get; } = 2;
		public int IssueMonthDuration { get; } = 1;
	}
	public class PostGrad : Members
	{
		public int MaxBookLimit { get; } = 4;
		public int IssueMonthDuration { get; } = 1;
	}
	public class ResearchScholar : Members
	{
		public int MaxBookLimit { get; } = 6;
		public int IssueMonthDuration { get; } = 3;
	}
	public class Faculty : Members
	{
		public int MaxBookLimit { get; } = 10;
		public int IssueMonthDuration { get; } = 6;
	}
}
