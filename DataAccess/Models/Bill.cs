using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	public class Bill
	{
		public static int BillNo { get; set; }
		public string Date { get; set; }
		public string MemberId { get; set; }
		public double Amount { get; set; }
		public static double PenaltyRate { get; } = 10.0;

		public void CreateBill() { }
		public void UpdateBill() { }
	}
}
