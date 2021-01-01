using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using System.Linq;
using System.Reflection;

namespace RocketLaunchJournal.Infrastructure.Services
{
    public abstract class BaseService
    {
        protected DataContext db { get; set; } = default!;
        protected ILoggingContext LoggingDb { get; set; } = default!;
        protected UserPermissionService UserPermissionService { get; set; } = default!;

        /// <summary>
        /// Sets up the data context and UserPermissionService for the service and all services injected in the service
        /// </summary>
        /// <param name="dataContext">data context</param>
        /// <param name="userPermissionService">UserPermissionService</param>
        public void SetupService(DataContext dataContext, ILoggingContext loggingDb, UserPermissionService userPermissionService)
        {
            db = dataContext;
            LoggingDb = loggingDb;
            UserPermissionService = userPermissionService;
            
            // go through all fields, if any of them are a BaseService Setup DB and UPS
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic |
                         BindingFlags.Static | BindingFlags.Instance |
                         BindingFlags.DeclaredOnly;
            var fields = this.GetType().UnderlyingSystemType.GetFields(flags).Where(w => w.FieldType.BaseType == typeof(BaseService)).ToList();
            foreach (var item in fields)
            {
                var baseService = (item.GetValue(this) as BaseService);
                baseService?.SetupService(db, LoggingDb, UserPermissionService);
            }
        }
    }
}
