using PingPay.Identity.Application.Abstractions;
using PingPay.Identity.Domain.Entities;
using PingPay.Identity.Domain.Repositories;
using PingPay.Shared.Kernel.Abstractions;
using PingPay.Shared.Kernel.Primitives;

namespace PingPay.Identity.Application.UseCases.Register;

public record RegisterMerchantCommand(string Name, string Email, string Cnpj, string Password);
public record RegisterMerchantResponse(Guid Id, string Email, string ApiKey);

public class RegisterMerchantHandler(
    IMerchantRepository repository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
{
    public async Task<Result<RegisterMerchantResponse>> HandleAsync(
        RegisterMerchantCommand command,
        CancellationToken ct = default)
    {
        if (await repository.ExistsByEmailAsync(command.Email, ct))
            return Result<RegisterMerchantResponse>.Failure("Email já cadastrado.");

        if (await repository.ExistsByCnpjAsync(command.Cnpj, ct))
            return Result<RegisterMerchantResponse>.Failure("CNPJ já cadastrado.");

        var passwordHash = passwordHasher.Hash(command.Password);
        var merchant = Merchant.Create(command.Name, command.Email, command.Cnpj, passwordHash);

        await repository.AddAsync(merchant, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return Result<RegisterMerchantResponse>.Success(
            new RegisterMerchantResponse(merchant.Id, merchant.Email, merchant.ApiKey));
    }
}
