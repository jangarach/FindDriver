using AutoMapper;
using FindDriver.Api.Mappings;
using FindDriver.Api.Model.DAL.Repositories;
using FindDriver.Api.Model.Services;
using FindDriver.Model;
using Microsoft.EntityFrameworkCore;

namespace FindDriver
{
    public class Startup : StartupBase
    {
        public Startup(IConfigurationRoot configuration)
        {
            Configuration = configuration;
        }
        public IConfigurationRoot Configuration { get; }
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Mapping
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<DbContext, ApplicationDbContext>();
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


            RegisterServices(services);
            
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
        public override void Configure(IApplicationBuilder app)
        {
            // Configure the HTTP request pipeline.
            if (app is WebApplication webApp && webApp.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }

        private void RegisterServices(IServiceCollection services)
        {
            RegisterRepositories(services);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICityService, CityService>();
        }

        private void RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
        }
    }
}
