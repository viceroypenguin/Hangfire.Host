using Hangfire.Dashboard;
using System.Security.Claims;

namespace Hangfire.Host;

public sealed class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
	public bool Authorize(DashboardContext context) => context.GetHttpContext().User.HasClaim(ClaimTypes.Role, "Admin");
}
