using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using QNTM.API.Data;
using QNTM.API.Hubs;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;
using QNTM.API.Helpers;

namespace QNTM.API
{
    #pragma warning disable CS1591
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.AddAutoMapper();
            services.AddCors();
            services.AddSignalR();
            services.AddScoped<IAuthRepositroy, AuthRepository>();
            services.AddScoped<IQNTMRepository, QNTMRepository>();
            services.AddScoped<IUserHandler, UserHandler>();
            // services.AddSingleton<UserHandler>();
            services.AddSingleton<OnlineUsers>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                options.SaveToken = true;
                
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context => {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if(!string.IsNullOrEmpty(accessToken) 
                            && path.StartsWithSegments("/hubs"))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;


                        if ( (context.Request.Path.Value.StartsWith("/hubs")) 
                            || (context.Request.Path.Value.StartsWith("/pm")) 
                            && context.Request.Query.TryGetValue("token", out StringValues token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }, OnAuthenticationFailed = context => {
                        var ex = context.Exception;
                        return Task.CompletedTask;
                    }
                };
                
            });
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { 
                    Title = "QNTM API", Version = "v2", 
                    Description = "A cross platform chat client API",
                    Contact = new Contact 
                    {
                        Name = "anon",
                        Email = "anon@anon.com"
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
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
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "QNTM API v2");
                c.RoutePrefix = string.Empty;
            });
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:4200"));
            app.UseAuthentication();
            app.UseSignalR(routes => {
                routes.MapHub<ChatHub>("/hubs/chat");
                routes.MapHub<PrivateMessageHub>("/pm");
                routes.MapHub<PresenceHub>("/hubs/presence");
            });
            app.UseMvc();
        }
    }
    #pragma warning restore CS1591
}