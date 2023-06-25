using DAL;
using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BL
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        DatabaseContext context; // Veritabanı tablolarımızı tutan DatabaseContext sınıfımız
        DbSet<T> _objectSet;
        public Repository()
        {
            if (context == null)
            {
                context = new DatabaseContext();
                _objectSet = context.Set<T>();
            }
        }
        public int Add(T entity) // Normal ekleme metodu
        {
            _objectSet.Add(entity);
            return SaveChanges();
        }

        public async Task AddAsync(T entity) // asenktron ekleme metodu. Asenkron metotlarda async kelimesi kullanılır metot adından önce
        {
            await _objectSet.AddAsync(entity); // Asenkron metot içerisinde await anahtar kelimesi ile asenkron işlemi tamamlanır
        }

        public int Delete(T entity)
        {
            _objectSet.Remove(entity);
            return SaveChanges();
        }

        public T Find(int id)
        {
            return _objectSet.Find(id);
        }

        public async Task<T> FindAsync(int id) // asenkron find metodu
        {
            return await _objectSet.FindAsync(id); // await ile işlemi sonuçlandırıyoruz!
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> expression)
        {
            return await _objectSet.FirstOrDefaultAsync(expression);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _objectSet.FirstOrDefault(expression);
        }

        public List<T> GetAll() // Normal tüm kayıtları listeleme metodu
        {
            return _objectSet.ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> expression) // normal filtreli listeleme
        {
            return _objectSet.Where(expression).ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync() // asenkron listeleme
        {
            return await _objectSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression)
        {
            return await _objectSet.Where(expression).ToListAsync();
        }

        public IQueryable<T> GetAllInclude(string table) // EF de join işlemi yapan include metodu
        {
            return _objectSet.Include(table); // parametreden gönderilecek tablo adını ana sorguya ekler ve 2 tablo joinle birleştirilir EF de.
        }

        public int SaveChanges() // Normal değişikşikleri kaydetme 
        {
            return context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync() // Asenkron değişikşikleri kaydetme 
        {
            return await context.SaveChangesAsync();
        }

        public int Update(T entity)
        {
            _objectSet.Update(entity);
            return SaveChanges();
        }
    }
}
