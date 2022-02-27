using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using GringottsBank.Internal.Contracts;
using GringottsBank.Core;
using Microsoft.EntityFrameworkCore;
using DataStore = GringottsBank.Internal.DataStore.Contracts;
using Sql = GringottsBank.Plugins.Data.Sql;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace GringottsBank
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GringottsBank", Version = "v1" });
            });
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddMvc(AddMVCOptions);
            
            services.AddTransient<ICustomerService, CustomerDataServices>();
            services.AddTransient<IAccountInfoService, AccountInfoService>();
            services.AddTransient<ITransactionService, AccountTransactionService>();
            services.AddTransient<ITransactionInfoService, TransactionInfoService>();

            services.AddDbContext<Sql.GringottsBankContext>(options => 
                        options.UseSqlServer(Configuration.GetConnectionString("Default")));
            services.AddTransient<IDataStore<DataStore.Customer>, Sql.SqlCustomerDataSource>();
            services.AddTransient<IDataStore<DataStore.Account>, Sql.SqlAccountsDataSource>();
            services.AddTransient<IDataStore<DataStore.Transaction>, Sql.SqlAccountTransactionsDataSource>();
            services.AddTransient<IRelatedDataStore<DataStore.Customer, List<DataStore.Account>>, Sql.SqlAccountsDataSource>();
            services.AddTransient<IRelatedDataStore<DataStore.Account, List<DataStore.Transaction>>, Sql.SqlAccountTransactionsDataSource>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GringottsBank v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void AddMVCOptions(MvcOptions options)
        {
            options.Filters.Add(new ModelStateFilterAttribute());
        }
    }

    public class ModelStateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(!context.ModelState.IsValid)
            {
                throw new System.Exception();
            }
        }
    }
}
