using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace temalab.Models
{
    public class Picture
    {
        public int ID { get; set; }
        public string Path { get; set; }
        public int? ItemID { get; set; }
        public virtual Item Item { get; set; }
    }
}