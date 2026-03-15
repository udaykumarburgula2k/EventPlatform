namespace EventPlatform.Api.Infrastructure;

public interface IPaymentService
{
    Task<(bool Success, string Reference)> ChargeAsync(
        decimal amount,
        string paymentMethod,
        string paymentToken);
}