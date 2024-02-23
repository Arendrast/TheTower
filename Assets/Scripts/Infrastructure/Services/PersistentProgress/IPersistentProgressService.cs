namespace Infrastructure.Services.PersistentProgress
{
    public interface IPersistentProgressService : IService
    {
        Progress Progress { get; set; }
    }
}