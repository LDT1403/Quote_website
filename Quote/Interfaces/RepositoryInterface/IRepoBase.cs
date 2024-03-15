namespace Quote.Interfaces.RepositoryInterface
{

    public interface IRepoBase<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<T> AddAsync(T entity);
        Task<T> AddReturnAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        Task<bool> DeleteItemAsync(int id);
        IQueryable<T> GetInclude(params string[] navigationProperties);
        System.Threading.Tasks.Task Save();
    }

}
