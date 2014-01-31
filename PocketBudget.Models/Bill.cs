using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketBudget.Models
{
    public class Bill
    {
        public int BillId { get; set; }
        public string Name { get; set; }
        public double Fee { get; set; }
        public int FirstDayToPay { get; set; }
        public int LastDayToPay { get; set; }
        public int HasFixedFee { get; set; }
        public double TotalAmount { get; set; }
    }
}
