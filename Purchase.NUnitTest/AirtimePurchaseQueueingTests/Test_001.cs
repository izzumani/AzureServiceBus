using Purchase.Application.DTO.Purchase;
using Purchase.Application.Commands;
using Purchase.Core.Events;

namespace Purchase.NUnitTest.AirtimePurchaseQueueingTests
{
    public class Tests : BaseTest
    {
        
        [Test]
        public async Task TEST_001_SEND_VALID_Airtime_Purchase_Queueing_Request()
        {
            var airtimePurchaseDTO = new AirtimePurchaseDTO()
            {
                TransactionId = new Guid(),
                MobileNumber = "+27825555555",
                MobileNetwork = "Vodacom",
                TraderId = "1234",
                TransactionAmount = 1000,
                ProductId = 1,
                CorrelationId = new Guid(),
                ParentId = null,
                CreatedDateTime = DateTime.UtcNow


            };

            var result = await SendAsync(new AirtimePurchaseQueueingCommand(airtimePurchaseDTO));

            Assert.That(string.Equals(result.ToString(),"Successfully"));
            
        }
    }
}