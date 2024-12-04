namespace SOA_CA2.Events
{
    public interface INotificationEvent
    {
        Task HandleAsync();
    }
}
