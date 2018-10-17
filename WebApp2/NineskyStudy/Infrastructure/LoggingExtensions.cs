using Microsoft.Extensions.Logging;
using NineskyStudy.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NineskyStudy.Infrastructure
{
    public static class LoggingExtensions
    {
        private static readonly Action<ILogger, Exception> _indexPageRequested = LoggerMessage.Define(
                  LogLevel.Information, new EventId(1, nameof(CategoryController)), "GET request for BootStrapController");


        private static readonly Action<ILogger, string, Exception> _quoteAdded = LoggerMessage.Define<string>(
                  LogLevel.Information, new EventId(1, nameof(CategoryController)), "Quote added (Quote = '{Quote}')");

        public static void IndexPageRequested(this ILogger logger)
        {
            _indexPageRequested(logger, null);
        }

        public static void QuoteAdded(this ILogger logger,string quote)
        {
            _quoteAdded(logger, quote, null);
        }
    }

    public class LoggingMessage
    {



    }

}
