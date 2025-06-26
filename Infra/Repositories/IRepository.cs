namespace Infra.Repositories;

public interface IRepository<T> where T : class
{
    Task<T> GetById(int id);
    Task<IEnumerable<T>> GetAll();
    Task Add(T entity);
    void Update(T entity);
    void Delete(T entity);
}