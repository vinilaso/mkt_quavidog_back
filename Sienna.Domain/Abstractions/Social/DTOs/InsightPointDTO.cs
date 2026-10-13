namespace Sienna.Domain.Abstractions.Social.DTOs
{
    /// <summary>
    /// Métricas de uma publicação em uma coleta. Valores nulos: métrica inexistente para o formato.
    /// </summary>
    public class InsightPointDTO
    {
        /// <summary>
        /// Quando as métricas foram coletadas, em UTC.
        /// </summary>
        public DateTime CollectedAt { get; set; }

        /// <summary>
        /// Visualizações da publicação.
        /// </summary>
        public int? Views { get; set; }

        /// <summary>
        /// Alcance da publicação (contas que a viram). Em stories, é a soma do alcance de cada story.
        /// </summary>
        public int? Reach { get; set; }

        /// <summary>
        /// Curtidas da publicação (disponível apenas para feed).
        /// </summary>
        public int? Likes { get; set; }

        /// <summary>
        /// Comentários da publicação (disponível apenas para feed).
        /// </summary>
        public int? Comments { get; set; }

        /// <summary>
        /// Compartilhamentos da publicação.
        /// </summary>
        public int? Shares { get; set; }

        /// <summary>
        /// Salvamentos da publicação (disponível apenas para feed).
        /// </summary>
        public int? Saved { get; set; }

        /// <summary>
        /// Respostas da publicação (disponível apenas para stories).
        /// </summary>
        public int? Replies { get; set; }

        /// <summary>
        /// Total de interações da publicação.
        /// </summary>
        public int? TotalInteractions { get; set; }
    }
}
