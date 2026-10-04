using Microsoft.Extensions.Options;
using Sienna.Application.Interfaces.Security;
using System.Security.Cryptography;
using System.Text;

namespace Sienna.Infrastructure.Security.SecretProtection
{
    internal sealed class AesSecretProvider : ISecretProvider
    {
        private const int NonceSize = 12, TagSize = 16;
        private readonly byte[] _key;

        public AesSecretProvider(IOptions<SecretProviderSettings> settings)
        {
            _key = Convert.FromBase64String(settings.Value.Key);

            if (_key.Length != 32)
                throw new InvalidOperationException("A chave precisa ter 32 bytes.");
        }

        public string Protect(string plainText)
        {
            var plain = Encoding.UTF8.GetBytes(plainText);
            var output = new byte[NonceSize + TagSize + plain.Length];

            var nonce = output.AsSpan(0, NonceSize);
            RandomNumberGenerator.Fill(nonce);

            using var aes = new AesGcm(_key, TagSize);
            aes.Encrypt(nonce, plain, output.AsSpan(NonceSize + TagSize), output.AsSpan(NonceSize, TagSize));

            return Convert.ToBase64String(output);
        }

        public string Unprotect(string protectedText)
        {
            var input = Convert.FromBase64String(protectedText);
            var plain = new byte[input.Length - NonceSize - TagSize];

            using var aes = new AesGcm(_key, TagSize);
            aes.Decrypt(input.AsSpan(0, NonceSize), input.AsSpan(NonceSize + TagSize), input.AsSpan(NonceSize, TagSize), plain);

            return Encoding.UTF8.GetString(plain);
        }
    }
}
