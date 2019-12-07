using Microsoft.AspNetCore.Mvc.Rendering;
using SportCenter.Models;
using SportCenter.ViewModels;
using System.Collections.Generic;

namespace SportCenter.Repositories.Interfaces
{
    public interface IGroupTrainingRepository : IRepository<GroupTrain>
    {
        IEnumerable<SelectListItem> GetTrainersSelect();
        IEnumerable<SelectListItem> SelectedDayOfWeek(IEnumerable<SelectListItem> selectLists, int dayOfWeek);
        IEnumerable<IEnumerable<GroupTrainingModel>> GetGroupTrainingModels(int dayOfWeek, int idTrainer, int clientId);
        void ReduceCapacity(IEnumerable<int> selectedID);
        void AddSelectedGT(IEnumerable<int> selectedID, int clientId);
        OrderGroup GetCancelOrderGroup(int clientId, int groupTrainingId);
        void DelteOrderGroup(OrderGroup orderGroup);
    }
}
