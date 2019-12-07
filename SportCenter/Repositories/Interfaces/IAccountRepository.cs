using SportCenter.Models;
using SportCenter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Client>
    {
        Client GetClient(string email, string password);
        IEnumerable<GroupTrainingModel> GetGroupTrainings(int clientId);
        IEnumerable<PersonalTrainingModel> GetPersonalTrainings(int clientId);
        void DeleteOrderGroupById(int groupTrainingId, int clientId);
        void DeletePersonalTrainById(int personalTrainingId);
    }
}
