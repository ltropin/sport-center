using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SportCenter.Data;
using SportCenter.Models;
using SportCenter.Repositories.Interfaces;
using SportCenter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SportCenter.Extensions.Extensions;

namespace SportCenter.Repositories
{
    public class GroupTrainingRepository : Repository<GroupTrain>, IGroupTrainingRepository
    {
        public GroupTrainingRepository(SportCenterContext context) : base(context) { }
        public void AddSelectedGT(IEnumerable<int> selectedID, int clientId)
        {
            selectedID.ToList().ForEach(idGt =>
            {
                context.OrderGroup.Add(new OrderGroup
                {
                    IdClient = clientId,
                    IdGroupTrain = idGt,
                    Date = DateTime.Now
                });
            });
        }

        public void DelteOrderGroup(OrderGroup orderGroup)
        {
            context.OrderGroup.Remove(orderGroup);
        }

        public OrderGroup GetCancelOrderGroup(int clientId, int groupTrainingId)
        {
            return context.OrderGroup.Single(x => x.IdClient == clientId && x.IdGroupTrain == groupTrainingId);
        }

        public IEnumerable<IEnumerable<GroupTrainingModel>> GetGroupTrainingModels(
            int dayOfWeek,
            int idTrainer,
            int clientId)
        {
            return context.GroupTrain.Include(x => x.IdTrainerNavigation)
                                     .Include(x => x.OrderGroup)
                                     .Where(gt => gt.DayOfWeek == dayOfWeek &&
                                                 (gt.IdTrainer == idTrainer || idTrainer == 0) &&
                                                 gt.Capacity > 0)
                                     .ToList()
                                     .Select(x => new GroupTrainingModel
                                     {
                                         ID = x.Id,
                                         Name = x.Name,
                                         Capacity = x.Capacity,
                                         DayOfWeek = DayOfWeekMap[x.DayOfWeek].Short,
                                         Time = x.Time.ToString(),
                                         TrainerName = x.IdTrainerNavigation.Fio,
                                         Recorded = x.OrderGroup.FirstOrDefault(x => x.IdClient == clientId) != null
                                     })
                                     .TakeMany(3);
        }

        public IEnumerable<SelectListItem> GetTrainersSelect()
        {
            var trainers = context.Trainer.Select(x => new SelectListItem
                                           {
                                               Text = x.Fio,
                                               Value = x.Id.ToString()
                                           })
                                           .AsNoTracking()
                                           .ToList();
            trainers.Insert(0, new SelectListItem { Text = "Все", Selected = true, Value = "0" });
            return trainers;
        }

        public void ReduceCapacity(IEnumerable<int> selectedID)
        {
            selectedID.Select(idGt => context.GroupTrain.Single(x => x.Id == idGt))
                      .ToList()
                      .ForEach(gt => gt.Capacity--);
        }

        public IEnumerable<SelectListItem> SelectedDayOfWeek(IEnumerable<SelectListItem> selectLists, int dayOfWeek)
        {
            return selectLists.Select(day => new SelectListItem(text: day.Text,
                                                                value: day.Value,
                                                                selected: int.Parse(day.Value) == dayOfWeek));
        }

    }
}
