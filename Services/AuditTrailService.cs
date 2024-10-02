using SingleTicketing.Data;
using SingleTicketing.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SingleTicketing.Services
{
    public interface IAuditTrailService
    {
        Task LogChangeAsync(int entityId, string fieldName, string? oldValue, string? newValue, string action, string? username);
    }

    public class AuditTrailService : IAuditTrailService
    {
        private readonly MyDbContext _context;

        public AuditTrailService(MyDbContext context)
        {
            _context = context;
        }

        public async Task LogChangeAsync(int entityId, string fieldName, string? oldValue, string? newValue, string action, string? username)
        {
            var auditTrail = new AuditTrail
            {
                EntityId = entityId,
                FieldName = fieldName,
                OldValue = oldValue,
                NewValue = newValue,
                Action = action,
                Username = username,
                Timestamp = DateTime.UtcNow
            };

            _context.AuditTrails.Add(auditTrail);
            await _context.SaveChangesAsync();
        }
    }

}
