using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services) //This is dependency injection container
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                //Sets password requirement for Registering (Identity) user
                option.Password.RequiredLength = 3;
                option.Password.RequiredUniqueChars = 0;
                option.Password.RequireDigit = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireNonAlphanumeric = false;
                option.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<AppDbContext>();

            #region Add MVC to this dependency Injection Container
            //services.AddMvcCore(); //This only included core methods of MVC and this is subset of MVCCore.
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;

                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }); 
     
            #endregion

            //using Singleton Dependency Injection. This creates single instance (of MockEmployeeRepository, in this case) per application. 
            services.AddScoped<IEmployeeRepository, SqlEmployeeRepository>();//This invokes MockEmployeeRepository implementation for IEmployeeRepository interface.
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                developerExceptionPageOptions.SourceCodeLineCount = 10; // This will show 10 lines of code from the line error occured.
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error");   
            }

            app.UseRouting();

            #region Setting default page

            #region One way of setting default page for application
            //app.UseDefaultFiles(); //This makes default.html, default.htm, index.html, or index.htm as a default page.

            //If you want to have custom page as your default page then use this
            //DefaultFilesOptions defaultFilesOption = new DefaultFilesOptions();
            //defaultFilesOption.DefaultFileNames.Clear();
            //defaultFilesOption.DefaultFileNames.Add("foo.html"); //adds foo.html as default page. 
            //app.UseDefaultFiles(defaultFilesOption); //Use this defaultFileOption parameter when you want to make custom page as default page.

            //app.UseStaticFiles(); //This helps to recognize and show static files like images, css, js. This should come after DefaultFileOption
            #endregion

            #region another way of setting default page for application
            //UseFileServer() - it includes the properties of UseStaticFiles(), UseDefaultFiles(), UseDirectoryFiles() (not used here)
            //app.UseFileServer(); //this make the default.html as default page.

            //If you want to have custom page as your default page then use this
            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("foo.html");//adds foo.html as default page. 
            //app.UseFileServer(fileServerOptions);
            #endregion

            #endregion
    
            app.UseStaticFiles();
            app.UseAuthentication();
            //app.UseMvcWithDefaultRoute(); //add mvc middleware to the reqeuest pipeline with default route which is home/index/id

            //This is custom routing. Here id is optional field
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });

            //app.UseMvc();

            #region UseEndpoints code provided in 3.1 core. This is new and does not exists in 2.2 version
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("/", async context =>
            //    {
            //        //await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
            //        await context.Response.WriteAsync(_config["MyKey"]);
            //    });
            //});
            #endregion

            #region implementation of app.use understand Middleware pipelines. This used in 2.2 version but still works in 3.1

            //app.Use(async (context, next) =>
            //{
            //    logger.LogInformation("MW1: Incoming Request");
            //    await next();
            //    logger.LogInformation("MW1: Outgoing Request");
            //});

            //app.Use(async (context, next) =>
            //{
            //    logger.LogInformation("MW2: Incoming Request");
            //    await next();
            //    logger.LogInformation("MW2: Outgoing Request");
            //});

            //app.Run(async (context) =>
            //{
            //    //throw new Exception("Some error Processing the request");
            //    await context.Response.WriteAsync("MW3: Request handled and response produced");
            //    await context.Response.WriteAsync("\n Hosting environment: " + env.EnvironmentName);
            //    logger.LogInformation("MW3: Request handled and response produced");
            //});

            #endregion
        }
    }
}
