using Microsoft.AspNetCore.Authentication.Cookies;
using SportCenter.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using SportCenter.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static SportCenter.Extensions.Extensions;
using SportCenter.Data;
using SportCenter.ViewModels;

namespace SportCenter.Repositories
{
    public class AccountRepository : Repository<Client>, IAccountRepository
    {
        public AccountRepository(SportCenterContext context) : base(context) { }

        public void DeleteOrderGroupById(int groupTrainingId, int clientId)
        {
            var deleteItem = context.OrderGroup.Single(x => x.IdGroupTrain == groupTrainingId && x.IdClient == clientId);
            context.OrderGroup.Remove(deleteItem);
        }

        public void DeletePersonalTrainById(int personalTrainingId)
        {
            var deleteItem = context.PersonalTrain.Single(x => x.Id == personalTrainingId);
            context.PersonalTrain.Remove(deleteItem);
        }

        public Client GetClient(string email, string hashPass)
        {
            return context.Client.SingleOrDefault(u => u.Email == email && u.Password == hashPass);
        }

        public IEnumerable<GroupTrainingModel> GetGroupTrainings(int clientId)
        {
            return context.OrderGroup.Include(x => x.IdGroupTrainNavigation)
                                     .ThenInclude(x => x.IdTrainerNavigation)
                                     .Where(x => x.IdClient == clientId)
                                     .Select(group => group.IdGroupTrainNavigation)
                                     .ToList()
                                     .Select(x => new GroupTrainingModel
                                     {
                                         ID = x.Id,
                                         Name = x.Name,
                                         TrainerName = x.IdTrainerNavigation.Fio,
                                         Capacity = x.Capacity,
                                         DayOfWeek = DayOfWeekMap[x.DayOfWeek].Long,
                                         Recorded = true,
                                         Time = x.Time.ToString(@"hh\:mm")
                                     }).AsEnumerable();
        }

        public IEnumerable<PersonalTrainingModel> GetPersonalTrainings(int clientId)
        {
            return context.PersonalTrain.Where(x => x.IdClient == clientId)
                                        .Include(x => x.IdTrainerNavigation)
                                        .ToList()
                                        .Select(x => new PersonalTrainingModel
                                        {
                                            ID = x.Id,
                                            TrainerName = x.IdTrainerNavigation.Fio,
                                            DayOfWeek = DayOfWeekMap[x.DayOfWeek].Long,
                                            Time = x.Time.ToString(@"hh\:mm")
                                        })
                                        .AsEnumerable();
        }
    }
}
