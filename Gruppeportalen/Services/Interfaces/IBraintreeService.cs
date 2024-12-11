using Braintree;

namespace Gruppeportalen.Services.Interfaces;

public interface IBraintreeService
{
    IBraintreeGateway CreateGateway();
    IBraintreeGateway GetGateway();
}