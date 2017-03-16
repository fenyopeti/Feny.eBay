using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace temalab.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        [Required()]
        [Display(Name="Category")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        public List<Item> Items { get; set; }
    }
}