﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Borigran.OneData.WebClient.Pipeline
{
    public class ErrorHandlingMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ErrorHandlingMiddleWare> logger;

        public ErrorHandlingMiddleWare(RequestDelegate next, ILogger<ErrorHandlingMiddleWare> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                logger.LogInformation("It's OK");
                next.Invoke(context);
            }
            catch(Exception ex)
            {
                LogException(ex);
            }
        }

        private void LogException(Exception ex)
        {
            logger.LogError(ex, ex.Message);
        }
    }
}
