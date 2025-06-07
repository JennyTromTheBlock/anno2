using Microsoft.AspNetCore.Mvc.Filters;
using Application.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

namespace Presentation.ActionFilter;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthActionFilterAttribute : Attribute, IActionFilter
{
    private readonly IWebHostEnvironment _env;

    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Hent IWebHostEnvironment fra HttpContext services
        var env = context.HttpContext.RequestServices.GetService(typeof(IWebHostEnvironment)) as IWebHostEnvironment;

        if (env != null && env.IsDevelopment())
        {
            // Mock bruger i dev
            context.HttpContext.Items["User"] = new UserRequest { UserId = 12 };
            return;
        }

        var headers = context.HttpContext.Request.Headers;

        if (headers.TryGetValue("Authorization", out var token))
        {
            var bearerToken = token.ToString().Replace("Bearer ", "");

            // Her kan du validere token og sætte user
            // For nu: mock med en hardcoded check
            if (bearerToken == "realToken")
            {
                context.HttpContext.Items["User"] = new UserRequest { UserId = 123 };
                return;
            }
        }

        // Hvis vi når hertil, er auth ikke gyldig
        context.Result = new UnauthorizedResult();
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Ikke brug for noget her
    }
}

