namespace SmartShoppingAssistant.DataAcces.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = null;
        public string Description { get; set; } = null;
        public string ImageUrl { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }

        public ICollection<Category> Categories { get; set; }  = new List<Category>();
    }
}
