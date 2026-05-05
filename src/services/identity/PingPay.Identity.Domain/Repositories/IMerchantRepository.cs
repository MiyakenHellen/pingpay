using PingPay.Identity.Domain.Entities;

namespace PingPay.Identity.Domain.Repositories;

public interface IMerchantRepository
{
    Task<Merchant?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Merchant?> GetByEmailAsync(string email, CancellationToken ct = default);
    Task<Merchant?> GetByApiKeyAsync(string apiKey, CancellationToken ct = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
    Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken ct = default);
    Task AddAsync(Merchant merchant, CancellationToken ct = default);
    void Update(Merchant merchant);
}