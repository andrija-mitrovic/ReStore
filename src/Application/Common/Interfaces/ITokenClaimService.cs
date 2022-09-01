namespace Application.Common.Interfaces
{
    public interface ITokenClaimService
    {
        Task<string> GetTokenAsync(string userName, string email);
    }
}
