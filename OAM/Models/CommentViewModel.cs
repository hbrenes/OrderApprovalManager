using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OAM.Models
{
    public class CommentViewModel
    {
        public int OrderCommentId { get; set; }
        public int OrderId { get; set; }
        public DateTime OrderCommentDate { get; set; }
        public string OrderComment { get; set; }
        public string OrderCommentUser { get; set; }
    }
}