using dotnetredis.Providers;
using dotnetredis.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Newtonsoft;

namespace dotnetredis
{
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "dotnetredis", Version = "v1"});
            });

            //Add Serializer configured for Redis
            //JSON config for Redis instance
            services.AddStackExchangeRedisExtensions<NewtonsoftSerializer>(options 
                => Configuration.GetSection("Redis").Get<RedisConfiguration>());
            
            //Add Redis healthcheck
            services.AddHealthChecks()
                .AddRedis(Configuration["Data:ConnectionStrings:Redis"]);
            
            //services.Configure<Redis>(Configuration);
            services.AddSingleton<RedisProvider>();
            services.AddTransient<BookService>();
            services.AddTransient<CartService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnetredis v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}