using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAMDataAccess.Repository
{
    public class OrderFilesRepository
    {
        OAMEntities db = new OAMEntities();

        public void Save()
        {
            db.SaveChanges();
        }

        public void Add(OrderFiles order)
        {
            db.OrderFiles.Add(order);
        }


        public void Update(OrderFiles order)
        {
            db.OrderFiles.Attach(order);
        }

        public void UpdateFile(OrderFiles order)
        {
            db.OrderFiles.Attach(order);
            db.SaveChanges();
        }


        public OrderFiles GetByOrderIdAndFileName(int orderNumber, string fileName)
        {
            return db.OrderFiles.Where(c => c.OrderNumber == orderNumber  && c.FileName == fileName).FirstOrDefault();
        }
    }
}
