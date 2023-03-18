using System;
using System.Collections.Generic;

namespace FlaschenpostApi.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string ShortDescription { get; set; }
        public double Price { get; set; }
        public string Unit { get; set; }
        public string PricePerUnitText { get; set; }
        public string Image { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public string BrandName { get; set; }
        public string Name { get; set; }
        public List<Article> Articles { get; set; }
        public string DescriptionText { get; set; }
    }
}
