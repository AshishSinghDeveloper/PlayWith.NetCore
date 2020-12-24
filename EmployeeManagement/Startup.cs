using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
            FileServerOptions fileServerOptions = new FileServerOptions();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("foo.html");//adds foo.html as default page. 
            app.UseFileServer(fileServerOptions);
            #endregion

            #endregion

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

            app.Use(async (context, next) =>
            {
                logger.LogInformation("MW1: Incoming Request");
                await next();
                logger.LogInformation("MW1: Outgoing Request");
            });

            app.Use(async (context, next) =>
            {
                logger.LogInformation("MW2: Incoming Request");
                await next();
                logger.LogInformation("MW2: Outgoing Request");
            });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("MW3: Request handled and response produced");
                logger.LogInformation("MW3: Request handled and response produced");
            });

            #endregion
        }
    }
}
