using Microsoft.AspNetCore.Components.Server;
using Radzen;
using UKHO.ADDS.Management.Host.Shell;
using UKHO.ADDS.Management.Shell.Services;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using UKHO.ADDS.Management.Host.Extensions;
using Microsoft.AspNetCore.Components;

namespace UKHO.ADDS.Management.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults();

            builder.Services.AddScoped(sp => {
                var nav = sp.GetRequiredService<NavigationManager>();
                return new HttpClient
                {
                    BaseAddress = new Uri(nav.BaseUri)
                };
            });

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.WebHost.UseStaticWebAssets();

            builder.Services.AddRazorPages();
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddRadzenComponents();
            builder.Services.AddRadzenQueryStringThemeService();

            builder.Services.AddScoped<ModulePageService>();
            builder.Services.AddOutputCache();

            builder.Services.AddHttpContextAccessor().AddTransient<AuthorizationHandler>();

            var oidcScheme = OpenIdConnectDefaults.AuthenticationScheme;

            builder.Services.AddAuthentication(oidcScheme)
                .AddKeycloakOpenIdConnect("keycloak", realm: "WeatherShop", oidcScheme, options =>
                {
                    options.ClientId = "WeatherWeb";
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.Scope.Add("weather:all");
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                    options.SaveTokens = true;
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

            builder.Services.AddCascadingAuthenticationState();

            builder.Services.AddAuthorization();

            builder.Services.Configure<CircuitOptions>(options =>
            {
                options.DetailedErrors = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAntiforgery();

            app.MapLoginAndLogout();

            app.MapStaticAssets();

            app.MapRazorPages();
            app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

            app.MapDefaultEndpoints();
            
            app.Run();
        }
    }
}
