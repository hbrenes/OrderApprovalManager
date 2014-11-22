using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAMDataAccess.Repository
{
    public class OrdersRepository
    {
        OAMEntities db = new OAMEntities();

        public void Save()
        {
            db.SaveChanges();
        }

        public void Add(Orders order) 
        {
            db.Orders.Add(order);
        }

        public void Update(Orders order)
        {
            order.ModifiedDate = DateTime.Now;
            //db.Orders.Attach(order);
            db.SaveChanges();
        }

        public void UpdatePartial(int orderNo, int statusId, string assignedTo, string comment, string user)
        {
            var order = db.Orders.Where(c => c.OrderNumber == orderNo).FirstOrDefault();
            order.ModifiedDate = DateTime.Now;
            order.StatusId = statusId;
            order.AssignedTo = assignedTo;
            order.Comment = comment;

            OrderHistory h1 = new OrderHistory();
            h1.ActionBy = user;
            h1.ActionDate = DateTime.Now;
            h1.ActionName = "Order updated in the system.";
            h1.ActionValue = String.Format("The order has been updated in the system.");
            h1.OrderStatusId = statusId;

            db.SaveChanges();


           
        }

        public void AssignOrder(int orderNo, string user)
        {
            var order = db.Orders.Where(c => c.OrderNumber == orderNo).FirstOrDefault();
            order.ModifiedDate = DateTime.Now;
            order.AssignedTo = user;

            OrderHistory h1 = new OrderHistory();
            h1.ActionBy = user;
            h1.ActionDate = DateTime.Now;
            h1.ActionName = "Order updated in the system.";
            h1.ActionValue = String.Format("The order has been assigned to {0}.", user);
            h1.OrderStatusId = order.StatusId;

            db.SaveChanges();



        }

        public Orders GetByOrderNumber(int orderNumber)
        {
            var order = (from c in db.Orders where c.OrderNumber == orderNumber select c).FirstOrDefault();
            return order;
         
        }

        public IEnumerable GetOrdersAll()
        {
            var res = (from c in db.Orders
                      select new {
                            OrderDate = c.OrderDate,
                            OrderNumber = c.OrderNumber,
                            CustomerNumber = c.CustomerNumber,
                            CustomerName = c.CustomerName,
                            Status = c.OrderStatus.Description
                                    }).OrderByDescending(c => c.OrderDate);
            return res.ToList();
        }


        public List<OrderList> GetOrdersByStatus(int statusId, string user, bool byUser)
        {
            if (byUser)
            {
                var res = (from c in db.Orders
                           where c.StatusId == statusId && c.AssignedTo == user
                           select new
                           {
                               OrderDate = c.OrderDate,
                               OrderNumber = c.OrderNumber,
                               CustomerNumber = c.CustomerNumber,
                               CustomerName = c.CustomerName,
                               Status = c.OrderStatus.Description,
                               CreditStatus = c.CreditStatus,
                               AssignedTo = c.AssignedTo,
                               Selected = false
                           }).OrderByDescending(c => c.OrderDate);

                List<OrderList> result = new List<OrderList>();

                foreach (var item in res)
	            {
                    result.Add(new OrderList()
                        {
                            AssignedTo = item.AssignedTo,
                            CustomerName = item.CustomerName,
                            CustomerNumber = item.CustomerNumber,
                            OrderDate = item.OrderDate,
                            OrderNumber= item.OrderNumber,
                            Status = item.Status,
                            CreditStatus = item.CreditStatus,
                            Selected = false,

                        });
	            }

                return result;
            }
            else
            {
                var res = (from c in db.Orders
                           where c.StatusId == statusId
                           select new
                           {
                               OrderDate = c.OrderDate,
                               OrderNumber = c.OrderNumber,
                               CustomerNumber = c.CustomerNumber,
                               CustomerName = c.CustomerName,
                               Status = c.OrderStatus.Description,
                               CreditStatus = c.CreditStatus,
                               AssignedTo = c.AssignedTo,
                               Selected = false
                           }).OrderByDescending(c => c.OrderDate);

                List<OrderList> result = new List<OrderList>();

                foreach (var item in res)
                {
                    result.Add(new OrderList()
                    {
                        AssignedTo = item.AssignedTo,
                        CustomerName = item.CustomerName,
                        CustomerNumber = item.CustomerNumber,
                        OrderDate = item.OrderDate,
                        OrderNumber = item.OrderNumber,
                        Status = item.Status,
                        CreditStatus = item.CreditStatus,
                        Selected = false,

                    });
                }

                return result;
            }

            
        }

        public List<OrderList> GetOrdersByStatusAndDate(int statusId, string searchFrom, string searchTo, string user, bool byUser)
        {

            DateTime fromDate = Convert.ToDateTime(searchFrom);
            DateTime toDate = Convert.ToDateTime(searchTo);

            if (byUser)
            {
                var res = (from c in db.Orders
                           where c.StatusId == statusId && c.OrderDate >= fromDate && c.OrderDate <= toDate && c.AssignedTo == user
                           select new
                           {
                               OrderDate = c.OrderDate,
                               OrderNumber = c.OrderNumber,
                               CustomerNumber = c.CustomerNumber,
                               CustomerName = c.CustomerName,
                               Status = c.OrderStatus.Description,
                               CreditStatus = c.CreditStatus,
                               AssignedTo = c.AssignedTo,
                               Selected = false
                           }).OrderByDescending(c => c.OrderDate);

                List<OrderList> result = new List<OrderList>();

                foreach (var item in res)
                {
                    result.Add(new OrderList()
                    {
                        AssignedTo = item.AssignedTo,
                        CustomerName = item.CustomerName,
                        CustomerNumber = item.CustomerNumber,
                        OrderDate = item.OrderDate,
                        OrderNumber = item.OrderNumber,
                        Status = item.Status,
                        CreditStatus = item.CreditStatus,
                        Selected = false,

                    });
                }

                return result;
            }
            else
            {
                var res = (from c in db.Orders
                           where c.StatusId == statusId && c.OrderDate >= fromDate && c.OrderDate <= toDate
                           select new
                           {
                               OrderDate = c.OrderDate,
                               OrderNumber = c.OrderNumber,
                               CustomerNumber = c.CustomerNumber,
                               CustomerName = c.CustomerName,
                               Status = c.OrderStatus.Description,
                               CreditStatus = c.CreditStatus,
                               AssignedTo = c.AssignedTo,
                               Selected = false
                           }).OrderByDescending(c => c.OrderDate);

                List<OrderList> result = new List<OrderList>();

                foreach (var item in res)
                {
                    result.Add(new OrderList()
                    {
                        AssignedTo = item.AssignedTo,
                        CustomerName = item.CustomerName,
                        CustomerNumber = item.CustomerNumber,
                        OrderDate = item.OrderDate,
                        OrderNumber = item.OrderNumber,
                        Status = item.Status,
                        CreditStatus = item.CreditStatus,
                        Selected = false,

                    });
                }

                return result;
            }

            
        }

        public List<OrderList> GetOrdersByOrderNo(int orderNo)
        {
            var res = (from c in db.Orders
                       where c.OrderNumber == orderNo 
                       select new
                       {
                           OrderDate = c.OrderDate,
                           OrderNumber = c.OrderNumber,
                           CustomerNumber = c.CustomerNumber,
                           CustomerName = c.CustomerName,
                           Status = c.OrderStatus.Description,
                           CreditStatus = c.CreditStatus,
                           AssignedTo = c.AssignedTo,
                           Selected = false
                       }).OrderByDescending(c => c.OrderDate);

            List<OrderList> result = new List<OrderList>();

            foreach (var item in res)
            {
                result.Add(new OrderList()
                {
                    AssignedTo = item.AssignedTo,
                    CustomerName = item.CustomerName,
                    CustomerNumber = item.CustomerNumber,
                    OrderDate = item.OrderDate,
                    OrderNumber = item.OrderNumber,
                    Status = item.Status,
                    CreditStatus = item.CreditStatus,
                    Selected = false,

                });
            }

            return result;
        }




        public Orders GetOrderByOrderNo(int orderNo)
        {
            var res = db.Orders.Where(c => c.OrderNumber == orderNo).FirstOrDefault();
            return res;
        }

        public List<OrderFiles> GetFilesFromOrder(int orderNo)
        {
            var res = db.OrderFiles.Where(c => c.OrderNumber == orderNo).ToList();
            return res;
        }

        public List<HistoryList> GetHistory(int orderId)
        {
            var result = (from c in db.OrderHistory
                          where c.OrderId == orderId
                          select new
                          {
                              c.ActionBy,
                              c.ActionDate,
                              c.ActionName,
                              c.ActionValue,
                              c.OrderStatus.Description
                          }).ToList();

            List<HistoryList> res = new List<HistoryList>();

            foreach (var item in result)
            {
                res.Add(new HistoryList()
                {
                   ActionBy = item.ActionBy,
                   ActionDate = item.ActionDate,
                   ActionName = item.ActionName,
                   ActionValue = item.ActionValue,
                   Description = item.Description
                });
            }

            return res;
        }
        
        public Orders GetOrderForUpdate(int orderNo) 
        {
            return db.Orders.Where(c => c.OrderNumber == orderNo).FirstOrDefault();
        }

        public void AddHistory(OrderHistory history)
        {
            db.OrderHistory.Add(history);
            db.SaveChanges();
        }

        public int GetOrderIdByOrderNumber(int orderNumber)
        {
            return db.Orders.Where(c => c.OrderNumber == orderNumber).FirstOrDefault().OrderId;
        }


        public List<CommentList> GetOrderCommentsByOrderId(int orderId)
        {
            var result = db.OrderComments.Where(c => c.OrderId == orderId);
            List<CommentList> ret = new List<CommentList>();

            foreach (var item in result)
            {
                ret.Add(new CommentList()
                {
                    OrderComment = item.OrderComment,
                    OrderCommentDate = item.OrderCommentDate,
                    OrderCommentId = item.OrderCommentId,
                    OrderCommentUser = item.OrderCommentUser,
                    OrderId = item.OrderId
                });
            }

            return ret;
        }

        public void AddComment(OrderComments comment)
        {
            db.OrderComments.Add(comment);
            db.SaveChanges();
        }
    }

    
}
