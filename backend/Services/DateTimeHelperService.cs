using NodaTime;

namespace BiometricFaceApi.Services
{
    public class DateTimeHelperService
    {
        // Método que retorna a hora atual no fuso horário de Manaus
        public static DateTime GetManausCurrentDateTime()
        {
            // Obtém o fuso horário de Manaus
            DateTimeZone manausTimeZone = DateTimeZoneProviders.Tzdb["America/Manaus"];

            // Obtém o instante atual (em UTC)
            Instant now = SystemClock.Instance.GetCurrentInstant();

            // Converte o instante UTC para o fuso horário de Manaus
            ZonedDateTime manausDateTime = now.InZone(manausTimeZone);

            // Converte para DateTime sem o fuso horário específico
            return manausDateTime.ToDateTimeUnspecified();
        }
    }
}
