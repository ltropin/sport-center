using SportCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> Filter(Func<T, bool> predicate);
        IEnumerable<T> GetAll();
        public T FindOne(Func<T, bool> predicate);
        T GetById(int ID);
        T GetById(long ID);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
        Client GetCurrentClient(string email, bool withRole = false);
        IEnumerable<Role> GetRoles();

    }
}
