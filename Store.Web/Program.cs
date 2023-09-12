using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Data;
using Store.Data.Models;
using Store.Infostructure;
using Store.Infrastructure.AutoMapper;
using Store.Infrastructure.Middlewares;
using Store.Infrastructure.Servicess;
using Store.Infrastructure.Servicess.CartServicess;
using Store.Infrastructure.Servicess.CategoriesServicess;
using Store.Infrastructure.Servicess.OrderHeaderServicess;
using Store.Infrastructure.Servicess.ProductsServicess;
using Store.Infrastructure.Servicess.ShoppingCartServicess;
using Store.Infrastructure.Servicess.UserServicess;
using Store.Web.Data;
using Store.Web.Hubs;
using Stripe;

namespace Store.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.Configure<SettingsStrip>(builder.Configuration.GetSection("Strip"));

            builder.Services.AddIdentity<User, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequireDigit = false;
                config.Password.RequiredLength = 6;
                config.Password.RequireLowercase = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
                config.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders().AddDefaultUI();
            builder.Services.AddSignalR();

            builder.Services.AddSession(options => {
				options.IdleTimeout = TimeSpan.FromMinutes(100);
				options.Cookie.HttpOnly = true;
				options.Cookie.IsEssential = true;
			});

			builder.Services.AddRazorPages();
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<ICategoryServices, CategoryServiecs>();
            builder.Services.AddScoped<IShoppingCartServicess, ShoppingCartServicess>();
            builder.Services.AddScoped<IProductServicess, ProductServices>();
            builder.Services.AddScoped<IUserServices, UserServices>();
			builder.Services.AddScoped<IOrderHeaderServiecs, OrderHeaderServiecs>();

			builder.Services.AddScoped<ICartServices, CartServices>();


            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Strip:Secretkey").Get<string>();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseExceptionHandler(opts => opts.UseMiddleware<ExceptionHandler>());

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Public}/{action=Index}/{id?}");
            app.MapRazorPages();

           
            app.MapHub<OrderHubs>("/hubs/order");

            app.SeedDb().Run();
        }
    }
}