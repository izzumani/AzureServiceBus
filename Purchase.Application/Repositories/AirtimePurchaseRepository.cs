using EventBusServiceBus.EventDTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Purchase.Application.DTO.Aggregates;
using Purchase.Application.DTO.Purchase;
using Purchase.Application.Repositories.Interfaces;
using Purchase.Core.Entities;
using Purchase.Infrastructure.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.Repositories
{
    public class AirtimePurchaseRepository : IAirtimePurchaseRepository
    {
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly ILogger<AirtimePurchaseRepository> _logger;
        public AirtimePurchaseRepository(
                                            ILogger<AirtimePurchaseRepository> logger,
                                            IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public MobileProviderAndBarcode GetMobileNetworkDescriptionBarcode (int id)
        {
            try
            {
                MobileProviderAndBarcode mobileProduct = _applicationDbContext.MobileProduct

                            .Include(x => x.NetworkProvider)
                            .Select(x => new
                            MobileProviderAndBarcode
                            {
                                Id = x.Id,
                                Barcode = x.Barcode,
                                MobileNetwork = x.NetworkProvider.Description
                            }).Where(x => x.Id == id).FirstOrDefault();


                return mobileProduct;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex?.InnerException?.Message ?? ex?.Message}");
                return null;
            }
            
        }
        public MobileTransactionDTO CreateMobileTransaction(AirtimePurchaseDTO airtimePurchaseDTO)
        {
            try
            {
                _logger.LogDebug("Query Tansactions");

                return _applicationDbContext.MobileProduct
                    .Where(x=> x.Id == airtimePurchaseDTO.ProductId)
                            .Include(x=> x.Category)
                            .Include(x=> x.Commission)
                            .Include(x=> x.MobileProductType)
                            .Include(x=> x.NetworkProvider)
                            .Include(x=> x.ServiceProvider)
                            .Include(x=> x.SubType)
                                    
                        .Select( x=> new MobileTransactionDTO
                        { 
                                EntertainmentTransactionId = airtimePurchaseDTO.TransactionId,
                                TraderId= airtimePurchaseDTO.TraderId,
                                ServiceProviderUId = x.ServiceProviderUId,
                                ProductDescription = x.Description,
                                Barcode = x.Barcode,
                                MobileNumber = airtimePurchaseDTO.MobileNumber,
                                Cost = x.Cost,
                                SMS = x.SMS,
                                ThumbnailLogo = x.ThumbnailLogo,
                                BannerLogo =x.BannerLogo,
                                SubTypeDescription = x.SubType.Description,
                                MobileProductTypeDescription = x.MobileProductType.Description,
                                CategoryDescription = x.Category.Description,
                                ServiceProviderDescription  = x.ServiceProvider.Description,
                                NetworkProviderDescription = x.NetworkProvider.Description,
                                CommissionTotal = x.Commission.Total,
                                CommissionThrive = x.Commission.Thrive,
                                CommissionTrader = x.Commission.Trader,
                                CommissionType= x.Commission.Type,
                                CreatedDate = DateTimeOffset.UtcNow

                        }).FirstOrDefault();

        
                
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex?.InnerException?.Message ?? ex?.Message}");
                return null;
            }
            
            }

        public PurchaseAirtimeRequestDTO createAirtimePurchaseRequest(AirtimePurchaseDTO airtimePurchaseDTO)
        {
            // Todo: Get Product Details
            MobileProviderAndBarcode mobileProviderAndBarcode = new MobileProviderAndBarcode();
            mobileProviderAndBarcode = GetMobileNetworkDescriptionBarcode(airtimePurchaseDTO.ProductId);
            return new PurchaseAirtimeRequestDTO
            {
                transactGuid = Guid.NewGuid(),
                merchandiseType = "Airtime",
                transactAmount = airtimePurchaseDTO.TransactionAmount,
                messageType = "AuthorizationRequest",
                productID = new ProductID
                {
                    MeterNumber = null,
                    MobileNetwork = mobileProviderAndBarcode.MobileNetwork, //lookup based on the ProductId from the database
                    MobileNumber = "+27825555555",
                    Barcode = mobileProviderAndBarcode.Barcode, //lookup based on the ProductId from the database
                    BillPaymentIdType = "EasyPayNumber",
                    BillPaymentIdValue = "6009711054894"
                },

                transactDateTime = DateTime.UtcNow,
                messageDateTime = DateTime.Now,
                tenderData = new TenderData
                {

                    type = "CreditCard",
                    amount = 2000,
                    maskedPAN = "5221001018651945",
                    reference = "1234567"

                },
                merchantGroup = "Ecentric",
                merchantName = "EPSTest",
                comment = null,
                storeId = "880000000000001",
                storeLocation = "StoreLocation",
                terminalId = "88000101",
                operatorId = "OP007",
                basketId = "569",
                productData = null,
                additionalData = null,
                messageMode = null,
                CorrelationId = Guid.NewGuid(), //New Guid
                ParentId = airtimePurchaseDTO.CorrelationId,
                EventName = "MobileMartPurhaseRequest",
                CreateDateTime = DateTime.UtcNow
            };
        }
    }
}
