using Autofac;
using Autofac.Extensions.DependencyInjection;
using BlazorHost;
using BlazorHost.Data;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Serilog;
using Serilog.Events;
using StreamHelper.Core.Auth;
using StreamHelper.Integrations.Twitch;
using StreamHelper.Integrations.Twitch.Abstractions.Extensions;
using StreamHelper.Integrations.Youtube;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
             .MinimumLevel.Information()
             .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
             .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
             .Filter.ByExcluding(logEvent => logEvent.Exception is OperationCanceledException)
             .Filter.ByExcluding(logEvent => logEvent.MessageTemplate.Text.Contains("System.OperationCanceledException"))
             .Enrich.FromLogContext()
             .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3} {SourceContext:l}] {Message:lj}{NewLine}{Exception}")
             .CreateLogger();

builder.Host
       .UseServiceProviderFactory(new AutofacServiceProviderFactory())
       .UseSerilog()
       .ConfigureContainer<ContainerBuilder>(b =>
       {
           b.RegisterModule(new TwitchIntegrationAutofacModule(builder.Configuration))
            .RegisterModule(new YouTubeIntegrationAutofacModule(builder.Configuration))
            .RegisterModule(new HostAutoFacModule(builder.Configuration));
       });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddAuthentication()
       .AddTwitch(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>()
       .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync();