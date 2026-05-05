using PingPay.Identity.Application.Abstractions;
using PingPay.Identity.Domain.Repositories;
using PingPay.Shared.Kernel.Primitives;

namespace PingPay.Identity.Application.UseCases.Login;

public record LoginMerchantCommand(string Email, string Password);
public record LoginMerchantResponse(string Token, string ApiKey);

public class LoginMerchantHandler(
    IMerchantRepository repository,
    IPasswordHasher passwordHasher,
    ITokenService tokenService)
{
    public async Task<Result<LoginMerchantResponse>> HandleAsync(
        LoginMerchantCommand command,
        CancellationToken ct = default)
    {
        var merchant = await repository.GetByEmailAsync(command.Email, ct);

        if (merchant is null || !passwordHasher.Verify(command.Password, merchant.PasswordHash))
            return Result<LoginMerchantResponse>.Failure("Credenciais inválidas.");

        if (merchant.Status == Domain.Enums.MerchantStatus.Suspended)
            return Result<LoginMerchantResponse>.Failure("Conta suspensa.");

        var token = tokenService.GenerateToken(merchant);

        return Result<LoginMerchantResponse>.Success(
            new LoginMerchantResponse(token, merchant.ApiKey));
    }
}
