using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace PocketBudget.Models
{
    public class BillContext : DbContext
    {
        public DbSet<Bill> Bills { get; set; }

        public BillContext()
            : base("PocketBudgetDataBase")
        { }
    }
}
