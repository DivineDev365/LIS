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
		public string PhoneNo { get; set; }
		public int BooksIssued { get; set; }
		public int MaxBookLimit { get; set; } 
		public int IssueMonthDuration { get; set; } 
	}

	public class UnderGrad : Members
	{
		public UnderGrad(Members b)
		{
			this.MemberId = b.MemberId;
			this.Name = b.Name;
			this.Password = b.Password;
			this.PhoneNo = b.PhoneNo;
			this.BooksIssued = b.BooksIssued;
		}
		public static new int MaxBookLimit { get; } = 2;
		public static new int IssueMonthDuration { get; } = 1;
	}
	public class PostGrad : Members
	{
		public PostGrad(Members b)
		{
			this.MemberId = b.MemberId;
			this.Name = b.Name;
			this.Password = b.Password;
			this.PhoneNo = b.PhoneNo;
			this.BooksIssued = b.BooksIssued;
		}
		public static new int MaxBookLimit { get; } = 4;
		public static new int IssueMonthDuration { get; } = 1;
	}
	public class ResearchScholar : Members
	{
		public ResearchScholar(Members b)
		{
			this.MemberId = b.MemberId;
			this.Name = b.Name;
			this.Password = b.Password;
			this.PhoneNo = b.PhoneNo;
			this.BooksIssued = b.BooksIssued;
		}
		public static new int MaxBookLimit { get; } = 6;
		public static new int IssueMonthDuration { get; } = 3;
	}
	public class Faculty : Members
	{
		public Faculty(Members b)
		{
			this.MemberId = b.MemberId;
			this.Name = b.Name;
			this.Password = b.Password;
			this.PhoneNo = b.PhoneNo;
			this.BooksIssued = b.BooksIssued;
		}
		public static new int MaxBookLimit { get; } = 10;
		public static new int IssueMonthDuration { get; } = 6;
	}
}
