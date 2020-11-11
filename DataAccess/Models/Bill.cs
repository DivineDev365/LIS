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
		public DateTime Date { get; set; }
		public string MemberId { get; set; }
		public float Amount { get; set; }

		public void CreateBill() { }
		public void UpdateBill() { }
	}
}
