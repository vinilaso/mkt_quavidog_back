namespace Sienna.Domain.Entities.Social
{
    /// <summary>
    /// Formato da publicação: "feed" (1 imagem = post simples; 2 a 10 = carrossel) ou
    /// "story" (cada imagem vira um story, publicados em sequência).
    /// </summary>
    public enum PublicationFormat
    {
        Feed,
        Story
    }
}
