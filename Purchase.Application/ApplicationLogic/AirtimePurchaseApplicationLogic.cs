using Microsoft.Extensions.Logging;
using Purchase.Application.DTO.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.ApplicationLogic
{
    public class AirtimePurchaseApplicationLogic
    {

        private readonly ILogger<AirtimePurchaseApplicationLogic> _logger;
        public AirtimePurchaseApplicationLogic(ILogger<AirtimePurchaseApplicationLogic> logger)
        {
            _logger = logger;
            
        }
        
    }
}
