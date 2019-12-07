using Microsoft.EntityFrameworkCore;
using SportCenter.Data;
using SportCenter.Models;
using SportCenter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly SportCenterContext context;
        public Repository(SportCenterContext context)
        {
            this.context = context;
        }

        public void Create(T entity)
        {
            context.Add(entity);
            Save();
        }

        public void Delete(T entity)
        {
            context.Remove(entity);
            Save();
        }

        public T FindOne(Func<T, bool> predicate) =>
            context.Set<T>().FirstOrDefault(predicate);

        public IEnumerable<T> Filter(Func<T, bool> predicate) =>
            context.Set<T>().Where(predicate);

        public IEnumerable<T> GetAll() => context.Set<T>();

        public T GetById(int ID) => context.Set<T>().Find(ID);
        public T GetById(long ID) => context.Set<T>().Find(ID);

        public void Save() => context.SaveChanges();

        public void Update(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Modified;
            Save();
        }

        public Client GetCurrentClient(string email, bool withRole = false)
        {
            if (withRole)
                return context.Client.Include(x => x.IdRoleNavigation).Single(x => x.Email == email);

            return context.Client.Single(x => x.Email == email);
        }
        public IEnumerable<Role> GetRoles() => context.Role;

    }
}
