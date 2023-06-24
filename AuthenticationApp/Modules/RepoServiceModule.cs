using AuthenticationApp.Core.Repositories;
using AuthenticationApp.Core.UnitOfWorks;
using AuthenticationApp.Repository.Repositories;
using AuthenticationApp.Repository.UnitOfWorks;
using Autofac;
using Module = Autofac.Module;
using AuthenticationApp.Service.Services;
using AuthenticationApp.Core.Services;

namespace AuthenticationApp.API.Modules
{
    public class RepoServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
        }
    }
}
