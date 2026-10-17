using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sienna.Domain.Abstractions.Results;

namespace Sienna.Tests.Common.Assertions
{
    /// <summary>Asserções para o padrão Result/Error do Sienna.</summary>
    public static class ResultAssert
    {
        public static T Success<T>(Result<T> result)
        {
            Success((Result)result);
            return result.Value;
        }

        public static void Success(Result result)
        {
            ArgumentNullException.ThrowIfNull(result);

            if (result.IsFailure)
                Assert.Fail($"Esperada sucesso, mas o resultado falhou com {result.Error.Code}: {result.Error.Message}");
        }

        public static Error Failure(Result result, string expectedCode, ErrorType? expectedType = null)
        {
            ArgumentNullException.ThrowIfNull(result);

            if (result.IsSuccess)
                Assert.Fail($"Esperava a falha {expectedCode}, mas o resultado foi um sucesso.");

            Assert.AreEqual(expectedCode, result.Error.Code);

            if (expectedType is ErrorType type)
                Assert.AreEqual(type, result.Error.ErrorType);

            return result.Error;
        }
    }
}
