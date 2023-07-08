using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Repositories;
using Serilog;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IRepository,MyRepository>();
builder.Services.AddControllersWithViews();

//PetShop data
builder.Services.AddDbContext<PetShopContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:DefaultConnection"]));

//User Data
builder.Services.AddDbContext<AuthenticationContext>(options => options.UseSqlite(builder.Configuration["ConnectionStrings:UserConnection"]));
builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddEntityFrameworkStores<AuthenticationContext>().AddDefaultTokenProviders();
//serilog
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<PetShopContext>();
    ctx.Database.EnsureCreated();
}

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AuthenticationContext>();    
    ctx.Database.EnsureCreated();
}

if( app.Environment.IsProduction())
{
    app.UseExceptionHandler("/Error/Index");
}

app.UseAuthentication();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute("Default", "{controller=home}/{action=index}");

app.Run();
