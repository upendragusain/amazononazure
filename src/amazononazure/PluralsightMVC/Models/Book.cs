﻿using System.Collections.Generic;

namespace PluralsightMVC.Models
{
    public class Book
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string AuthorName { get; set; }

        public string Publisher { get; set; }

        public DepartmentType Department { get; set; }

        public List<CategoryType> Categories { get; set; }

        public List<MediaContent> Media { get; set; }

        public List<CustomerReview> Reviews { get; set; }
    }
}
