using EventBusServiceBus.EventDTOs;
using Purchase.Application.DTO.Aggregates;
using Purchase.Application.DTO.Purchase;
using Purchase.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.Repositories.Interfaces
{
    public interface IAirtimePurchaseRepository
    {


        //IQueryable<MobileProduct> GetMobileProductsAsync(int id);
        MobileProviderAndBarcode GetMobileNetworkDescriptionBarcode(int id);
        PurchaseAirtimeRequestDTO createAirtimePurchaseRequest(AirtimePurchaseDTO airtimePurchaseDTO);

        MobileTransactionDTO CreateMobileTransaction(AirtimePurchaseDTO airtimePurchaseDTO);
    }
}
