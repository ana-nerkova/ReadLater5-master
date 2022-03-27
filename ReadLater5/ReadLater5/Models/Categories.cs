using Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ReadLater5.Views
{
    public class Categories
    {
        public List<Category> _categories;

        public int SelectedCategoryId { get; set; }

        public IEnumerable<SelectListItem> ChosenCategory
        {
            get { return new SelectList(_categories, "ID", "Name"); }
        }
    }
}
