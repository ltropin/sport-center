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
    public class ManagerRepository : Repository<RequestAbonement>, IManagerRepository
    {
        public ManagerRepository(SportCenterContext context) : base(context) { }

        public void AddAbonement(RequestAbonement request)
        {
            context.Abonement.Add(new Abonement
            {
                IdClient = request.IdClient,
                Capacity = 30,
                Term = request.Term,
                Time = request.Time,
                IsActive = true
            });
            context.RequestAbonement.Remove(request);
        }



        public IEnumerable<RequestAbonement> GetRequestAbonements(int clientRoleId)
        {
            return context.RequestAbonement.Include(x => x.IdClientNavigation)
                                           .Where(x => x.IdClientNavigation.IdRole == clientRoleId);
        }

    }
}
