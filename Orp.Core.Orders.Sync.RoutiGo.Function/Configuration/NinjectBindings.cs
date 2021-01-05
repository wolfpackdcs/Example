using Encrypted.Configuration.Provider.JsonFiles;
using log4net;
using Ninject;
using Ninject.Modules;
using Orp.Core.Clients.Base.Security;
using Orp.Core.FTP;
using Orp.Core.FTP.Adapters.Binders;
using Orp.Core.FTP.Interfaces;
using Orp.Core.Logging.Adapters.Log4Net;
using Orp.Core.Logging.DI.Ninject;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Clients;
using Orp.Core.Orders.Sync.RoutiGo.Function.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Wolfpack.MultiTenant.Core;
using Wolfpack.MultiTenant.TokenProvider.Models;

namespace Orp.Core.Orders.Sync.RoutiGo.Function.Configuration
{
    internal class NinjectBindings : NinjectModule
    {
        private readonly ConfigurationHelper _configuration;

        public NinjectBindings(ConfigurationHelper configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public override void Load()
        {
            BindNinjectContainerLogging();

            Bind<IProvideEncryptedConfiguration<TenantConfigurationHolder>>().To<EncryptedConfigurationProvider<TenantConfigurationHolder>>()
                .WithConstructorArgument("configurationFilePath", Path.Combine(_configuration.AppBaseDirectory, $"TenantConfiguration.{_configuration.CurrentEnvironment}.json"))
                .WithConstructorArgument("fingerPrint", _configuration.ConfigCertThumbprint);

            Bind<TenantConfigurationHolder>().ToMethod(x => x.Kernel.Get<IProvideEncryptedConfiguration<TenantConfigurationHolder>>().GetEncryptedConfiguration());

            Bind<IFTPClient>().To<FTPClient>();
            Bind<IFTPClientAdapter>().To<BinderFTPAdapter>()
                                     .WithConstructorArgument("storageAccountAppSettingKey", "FTP_BLOB_STORAGE");

            foreach (TenantConfiguration tenantConfiguration in GetTenantConfigurationList())
            {
                foreach (ScopeConfiguration scopeConfiguration in tenantConfiguration.ScopeConfigurations ?? Enumerable.Empty<ScopeConfiguration>())
                {
                    var tenantContext = new TenantContext(tenantConfiguration.TenantId, scopeConfiguration.ScopeId);

                    Bind<TenantContext>().ToConstant(tenantContext)
                                         .MultiTenant(tenantContext.TenantScope)
                                         .InSingletonScope();

                    Bind<DeliverySyncHandler>().ToSelf()
                                              .MultiTenant(tenantContext.TenantScope)
                                               .WithConstructorArgument("exportDeliveryUploadDirectory", scopeConfiguration.ExportDeliveryUploadDirectory);

                    TenantOAuth2Configuration oauthConfiguration = scopeConfiguration.TenantOAuth2Configuration;

                    Bind<IOrderReadClient>().To<ORPOrderReadClient>()
                                                      .MultiTenant(tenantContext.TenantScope)
                                                      .InSingletonScope()
                                                      .WithConstructorArgument("scopeId", tenantContext.ScopeId)
                                                      .WithConstructorArgument("baseAddress", _configuration.OrderReadAPIUri)
                                                      .WithConstructorArgument("configuration", new OAuth2Configuration(_configuration.IdentityServerUri,
                                                                                                                        oauthConfiguration.OAuth2Client,
                                                                                                                        oauthConfiguration.OAuth2Secret,
                                                                                                                        oauthConfiguration.OAuth2Scope.Split(' ')));

                    Kernel.UseTenantLogger(tenantContext.TenantId, tenantContext.ScopeId);
                }
            }
        }

        /// <summary>
        /// Injects a logger into the Ninject DI container. 
        /// The services inside the Ninject DI container will use this logger since the logger 
        /// defined in the .NET Core container will be inaccessible in this context. 
        /// </summary>
        private void BindNinjectContainerLogging()
        {
            string path = Path.Combine(_configuration.AppBaseDirectory, $"log4net.{_configuration.CurrentEnvironment}.config");
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());

            log4net.Config.XmlConfigurator.Configure(logRepository, new FileInfo(path));
            Kernel.UseLogging<Log4NetFactory>();
        }

        private IEnumerable<TenantConfiguration> GetTenantConfigurationList()
        {
            var list = Kernel.Get<TenantConfigurationHolder>()?.TenantConfigurations;

            if (!list?.Any() ?? true)
            {
                throw new Exception("Unable to find any tenant configurations in the configuration file.");
            }

            return list;
        }
    }
}
