using Autofac;
using Autofac.Extensions.DependencyInjection;
using Controller.Config;
using Controller.Seeds.Interfaces;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Добавим сервисы, которые будут зарегистрированы через Autofac
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new TradeKz.AutofacModule());
});

var bearer = new BearerServiceAdd(builder.Configuration);
bearer.SetBearer(ref builder);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Settings"));

var app = builder.Build();

using (var scope = app.Services.GetAutofacRoot().BeginLifetimeScope())
{
    var initializer = scope.Resolve<ISeedDataRepo>();
    await initializer.Execute();
}

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<Middleware>();

app.MapControllers();

app.Run();