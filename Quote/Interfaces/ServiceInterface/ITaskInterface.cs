

namespace Quote.Interfaces.ServiceInterface
{
    public interface ITaskInterface
    {
        Task<List<Models.Task>> GetTasks();
    }
}
