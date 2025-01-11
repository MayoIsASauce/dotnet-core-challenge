using System;
using System.Collections.Generic;
using CodeChallenge.Data;
using CodeChallenge.Repositories;
using CodeChallenge.Services;

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeChallenge.Config
{
    public class App
    {
        // Fix for bringing the reference data to the EmployeeService bundle
        private Dictionary<string, List<string>> initRefData;
        public WebApplication Configure(string[] args)
        {
            args ??= Array.Empty<string>();

            var builder = WebApplication.CreateBuilder(args);

            builder.UseEmployeeDB();
            
            AddServices(builder.Services);

            var app = builder.Build();

            var env = builder.Environment;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                SeedEmployeeDB();
            }

            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private void AddServices(IServiceCollection services)
        {
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmployeeRepository, EmployeeRespository>();
            // Add the service with the default values
            services.AddScoped<IReferenceRepository>(provider =>
                new ReferenceRepository(initRefData)
            );
            services.AddScoped<ICompensationService, CompensationService>();
            services.AddScoped<ICompensationRepository, CompensationRepository>();


            services.AddControllers();
        }

        private void SeedEmployeeDB()
        {
            EmployeeDataSeeder eds = new EmployeeDataSeeder(
                new EmployeeContext(
                    new DbContextOptionsBuilder<EmployeeContext>().UseInMemoryDatabase("EmployeeDB").Options
                ));
            eds.Seed().Wait();
            
            initRefData = eds.refData; // get the ref data from the class
        }
    }
}
