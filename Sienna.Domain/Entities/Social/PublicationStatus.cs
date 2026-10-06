namespace Sienna.Domain.Entities.Social
{
    /// <summary>
    /// Situação da publicação: "pendingApproval" (aguarda um gestor), "approved" (sai no horário agendado),
    /// "rejected" (reprovada), "canceled" (cancelada), "publishing" (sendo enviada),
    /// "published" (publicada) ou "failed" (falhou; o motivo fica em failureReason).
    /// </summary>
    public enum PublicationStatus
    {
        PendingApproval,
        Approved,
        Rejected,
        Canceled,
        Publishing,
        Published,
        Failed
    }
}
