
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;
using PersistenceLayer.Repositorisos;
using PersistenceLayer.Interfaces;
using System;
using PersistenceLayer.Entidades;

namespace TodoApi
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

            //services.AddDbContext<TodoContext>(opt =>
            // opt.UseInMemoryDatabase("TodoList"));
           
            services.AddDbContext<TodoContext>(options =>
               options.UseSqlServer(
                   Configuration.GetConnectionString("TodoContext")));
           
            services.AddControllers();
                
            services.AddSwaggerGen();

            services.AddSingleton(typeof(ICrud<>), typeof(CrudEF<>));

            services.AddSingleton<ICrudTodoItem, CrudTodoItemEF>();
            //services.AddTransient<ICrudTodoItem, CrudTodoItemEF>();

        }

        private void CrudTodoItemEF()
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

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
