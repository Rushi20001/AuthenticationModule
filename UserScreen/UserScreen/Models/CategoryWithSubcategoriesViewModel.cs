using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserScreen.Models
{
    public class CategoryWithSubcategoriesViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<SubcategoryViewModel> Subcategories { get; set; }
    }
}