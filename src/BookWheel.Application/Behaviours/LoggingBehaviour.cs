using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWheel.Application.Behaviours
{
    public class LoggingBehaviour<TReq, TRes> : IPipelineBehavior<TReq, TRes>
    {
        public LoggingBehaviour(ILogger<LoggingBehaviour<TReq,TRes>> logger)
        {
            Logger = logger;
        }

        public ILogger<LoggingBehaviour<TReq, TRes>> Logger { get; }

        public async Task<TRes> Handle(TReq request, RequestHandlerDelegate<TRes> next, CancellationToken cancellationToken)
        {
            Logger.LogInformation($"Executing command/query:{typeof(TReq).Name}");
            var response = await next();
            Logger.LogInformation($"Executed command/query:{typeof(TReq).Name}");
            return response;
        }
    }
}
