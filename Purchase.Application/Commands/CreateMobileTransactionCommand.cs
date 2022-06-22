using AutoMapper;
using EventBusServiceBus.EventDTOs;
using MediatR;
using Microsoft.Extensions.Logging;
using Purchase.Application.DTO.Purchase;
using Purchase.Application.Repositories.Interfaces;
using Purchase.Core.Entities;
using Purchase.Infrastructure.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Purchase.Application.Commands
{
    public class CreateMobileTransactionCommand : IRequest<string>
    {
        
        public AirtimePurchaseDTO _airtimePurchaseDTO { get; }
        
        public CreateMobileTransactionCommand(AirtimePurchaseDTO airtimePurchaseDTO)
        {

            _airtimePurchaseDTO = airtimePurchaseDTO;
            
        }
    }

    public class CreateMobileTransactionCommandCommandHandler : IRequestHandler<CreateMobileTransactionCommand, string>
    {
        
        private readonly IMediator _mediator;
        private readonly ILogger<CreateMobileTransactionCommandCommandHandler> _logger;
        private readonly IApplicationDbContext _applicationDbContext;
        private readonly IAirtimePurchaseRepository _airtimePurchaseRepository;
        private readonly IMapper _mapper;
        public CreateMobileTransactionCommandCommandHandler(IMediator mediator,
                                                    ILogger<CreateMobileTransactionCommandCommandHandler> logger,
                                                     IApplicationDbContext applicationDbContext,
                                                      IAirtimePurchaseRepository airtimePurchaseRepository
                                                    , IMapper mapper
                                                        )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _applicationDbContext = applicationDbContext;
            _airtimePurchaseRepository = airtimePurchaseRepository;
            _mapper = mapper;
        }

        public async Task<string> Handle(CreateMobileTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating Mobile Trasnacton");
            var _repository =  _airtimePurchaseRepository.CreateMobileTransaction(request._airtimePurchaseDTO);

            MobileTransaction mobileTransaction = _mapper.Map<MobileTransaction>(_repository);
            await _applicationDbContext.MobileTransaction.AddAsync(mobileTransaction);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return string.Empty;

        }
    }
}