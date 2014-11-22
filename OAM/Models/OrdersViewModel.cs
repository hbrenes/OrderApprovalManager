using OAMDataAccess.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAM.Models
{
    public class OrdersViewModel
    {
        public string userToAssign;
        public List<OrderList> Orders { get; set; }

    }
}