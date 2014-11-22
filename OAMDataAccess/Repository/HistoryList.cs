using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAMDataAccess.Repository
{
    public class HistoryList
    {
        public string ActionBy { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionName { get; set; }
        public string ActionValue { get; set; }
        public string Description { get; set; }
    }
}