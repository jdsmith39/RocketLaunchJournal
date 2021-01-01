using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Threading;
using System.Threading.Tasks;
using RocketLaunchJournal.Model;

namespace RocketLaunchJournal.Entities
{
    public interface ILoggingContext
    {
        DbSet<APILog> APILogs { get; set; }
        DbSet<SystemLog> SystemLogs { get; set; }

        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity: class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
