namespace AppsWave.Ecommerce.Domain.Entities
{
    public class Invoice
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public List<InvoiceDetail> Details { get; set; } = new();
    }
}

