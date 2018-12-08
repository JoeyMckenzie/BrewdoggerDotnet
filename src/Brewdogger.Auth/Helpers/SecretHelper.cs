namespace Brewdogger.Auth.Helpers
{
    public class SecretHelper : ISecretHelper
    {
        public SecretHelper(string secret)
        {
            Secret = secret;
        }

        public string Secret { get; }
    }
}