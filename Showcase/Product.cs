namespace ShowcaseLib
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public int Price { get; set; }

        public Product()
        {
            Id = ShowcaseLib.Id.GetId(Type.Product);
            Name = $"NewProduct{Id}";
            Volume = 1;
        }
    }
}