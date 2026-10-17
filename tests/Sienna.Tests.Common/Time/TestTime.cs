namespace Sienna.Tests.Common.Time
{
    public static class TestTime
    {
        /// <summary>Instante fixo usado como "agora" nos testes: 12/10/2026, 12:00 em Brasília.</summary>
        public static readonly DateTime Now = new(2026, 10, 12, 15, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Converte um horário de Brasília para UTC. Brasília está em UTC-3, sem horário de verão desde 2019.
        /// </summary>

        public static DateTime Brasilia(int year, int month, int day, int hour, int minute = 0) =>
            new DateTime(year, month, day, hour, minute, 0, DateTimeKind.Utc).AddHours(3);
    }
}
