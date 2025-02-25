using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace BiometricFaceApi.Middleware
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env; // Injeção do ambiente
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Adiciona cabeçalhos de segurança apenas se estiver em produção
                if (_env.IsProduction())
                {
                    context.Response.OnStarting(() =>
                    {
                        // Adiciona cabeçalhos de segurança se ainda não estiverem presentes
                        if (!context.Response.Headers.ContainsKey("X-Content-Type-Options"))
                        {
                            context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                        }
                        if (!context.Response.Headers.ContainsKey("X-Frame-Options"))
                        {
                            context.Response.Headers.Add("X-Frame-Options", "DENY");
                        }
                        if (!context.Response.Headers.ContainsKey("Content-Security-Policy"))
                        {
                            context.Response.Headers.Add("Content-Security-Policy", "default-src 'self'; script-src 'self'; style-src 'self'; img-src 'self';");
                        }
                        return Task.CompletedTask;
                    });
                }

                // Passa a requisição para o próximo middleware
                await _next(context);
            }
            catch (Exception exception)
            {
                // Log de exceção
                _logger.LogError(exception, "Erro não tratado capturado no middleware global.");

                // Trata exceções inesperadas
                await HandleExceptionAsync(context, exception);
            }
        }
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string mensagem;
            string stackTrace = string.Empty;

           
            // Verifica o tipo de exceção e define o status HTTP apropriado
            if (exception is UnauthorizedAccessException)
            {
                status = HttpStatusCode.Unauthorized;
                mensagem = "Acesso não autorizado.";
            }
            else if (exception is ArgumentException)
            {
                status = HttpStatusCode.BadRequest;
                mensagem = "Erro nos argumentos fornecidos.";
            }
            else if (exception is KeyNotFoundException)
            {
                status = HttpStatusCode.NotFound;
                mensagem = exception.Message;
            }
            else
            {
                status = HttpStatusCode.InternalServerError;
                mensagem = "Erro interno do servidor.";
            }

            // Em desenvolvimento, incluir stack trace nos detalhes
            if (context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment())
            {
                stackTrace = exception.StackTrace; // Exibe o stack trace apenas em desenvolvimento
            }

            // Serializa apenas o código de status e a mensagem de erro
            var result = JsonSerializer.Serialize(new
            {
                status = (int)status,
                mensagem,
                stackTrace = stackTrace
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            return context.Response.WriteAsync(result);
        }
    }
}
