using GraphQLDemo.DataLoader;
using GraphQLDemo.Schema.Mutation;
using GraphQLDemo.Services.Courses;
using GraphQLDemo.Schema.Queries;
using GraphQLDemo.Schema.Subscriptions;
using GraphQLDemo.Services;
using GraphQLDemo.Services.Instructor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GraphQLDemo.FluentValidation;
using FluentValidation.AspNetCore;
using AppAny.HotChocolate.FluentValidation;

namespace GraphQLDemo
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<CourseTypeValidation>();
            services.AddFluentValidation();

            services.AddGraphQLServer()
                .AddQueryType<Query>()
                .AddMutationType<Mutation>()
                .AddSubscriptionType<Subscription>()
                .AddFiltering()
                .AddSorting()
                .AddProjections()
                .AddFluentValidation();
                //.AddAuthorization();

            services.AddInMemorySubscriptions();

            string connectionstring = _configuration.GetConnectionString("default");
            services.AddPooledDbContextFactory<SchoolDbContext>(o => o.UseSqlite(connectionstring));

            services.AddScoped<CourseRepository>();
            services.AddScoped<InstructorRepository>();
            services.AddScoped<InstructorDataLoader>();

            //var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = _configuration["Jwt:Issuer"],
            //        ValidAudience = _configuration["Jwt:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(key)
            //    };
            //});
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("AdminPolicy", policy =>
            //        policy.RequireRole("Admin"));
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //app.UseAuthentication();
            //app.UseAuthorization();
            app.UseWebSockets();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL("/graphql");
            });
        }
    }
}
