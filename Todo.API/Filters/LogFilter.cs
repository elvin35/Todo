using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;


namespace Todo.API.Filters
{
    public class LogFilter : Attribute, IActionFilter, IExceptionFilter
    {
        readonly ILogger _log;


        public LogFilter(ILogger logger)
        {
            _log = logger;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new System.NotImplementedException();
        }

        public void OnException(ExceptionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}
