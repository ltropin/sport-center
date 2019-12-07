using SportCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.Repositories.Interfaces
{
    public interface IManagerRepository : IRepository<RequestAbonement>
    {
        IEnumerable<RequestAbonement> GetRequestAbonements(int clientRoleId);
        void AddAbonement(RequestAbonement request);
    }
}
