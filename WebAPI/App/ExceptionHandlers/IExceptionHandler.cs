using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace WebAPI.App.ExceptionHandlers
{
    public interface IExceptionHandler
    {
        bool CanHandle(ExceptionContext context);
        Task HandleExceptionAsync(ExceptionContext context);
    }

}