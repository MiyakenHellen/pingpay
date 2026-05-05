using PingPay.Identity.Domain.Entities;

namespace PingPay.Identity.Application.Abstractions;

public interface ITokenService
{
    string GenerateToken(Merchant merchant);
}