

using JobMatch.Models;
using JobMatch.Services;
using Microsoft.EntityFrameworkCore;

namespace JobMatch
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            builder.Services.AddDbContext<JobMatchContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("ConnectedDb")
                    )); ;

            builder.Services.AddSession();
            builder.Services.AddControllersWithViews().AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Add("/Views/Shared/PartialViews/{0}.cshtml");
            }); ;
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddScoped<ResumeService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSingleton<UserService>(); // Đăng ký UserService





            var app = builder.Build();
            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
