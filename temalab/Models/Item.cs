using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace temalab.Models
{
    public class Item
    {
        public int ID { get; set; }
        [Required()]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name = "Price")]
        public int? BiggestTip { get; set; }
        public string DefaultPic { get; set; }
        public DateTime End { get; set; }

        //Foreign Keys
        [Display(Name = "Category")]
        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public ApplicationUser Owner { get; set; }
        public ApplicationUser Buyer { get; set; }
    }
}