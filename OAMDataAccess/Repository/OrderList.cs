using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAMDataAccess.Repository
{
    public class OrderList
    {
        public DateTime OrderDate { get; set; }
        public int OrderNumber { get; set; }
        public int CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public string Status { get; set; }
        public string AssignedTo { get; set; }
        public bool Selected { get; set; }
        public string CreditStatus { get; set; }

    }
}