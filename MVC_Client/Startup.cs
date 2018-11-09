using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MVC_Client
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    //identity server uri reference
                    options.Authority = "http://localhost:5000";
                    // in production configure to HTTP
                    options.RequireHttpsMetadata = false;

                    //Client specific information 
                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token";

                    //Must clear spoce to remove proprietary mappings
                    options.Scope.Clear();

                    //add scopes to request.  openid must be requested.
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("customProfile1");
                    options.Scope.Add("customProfile2");

                    //Map of specific claims.  Only a few are mapped automatically.  Map all that you need to be sure.
                    options.ClaimActions.MapUniqueJsonKey("sub", "sub");
                    options.ClaimActions.MapUniqueJsonKey("name", "name");
                    options.ClaimActions.MapUniqueJsonKey("foo", "foo");
                    options.ClaimActions.MapUniqueJsonKey("website", "website");
                    options.ClaimActions.MapUniqueJsonKey("role", "role");

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
