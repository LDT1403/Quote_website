using Quote.Interfaces.RepositoryInterface;
using Quote.Interfaces.ServiceInterface;
using Quote.Repositorys;

namespace Quote.Services
{
    public class TaskService : ITaskInterface
    {
        private readonly IRepoBase<Models.Task> _repo;

        public TaskService(IRepoBase<Models.Task> repo)
        {
            _repo = repo;
        }

        public async Task<Models.Task> CreateTasks(Models.Task task)
        {
            var item = await _repo.AddReturnAsync(task);
            if(item != null)
            {
                return item;
            }
            return null;
           
        }

        public async Task<Models.Task> GetTaskById(int taskId)
        {
            var task = await _repo.GetByIdAsync(taskId);
            return task;
        }

        public async Task<List<Models.Task>> GetTasks()
        {
            var list = await _repo.GetAllAsync();
            if(list.Count == 0)
            {
                return null;
            }
            return list;
        }

        public async Task<Models.Task> UpdateTasks(Models.Task task)
        {
            var taskUp = await _repo.UpdateAsync(task);
            return taskUp;
        }
    }
}
