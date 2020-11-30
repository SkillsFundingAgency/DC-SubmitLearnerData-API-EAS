using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Features.AttributeFilters;
using ESFA.DC.Api.Common.Settings;
using ESFA.DC.PublicApi.EAS.Services;
using ESFA.DC.PublicApi.EAS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ESFA.DC.PublicApi.EAS.Ioc
{
    public class ServiceRegistrations : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Eas1819Repository>().Keyed<IEASRepository>(1819).WithAttributeFiltering().InstancePerLifetimeScope();
            builder.RegisterType<Eas1920Repository>().Keyed<IEASRepository>(1920).WithAttributeFiltering().InstancePerLifetimeScope();
            builder.RegisterType<Eas2021Repository>().Keyed<IEASRepository>(2021).WithAttributeFiltering().InstancePerLifetimeScope();

            // Db contexts
            builder.RegisterType<EAS1819.EF.EasContext>().As<EAS1819.EF.Interface.IEasdbContext>().ExternallyOwned();
            builder.RegisterType<EAS1920.EF.EasContext>().As<EAS1920.EF.Interface.IEasdbContext>().ExternallyOwned();
            builder.RegisterType<EAS2021.EF.EasContext>().As<EAS2021.EF.Interface.IEasdbContext>().ExternallyOwned();

            builder.Register(context =>
                {
                    var connectionStrings = context.Resolve<ConnectionStrings>();
                    var optionsBuilder = new DbContextOptionsBuilder<EAS1819.EF.EasContext>();
                    optionsBuilder.UseSqlServer(
                        connectionStrings.KeyValues["EAS1819"],
                        options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                    return optionsBuilder.Options;
                })
                .As<DbContextOptions<EAS1819.EF.EasContext>>()
                .SingleInstance();

            builder.Register(context =>
                {
                    var connectionStrings = context.Resolve<ConnectionStrings>();
                    var optionsBuilder = new DbContextOptionsBuilder<EAS1920.EF.EasContext>();
                    optionsBuilder.UseSqlServer(
                        connectionStrings.KeyValues["EAS1920"],
                        options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                    return optionsBuilder.Options;
                })
                .As<DbContextOptions<EAS1920.EF.EasContext>>()
                .SingleInstance();


            builder.Register(context =>
                {
                    var connectionStrings = context.Resolve<ConnectionStrings>();
                    var optionsBuilder = new DbContextOptionsBuilder<EAS2021.EF.EasContext>();
                    optionsBuilder.UseSqlServer(
                        connectionStrings.KeyValues["EAS2021"],
                        options => options.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), new List<int>()));

                    return optionsBuilder.Options;
                })
                .As<DbContextOptions<EAS2021.EF.EasContext>>()
                .SingleInstance();

        }
    }
}