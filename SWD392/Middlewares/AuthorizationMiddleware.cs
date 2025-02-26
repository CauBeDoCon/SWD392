using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SWD392.DB;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IServiceScopeFactory _scopeFactory; // Dùng scope factory thay vì inject trực tiếp

    public AuthorizationMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
    {
        _next = next;
        _scopeFactory = scopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrEmpty(userId))
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await dbContext.Users.Include(u => u.Orders).FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);

                if (context.Request.Path.Value.Contains("/comment", StringComparison.OrdinalIgnoreCase) && !roles.Contains("Doctor"))
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Only doctors can comment.");
                    return;
                }

                if (context.Request.Path.Value.Contains("/review", StringComparison.OrdinalIgnoreCase) && !user.Orders.Any())
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("User must have at least one order to review.");
                    return;
                }
            }
        }

        await _next(context);
    }
}
