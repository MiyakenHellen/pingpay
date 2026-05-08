using Microsoft.EntityFrameworkCore;
using PingPay.Identity.Domain.Entities;
using PingPay.Identity.Domain.Repositories;
using PingPay.Identity.Infrastructure.Data;

namespace PingPay.Identity.Infrastructure.Repositories;

public class MerchantRepository(IdentityDbContext context) : IMerchantRepository
{
    private readonly IdentityDbContext _context = context;

    public Task<Merchant?> GetByIdAsync(Guid id, CancellationToken ct = default) =>
     _context.Merchants.FirstOrDefaultAsync(m => m.Id == id, ct);

    public Task<Merchant?> GetByEmailAsync(string email, CancellationToken ct = default) =>
     _context.Merchants.FirstOrDefaultAsync(m => m.Email == email, ct);
    
    public Task<Merchant?> GetByApiKeyAsync(string apiKey, CancellationToken ct = default) =>
     _context.Merchants.FirstOrDefaultAsync(m => m.ApiKey == apiKey, ct);
    
    public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default) =>
     _context.Merchants.AnyAsync(m => m.Email == email, ct);

    public Task<bool> ExistsByCnpjAsync(string cnpj, CancellationToken ct = default) =>
     _context.Merchants.AnyAsync(m => m.Cnpj == cnpj, ct);

    public async Task AddAsync(Merchant merchant, CancellationToken ct = default) =>
     await _context.Merchants.AddAsync(merchant, ct);
    
    public void Update(Merchant merchant) =>
        _context.Merchants.Update(merchant);
}