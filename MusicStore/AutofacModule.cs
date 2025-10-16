using System.Net.Mime;
using Application;
using Application.Domine;
using Application.Repo;
using Application.UserRegistrate.Domain.Contracts;
using Application.UserRegistrate.Repositories;
using Autofac;
using Controller.Seeds.Interfaces;
using Infrastructure.Frameworks.DataBase;
using Inrastructure.Seeds.Repositories;
using Settings;
using Basket = Infrastructure.Frameworks.Models.Basket;

namespace TradeKz;

public class AutofacModule:Module
{
   
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<Controller.Config.Settings>().As<IJwtSettings>().InstancePerLifetimeScope();      
        builder.RegisterType<SeedDataRepo>().As<ISeedDataRepo>().InstancePerLifetimeScope();      
        builder.RegisterType<JwtService>().As<IJwtService>().InstancePerLifetimeScope();      
        builder.RegisterType<SeedDataRepo>().As<ISeedDataRepo>().InstancePerLifetimeScope();      
        builder.RegisterType<UserRegistrateRepo>().As<IUserRegistrateRepo>().InstancePerLifetimeScope();      
        builder.RegisterType<Registrade>().As<IRegistrade>().InstancePerLifetimeScope();      
        builder.RegisterType<ProductRepo>().As<IProductRepo>().InstancePerLifetimeScope();      
        builder.RegisterType<Product>().As<IProduct>().InstancePerLifetimeScope();      
        builder.RegisterType<Auth>().As<IAuth>().InstancePerLifetimeScope();      
        builder.RegisterType<Basket>().As<IBasket>().InstancePerLifetimeScope();      
        builder.RegisterType<BasketRepo>().As<IBasketRepo>().InstancePerLifetimeScope();      

        
        builder.Register(c =>
        {
            
            var configuration = c.Resolve<IJwtSettings>();
            var connectionString = configuration.GetConnectionStringPostgers;
            return new Context(connectionString);
            
        }).AsSelf().InstancePerLifetimeScope();
    }
}