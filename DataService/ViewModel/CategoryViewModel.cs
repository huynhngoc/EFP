﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService.ViewModel
{
    public class CategoryViewModel
    {
        public int Level { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string CategoryName { get; set; }
        public List<CategoryViewModel> ChildCategory { get; set; }
        public CategoryViewModel()
        {
            ChildCategory = new List<CategoryViewModel>();
        }
    }
}
