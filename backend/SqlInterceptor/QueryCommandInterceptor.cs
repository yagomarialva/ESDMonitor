using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace BiometricFaceApi.SqlInterceptor
{
    public class QueryCommandInterceptor : DbCommandInterceptor
    {
        public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            command.CommandText = command.CommandText.Replace('"'.ToString(), string.Empty);
            command.CommandText = command.CommandText.Replace("  ", " ");

            foreach (var param in command.Parameters)
            {
                var prop = param.GetType().GetProperty("Value");
                if (prop is null || prop.GetValue(param) is not string)
                    continue;

                prop.SetValue(param, $"'{prop.GetValue(param)!}'");
                command.CommandText = command.CommandText.Replace(
                    param.GetType().GetProperty("ParameterName")!.GetValue(param)!.ToString()!,
                    prop.GetValue(param)!.ToString());
            }
            return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
        }
    }
}
