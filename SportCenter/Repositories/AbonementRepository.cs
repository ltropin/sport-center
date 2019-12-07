using Microsoft.AspNetCore.Mvc.Rendering;
using SportCenter.Data;
using SportCenter.Models;
using SportCenter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.Repositories
{
    public class AbonementRepository : Repository<Abonement>, IAbonementRepository
    {
        public AbonementRepository(SportCenterContext context) : base(context) { }
        public void AddRequestAbonement(RequestAbonement request, int clientId)
        {
            request.IdClient = clientId;
            context.RequestAbonement.Add(request);
        }

        public void DeleteRequestAbonemetById(int id)
        {
            var requestAbonement = context.RequestAbonement.Single(x => x.Id == id);
            context.RequestAbonement.Remove(requestAbonement);
        }

        public void FreezeAbonement(Abonement currentAbonement, Abonement abonementData)
        {
            currentAbonement.Capacity -= abonementData.IntervalBlock.Value;
            currentAbonement.DateBlock = abonementData.DateBlock.Value;
            currentAbonement.IntervalBlock = abonementData.IntervalBlock.Value;
            currentAbonement.IsActive = false;
        }

        public Abonement GetByClientId(int clientId)
        {
            return context.Abonement.SingleOrDefault(x => x.IdClient == clientId);
        }

        public IEnumerable<RequestAbonement> GetRequestAbonements(int clientId)
        {
            return context.RequestAbonement.Where(x => x.IdClient == clientId);
        }

        public IEnumerable<SelectListItem> SelectTerm(IEnumerable<int> terms)
        {
            return terms.Select(x => new SelectListItem { Text = x.ToString(), Value = x.ToString() });
        }

        public IEnumerable<SelectListItem> SelectTime(IEnumerable<TimeSpan> times)
        {
            return times.Select(x => new SelectListItem { Text = x.ToString(@"hh\:mm"), Value = x.ToString() });
        }
    }
}
