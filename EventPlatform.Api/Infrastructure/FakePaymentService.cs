namespace EventPlatform.Api.Infrastructure;

public class FakePaymentService : IPaymentService
{
    public Task<(bool Success, string Reference)> ChargeAsync(
        decimal amount,
        string paymentMethod,
        string paymentToken)
    {
        if (string.IsNullOrWhiteSpace(paymentToken))
            return Task.FromResult((false, string.Empty));

        var reference = $"PAY-{Guid.NewGuid():N}";
        return Task.FromResult((true, reference));
    }
}