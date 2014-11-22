using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAMDataAccess.Repository
{
    public class CommentList
    {
        public int OrderCommentId { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderCommentDate { get; set; }
        public string OrderComment { get; set; }
        public string OrderCommentUser { get; set; }
    }
}
