using PingPay.Identity.Domain.Enums;
using PingPay.Shared.Kernel.Primitives;

namespace PingPay.Identity.Domain.Entities;

public class Merchant : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Cnpj { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string ApiKey { get; private set; } = string.Empty;
    public string? WebhookURL { get; private set; } = string.Empty;
    public MerchantStatus Status { get; private set; } = MerchantStatus.PendingVerification;
    public SimulatorConfig SimulatorConfig { get; private set; } = new();

    public Merchant(string name, string email, string cnpj, string passwordHash, string apiKey, string? webhookURL)
    {
        Name = name;
        Email = email;
        Cnpj = cnpj;
        PasswordHash = passwordHash;
        ApiKey = GenerateApiKey() ;
        WebhookURL = webhookURL;
    }

    public static Merchant Create(string name, string email, string cnpj, string passwordHash)
    {
        return new Merchant(name, email, cnpj, passwordHash, GenerateApiKey(), null);
    }

    public void Activate() 
    {
        Status = MerchantStatus.Active;
        SetUpdatedAt();
    }

    public void Suspend() 
    { 
        Status = MerchantStatus.Suspended; 
        SetUpdatedAt(); 
    }
    
    public void RegenerateApiKey() 
    { 
        ApiKey = GenerateApiKey(); 
        SetUpdatedAt(); 
    }

    private static string GenerateApiKey() => 
        $"pk_{Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "").Replace("/", "")}";

}