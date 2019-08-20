using Funq;

using WebApplication.ServiceModel;
using WebApplication.ServiceInterface;

using ServiceStack.OrmLite;
using ServiceStack;
using ServiceStack.Data;

using System.Configuration;

namespace WebApplication
{
    //VS.NET Template Info: https://servicestack.net/vs-templates/EmptyAspNet
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("WebApplication", typeof(MyServices).Assembly) { }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            var dbFactory = new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings["DbConn"].ConnectionString,
                                                        ServiceStack.OrmLite.SqlServer.SqlServerOrmLiteDialectProvider.Instance);

            container.Register<IDbConnectionFactory>(dbFactory);
            container.Register(c => new DbRepository()).ReusedWithin(ReuseScope.Request);
            container.RegisterAutoWired<DbRepository>();
        }
    }
}