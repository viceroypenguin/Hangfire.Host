using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hangfire.Host.Pages;

public class IndexModel : PageModel
{
	private readonly ILogger<IndexModel> _logger;

	public IndexModel(ILogger<IndexModel> logger)
	{
		_logger = logger;
	}

	public async Task OnGet()
	{
		var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
			.WithRedirectUri("/hangfire")
			.Build();

		await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
	}
}
