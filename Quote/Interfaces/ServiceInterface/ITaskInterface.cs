

namespace Quote.Interfaces.ServiceInterface
{
    public interface ITaskInterface
    {
        Task<List<Models.Task>> GetTasks();

        Task<Models.Task> CreateTasks(Models.Task task);
    }
}
