using DP.CCM.Data.Mapping;
using DP.CCM.Entity.Models;
using DP.CCM.Modules.Administration.Interface.Repository;
using DP.CCM.Modules.Administration.Interface.Service;
using DP.CCM.Modules.Administration.Repository;
using DP.CCM.Modules.Administration.Service;
using DP.CCM.Modules.Configuration.Interface.Repository;
using DP.CCM.Modules.Configuration.Interface.Service;
using DP.CCM.Modules.Configuration.Repository;
using DP.CCM.Modules.Configuration.Service;
using DP.CCM.Modules.DigMail.Interface.Repository;
using DP.CCM.Modules.DigMail.Interface.Service;
using DP.CCM.Modules.DigMail.Repository;
using DP.CCM.Modules.DigMail.Service;
using DP.CCM.Modules.DigSMS.Interface.Repository;
using DP.CCM.Modules.DigSMS.Interface.Service;
using DP.CCM.Modules.DigSMS.Repository;
using DP.CCM.Modules.DigSMS.Service;
using DP.CCM.Modules.ECM.Interface.Repository;
using DP.CCM.Modules.ECM.Interface.Service;
using DP.CCM.Modules.ECM.Repository;
using DP.CCM.Modules.ECM.Service;
using DP.CCM.Modules.EntryControl.Interface.Repository;
using DP.CCM.Modules.EntryControl.Interface.Service;
using DP.CCM.Modules.EntryControl.Repository;
using DP.CCM.Modules.EntryControl.Service;
using DP.CCM.Modules.ProductionControl.Interface.Repository;
using DP.CCM.Modules.ProductionControl.Interface.Service;
using DP.CCM.Modules.ProductionControl.Repository;
using DP.CCM.Modules.ProductionControl.Service;
using DP.CCM.Modules.Security.Interface.Service;
using DP.CCM.Modules.Security.Service;
using DP.Framework.Integration.Repository;
using Microsoft.Practices.Unity;
using System.Web.Mvc;

namespace DP.ProductionOrderWebApi
{
    public class UnityBootstrapper
    {
        private static IUnityContainer _container;

        public IUnityContainer GetInstance()
        {
            if (_container == null)
            {
                _container = new UnityContainer();
                RegisterAllDependency();
            }

            return _container;
        }

        public T Resolve<T>() where T : class
        {
            return GetInstance().Resolve<T>();
        }

        private void RegisterAllDependency()
        {
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            // e.g. container.RegisterType<ITestService, TestService>();            

            _container.RegisterType<IUnitOfWork, UnitOfWork>(new PerResolveLifetimeManager());
            _container.RegisterType<IDataContext, CCM2Context>(new PerResolveLifetimeManager());

            //Serviços
            _container.RegisterType<IUserService, UserService>(new PerResolveLifetimeManager());
            _container.RegisterType<IDigMailService, DigMailService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEntryControlFileService, EntryControlFileService>(new PerResolveLifetimeManager());
            _container.RegisterType<IProductionControlService, ProductionControlService>(new PerResolveLifetimeManager());
            _container.RegisterType<IDigSmsService, DigSmsService>(new PerResolveLifetimeManager());
            _container.RegisterType<ISecurityService, SecurityService>(new PerResolveLifetimeManager());
            _container.RegisterType<IConfigurationService, ConfigurationService>(new PerResolveLifetimeManager());
            _container.RegisterType<IAdminService, AdminService>(new PerResolveLifetimeManager());
            _container.RegisterType<IEcmService, EcmService>(new PerResolveLifetimeManager());

            //Repositórios
            _container.RegisterType<IClientConfigPurgeInvoiceRepository, ClientConfigPurgeInvoiceRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ICampaignDetailReportRepository, CampaignDetailReportRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IArtifactRepository, ArtifactRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IClientServiceRepository, ClientServiceRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IServiceRepository, ServiceRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IAttachmentRepository, AttachmentRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IAttachmentPathRepository, AttachmentPathRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IAttachmentRelationshipRepository, AttachmentRelationshipRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ICampaignRepository, CampaignRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IExpectedFileRepository, ExpectedFileRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IExtensionRepository, ExtensionRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IFileConfigurationRepository, FileConfigurationRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IFilePropertyFieldRepository, FilePropertyFieldRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IFilePropertyRepository, FilePropertyRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IFileRegistryRepository, FileRegistryRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IFileRepository, FileRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IFileConfigurationPathRepository, FileConfigurationPathRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IUserRepository, UserRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IProductionOrderRepository, ProductionOrderRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IProductionDocumentRepository, ProductionDocumentRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IChannelTypeRepository, ChannelTypeRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IOrderStatusRepository, OrderStatusRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ISLARepository, SLARepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IPrintBatchRepository, PrintBatchRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IProductionOrderFeedbackRepository, ProductionOrderFeedBackRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IServiceCommunicationRepository, ServiceCommunicationRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IProductionOrderStatusRepository, ProductionOrderStatusRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IPrintBatchFeedBackRepository, PrintBatchFeedBackRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IPrintBatchStatusRepository, PrintBatchStatusRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IControlRegistryRepository, ControlRegistryRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ITagRepository, TagRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ITagTypeRepository, TagTypeRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ICampaignSmsRepository, CampaignSmsRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<DP.CCM.Modules.DigSMS.Interface.Repository.IContactRepository, DP.CCM.Modules.DigSMS.Repository.ContactRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<DP.CCM.Modules.DigMail.Interface.Repository.IContactRepository, DP.CCM.Modules.DigMail.Repository.ContactRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<DP.CCM.Modules.DigMail.Interface.Repository.IBaseRepository, DP.CCM.Modules.DigMail.Repository.BaseRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ISmsRepository, SmsRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ISmsStatusRepository, SmsStatusRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IControlStatusRepository, ControlStatusRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IArtifactRepository, ArtifactRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ICostRepository, CostRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ICostWithTagRepository, CostWithTagRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ICommunicationTypeRepository, CommunicationTypeRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IActionRepository, ActionRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IGroupRepository, GroupRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IProfileRepository, ProfileRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IUserRepository, UserRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IContactDispatchRepository, ContactDispatchRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IClientRepository, ClientRepository>(new PerResolveLifetimeManager());

            _container.RegisterType<ITagOrderRepository, TagOrderRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IEmailRepository, EmailRepository>(new PerResolveLifetimeManager());

            _container.RegisterType<ICostCenterRepository, CostCenterRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ICostCenterDetailsRepository, CostCenterDetailsRepository>(new PerResolveLifetimeManager());

            //ECM
            _container.RegisterType<ICommunicationRepository, CommunicationRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IDocumentHistoryViewRepository, DocumentHistoryViewRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IDocumentIndexerRepository, DocumentIndexerRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IDocumentRepository, DocumentRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IDocumentTypeRepository, DocumentTypeRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IIndexerRepository, IndexerRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ILogRepository, LogRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IReachRepository, ReachRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IScopeRepository, ScopeRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ISendTypeRepository, SendTypeRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IUploadIndexerRepository, UploadIndexerRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IUploadRepository, UploadRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ISendTypeDocumentTypeRepository, SendTypeDocumentTypeRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ISendMissiveRepository, SendMissiveRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<ISendSmsRepository, SendSmsRepository>(new PerResolveLifetimeManager());
            _container.RegisterType<IEmailPropertiesRepository, EmailPropertiesRepository>(new PerResolveLifetimeManager());
        }

    }
}
