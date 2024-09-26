namespace SingleTicketing.Services
{
    public interface IActivityLogService
    {
        Task LogActivityAsync(int userId, string action, string details, string ipAddress);
    }
}
