using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserScreen.Models
{
    public class categoryViewModel
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public string subcategories { get; set; }
    }
}