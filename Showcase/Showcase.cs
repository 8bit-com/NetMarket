using System;
using System.Collections.Generic;

namespace ShowcaseLib
{
    public class Showcase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int Price { get; set; }
        public DateTime DateCreate { get; set; }

        public List<Product> list;

        public Showcase()
        {
            Id = ShowcaseLib.Id.GetId(Type.Showcase);
            Name = $"NewShowcase{Id}";
            Volume = 10;
            DateCreate = DateTime.Now;
            list = new List<Product>();
        }
    }
}