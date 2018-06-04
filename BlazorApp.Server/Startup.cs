using System.Linq;
using System.Net.Mime;
using BlazorApp.Server.Interfaces;
using BlazorApp.Server.Services;
using Microsoft.AspNetCore.Blazor.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;

namespace BlazorApp.Server {
    public class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddResponseCompression(options => {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {
                    MediaTypeNames.Application.Octet,
                    WasmMediaTypeNames.Application.Wasm
                });
            });

            services.AddTransient<INumberService, NumberService>();
            services.AddSingleton<ILogger, MemoryLogger>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            app.UseResponseCompression();

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc(routes => { routes.MapRoute("default", "{controller}/{action}/{id?}"); });

            app.UseBlazor<Client.Program>();
        }
    }
}