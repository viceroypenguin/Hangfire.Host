using Auth0.AspNetCore.Authentication;
using CommunityToolkit.Diagnostics;
using Hangfire;
using Hangfire.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json", optional: true);

builder.Services.AddHangfire(c => c
	.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
	.UseSimpleAssemblyNameTypeSerializer()
	.UseRecommendedSerializerSettings()
	.UseSqlServerStorage(builder.Configuration.GetConnectionString("Hangfire")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services
	.AddAuth0WebAppAuthentication(o =>
	{
		var domain = builder.Configuration["Auth0:Domain"];
		Guard.IsNotNull(domain);
		o.Domain = domain;

		var clientId = builder.Configuration["Auth0:ClientId"];
		Guard.IsNotNull(clientId);
		o.ClientId = clientId;
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
	Authorization = new[] { new HangfireAuthorizationFilter() },
});

app.MapRazorPages();

app.Run();
