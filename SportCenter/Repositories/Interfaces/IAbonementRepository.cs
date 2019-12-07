using Microsoft.AspNetCore.Mvc.Rendering;
using SportCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.Repositories.Interfaces
{
    public interface IAbonementRepository : IRepository<Abonement>
    {
        IEnumerable<RequestAbonement> GetRequestAbonements(int clientId);
        IEnumerable<SelectListItem> SelectTerm(IEnumerable<int> terms);
        IEnumerable<SelectListItem> SelectTime(IEnumerable<TimeSpan> times);
        void AddRequestAbonement(RequestAbonement request, int clientId);
        void DeleteRequestAbonemetById(int id);
        void FreezeAbonement(Abonement currentAbonement, Abonement abonementData);
        Abonement GetByClientId(int clientId);
    }
}
