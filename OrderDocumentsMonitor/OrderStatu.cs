//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrderDocumentsMonitor
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderStatu
    {
        public OrderStatu()
        {
            this.OrderHistories = new HashSet<OrderHistory>();
            this.Orders = new HashSet<Order>();
        }
    
        public int OrderStatusId { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
