//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OAMDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderHistory
    {
        public int OrderHistoryId { get; set; }
        public int OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public System.DateTime ActionDate { get; set; }
        public string ActionName { get; set; }
        public string ActionValue { get; set; }
        public string ActionBy { get; set; }
    
        public virtual Orders Orders { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
    }
}
