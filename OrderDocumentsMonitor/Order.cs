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
    
    public partial class Order
    {
        public Order()
        {
            this.OrderHistories = new HashSet<OrderHistory>();
        }
    
        public int OrderId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public System.DateTime OrderDate { get; set; }
        public int CustomerNumber { get; set; }
        public string CustomerName { get; set; }
        public int PONumber { get; set; }
        public string DocumentType { get; set; }
        public int StatusId { get; set; }
        public string WSF_Inbox { get; set; }
        public string AssignedTo { get; set; }
        public string Comment { get; set; }
        public int OrderNumber { get; set; }
        public string CreditStatus { get; set; }
    
        public virtual ICollection<OrderHistory> OrderHistories { get; set; }
        public virtual OrderStatu OrderStatu { get; set; }
    }
}
