using System.Net.Mime;
using Application;
using Application.UserRegistrate.Domain.Contracts;
using Application.UserRegistrate.Repositories;
using Autofac;
using Controller.Seeds.Interfaces;
using Infrastructure.Frameworks.DataBase;
using Inrastructure.Seeds.Repositories;
using Settings;

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
        builder.RegisterType<Auth>().As<IAuth>().InstancePerLifetimeScope();      

        
        builder.Register(c =>
        {
            
            var configuration = c.Resolve<IJwtSettings>();
            var connectionString = configuration.GetConnectionStringPostgers;
            return new Context(connectionString);
            
        }).AsSelf().InstancePerLifetimeScope();
    }
}