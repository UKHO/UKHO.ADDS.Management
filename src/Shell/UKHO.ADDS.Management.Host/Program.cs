using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Radzen;
using UKHO.ADDS.Management.Host.Extensions;
using UKHO.ADDS.Management.Host.Shell;
using UKHO.ADDS.Management.Modules.Samples;
using UKHO.ADDS.Management.Modules.Samples.Registration;
using UKHO.ADDS.Management.Shell.Services;

namespace UKHO.ADDS.Management.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.AddServiceDefaults();

            // Registers the sample module
            builder.Services.AddSampleModule();

            builder.Services.AddScoped(sp =>
            {
                var nav = sp.GetRequiredService<NavigationManager>();
                return new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
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
                .AddKeycloakOpenIdConnect("keycloak", "ADDSManagement", oidcScheme, options =>
                {
                    options.ClientId = "ADDSManagementShell";
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.Scope.Add("addsmanagement:all");
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.NameClaimType = JwtRegisteredClaimNames.Name;
                    options.SaveTokens = true;
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);

            builder.Services.AddCascadingAuthenticationState();

            builder.Services.AddAuthorization();

            builder.Services.Configure<CircuitOptions>(options => { options.DetailedErrors = true; });

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

            Assembly[] moduleAssemblies = [typeof(SamplePage).Assembly];

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                .AddAdditionalAssemblies(moduleAssemblies);

            app.MapRazorPages();

            app.MapDefaultEndpoints();

            app.Run();
        }
    }
}
