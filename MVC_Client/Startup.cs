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

                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token";

                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    //this must be cleared -see trouble shooting link in readme
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    //must be removed if you want these openid claims -see trouble shooting link in readme
                    options.ClaimActions.Remove("nonce");
                    options.ClaimActions.Remove("aud");
                    options.ClaimActions.Remove("azp");
                    options.ClaimActions.Remove("acr");
                    options.ClaimActions.Remove("amr");
                    options.ClaimActions.Remove("iss");
                    options.ClaimActions.Remove("iat");
                    options.ClaimActions.Remove("nbf");
                    options.ClaimActions.Remove("exp");
                    options.ClaimActions.Remove("at_hash");
                    options.ClaimActions.Remove("c_hash");
                    options.ClaimActions.Remove("ipaddr");
                    options.ClaimActions.Remove("auth_time");
                    options.ClaimActions.Remove("platf");
                    options.ClaimActions.Remove("ver");

                    options.ClaimActions.MapUniqueJsonKey("profile", "profile");

                    options.Scope.Add("api1");
                    options.Scope.Add("offline_access");
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
