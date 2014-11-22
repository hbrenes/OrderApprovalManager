using OAMDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAM.Models
{
    public class OrderHistoryViewModel
    {
        public string ActionBy { get; set; } 
        public DateTime ActionDate { get; set; }
        public string ActionName { get; set; }
        public string ActionValue { get; set; }
        public string OrderStatus { get; set; }            
    }
}