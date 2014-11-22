using OAMDataAccess.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAM.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public int StatusId { get; set; }
        public string AssignedTo { get; set; }
        public int PoNumber { get; set; }
        public string Comment { get; set; }
        public List<OAMDataAccess.OrderFiles> Files { get; set; }
        public List<HistoryList> History { get; set; }
        public List<CommentList> Comments { get; set; }

    }
}