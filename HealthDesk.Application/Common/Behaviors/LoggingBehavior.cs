using MediatR;
using Microsoft.Extensions.Logging;

namespace HealthDesk.Application.Common.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var startTime = DateTime.UtcNow;
            var requestName = typeof(TRequest).Name;
            _logger.LogInformation("[Request Started] {RequestName} at {Time}", requestName, startTime);
            try
            {
                //zincirdeki bir sonraki davranışa veya handlera geçer
                var response = await next();

                var endTime = DateTime.UtcNow;
                var duration = endTime - startTime;

                _logger.LogInformation("[Request Completed] {RequestName} completed in {Duration} ms", requestName, duration.TotalMilliseconds);

                return response;
            } catch(Exception ex)
            {
                _logger.LogError("[Request Failed] {RequestName} threw an exception: {Message}", requestName, ex.Message);
                throw;
            }
        }
    }
}