namespace BookWheel.Application.Auth
{
    public interface ITokenService
    {
        public Task<string> GetTokenAsync(string userName);
    }
}
