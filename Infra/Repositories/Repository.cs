using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Infra.Data.Context;
using Infra.Repositories.Interfaces;

namespace Infra.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T> GetById(int id) => await _dbSet.FindAsync(id);
    public async Task<IEnumerable<T>> GetAll() => await _dbSet.ToListAsync();
    public async Task Add(T entity) => await _dbSet.AddAsync(entity);
    public void Update(T entity) => _dbSet.Update(entity);
    public void Delete(T entity) => _dbSet.Remove(entity);
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }
}