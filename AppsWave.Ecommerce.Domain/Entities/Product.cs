namespace AppsWave.Ecommerce.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ArabicName { get; set; } = string.Empty;
        public string EnglishName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}

