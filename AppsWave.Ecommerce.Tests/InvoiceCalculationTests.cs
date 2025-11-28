using AppsWave.Ecommerce.Domain.Entities;
using Xunit;

namespace AppsWave.Ecommerce.Tests
{
    public class InvoiceCalculationTests
    {
        [Fact]
        public void CalculateTotalAmount_ShouldReturnCorrectTotal()
        {
            // Arrange
            var invoice = new Invoice
            {
                UserId = 1,
                Date = DateTime.UtcNow,
                Details = new List<InvoiceDetail>
                {
                    new InvoiceDetail { ProductId = 1, Price = 100.00m, Quantity = 2 },
                    new InvoiceDetail { ProductId = 2, Price = 50.00m, Quantity = 3 },
                    new InvoiceDetail { ProductId = 3, Price = 25.00m, Quantity = 1 }
                }
            };

            // Act
            decimal totalAmount = invoice.Details.Sum(d => d.Price * d.Quantity);

            // Assert
            Assert.Equal(375.00m, totalAmount); // (100*2) + (50*3) + (25*1) = 200 + 150 + 25 = 375
        }

        [Fact]
        public void CalculateTotalAmount_ShouldReturnZero_WhenNoDetails()
        {
            // Arrange
            var invoice = new Invoice
            {
                UserId = 1,
                Date = DateTime.UtcNow,
                Details = new List<InvoiceDetail>()
            };

            // Act
            decimal totalAmount = invoice.Details.Sum(d => d.Price * d.Quantity);

            // Assert
            Assert.Equal(0m, totalAmount);
        }
    }
}

