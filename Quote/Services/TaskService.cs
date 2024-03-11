﻿using Quote.Interfaces.ServiceInterface;
using Quote.Repositorys;

namespace Quote.Services
{
    public class TaskService : ITaskInterface
    {
        private readonly TaskRepository _repo;

        public TaskService(TaskRepository repo)
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

        public async Task<List<Models.Task>> GetTasks()
        {
            var list = await _repo.GetAllAsync();
            if(list.Count == 0)
            {
                return null;
            }
            return list;
        }
    }
}
