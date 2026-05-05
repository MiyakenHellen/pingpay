using NSubstitute;
using PingPay.Identity.Application.Abstractions;
using PingPay.Identity.Application.UseCases.Register;
using PingPay.Identity.Domain.Repositories;
using PingPay.Shared.Kernel.Abstractions;

namespace PingPay.Identity.Tests.UseCases;

public class RegisterMerchantHandlerTests
{
    private readonly IMerchantRepository _repository = Substitute.For<IMerchantRepository>();
    private readonly IPasswordHasher _hasher = Substitute.For<IPasswordHasher>();
    private readonly IUnitOfWork _uow = Substitute.For<IUnitOfWork>();
    private readonly RegisterMerchantHandler _handler;

    public RegisterMerchantHandlerTests()
    {
        _handler = new RegisterMerchantHandler(_repository, _hasher, _uow);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenMerchantIsNew()
    {
        _repository.ExistsByEmailAsync(Arg.Any<string>()).Returns(false);
        _repository.ExistsByCnpjAsync(Arg.Any<string>()).Returns(false);
        _hasher.Hash(Arg.Any<string>()).Returns("hashed_password");

        var command = new RegisterMerchantCommand("Loja X", "loja@x.com", "12345678000199", "senha123");
        var result = await _handler.HandleAsync(command);

        Assert.True(result.IsSuccess);
        Assert.Equal("loja@x.com", result.Value!.Email);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEmailAlreadyExists()
    {
        _repository.ExistsByEmailAsync(Arg.Any<string>()).Returns(true);

        var command = new RegisterMerchantCommand("Loja X", "loja@x.com", "12345678000199", "senha123");
        var result = await _handler.HandleAsync(command);

        Assert.True(result.IsFailure);
        Assert.Equal("Email já cadastrado.", result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenCnpjAlreadyExists()
    {
        _repository.ExistsByEmailAsync(Arg.Any<string>()).Returns(false);
        _repository.ExistsByCnpjAsync(Arg.Any<string>()).Returns(true);

        var command = new RegisterMerchantCommand("Loja X", "loja@x.com", "12345678000199", "senha123");
        var result = await _handler.HandleAsync(command);

        Assert.True(result.IsFailure);
        Assert.Equal("CNPJ já cadastrado.", result.Error);
    }
}