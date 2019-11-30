using SportCenter.Data;
using SportCenter.Extensions;
using SportCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Linq.Enumerable;
using static SportCenter.Extensions.Extensions;

namespace SportCenter.AppStart
{
    public static class StartData
    {
        static Random rnd = new Random((int)DateTime.Now.Ticks);
        /// <summary>
        /// Вызывает методы для добавления временных данных в БД
        /// </summary>
        /// <param name="context">Контекст бд</param>
        public static void SetupData(ref SportCenterContext context)
        {
            // Независмые данные
            AddRoles(ref context);
            AddTrainers(ref context);
            AddClients(ref context);
            // Зависмые данные
            AddGroupTrains(ref context);
            AddOrderGroup(ref context);
            AddPersonalTrains(ref context);
        }

        private static void AddRoles(ref SportCenterContext context)
        {
            var roles = new List<Role>()
            {
                new Role { Name = "Клиент" },
                new Role { Name = "Менеджер" }
            };

            context.Role.AddRange(roles);

            context.SaveChanges();
        }

        private static void AddOrderGroup(ref SportCenterContext context)
        {
            var groupTrainings = context.GroupTrain.ToList();
            var clients = context.Client.ToList();

            var maxCount = Math.Max(clients.Count, groupTrainings.Count);

            var offset = rnd.Next(3, 5);

            foreach (var i in Range(maxCount, offset))
            {
                context.OrderGroup.Add(new OrderGroup
                {
                    IdClient = clients.Choice().Id,
                    IdGroupTrain = groupTrainings.Choice().Id,
                    Date = DateTime.Now
                });
            }

            context.SaveChanges();
        }

        private static void AddPersonalTrains(ref SportCenterContext context)
        {
            var trainers = context.Trainer.ToList();
            var clients = context.Client.ToList();
            var timesRange = new TimeSpan(8, 0, 0).RangeTimeSpan(new TimeSpan(1, 30, 0), 9);

            var maxCount = Math.Max(clients.Count, trainers.Count);

            var offset = rnd.Next(3, 5);

            foreach (var i in Range(maxCount, offset))
            {
                context.PersonalTrain.Add(new PersonalTrain
                {
                    IdTrainer = trainers.Choice().Id,
                    IdClient = clients.Choice().Id,
                    Time = timesRange.Choice(),
                    DayOfWeek = rnd.Next(0, 7),
                });
            }

            context.SaveChanges();
        }

        private static void AddClients(ref SportCenterContext context)
        {
            var allRoles = context.Role.ToList();
            var mangerRole = FromRoleEnum(Roles.Manager, allRoles);
            var clientRole = FromRoleEnum(Roles.Client, allRoles);

            var clients = new List<Client>()
            {
                new Client { Fio = "Тропин А. А.", Email = "admin@admin.ru", Password = Client.HashPass("123"), IdRole = clientRole.Id },
                new Client { Fio = "Манов К. А.", Email = "kirill@bk.ru", Password = Client.HashPass("12345"), IdRole = mangerRole.Id }
            };
            context.Client.AddRange(clients);
            context.SaveChanges();
        }


        private static void AddTrainers(ref SportCenterContext context)
        {
            var trainers = new List<Trainer>()
            {
                new Trainer { Fio = "Богданов Александр" },
                new Trainer { Fio = "Дуденькова Наталья" },
                new Trainer { Fio = "Мазгутов Эдуард" },
                new Trainer { Fio = "Осьмин Юрий" },
                new Trainer { Fio = "Хабилов Идель" },
                new Trainer { Fio = "Крапоткина Анастасия" },
            };

            context.Trainer.AddRange(trainers);
            context.SaveChanges();
        }

        private static void AddGroupTrains(ref SportCenterContext context)
        {
            List<string> NamesGroupTrain = new List<string>()
            {
                "Hot Iron (Хот-Айрон)",
                "Cycle (Сайкл)",
                "Tabat (Табата)",
                "Stretching (Стретчинг)",
                "Единоборства: Boxing, MMA",
                "Yoga (Йога)"
            };

            var trainers = context.Trainer.ToList();
            var timesRange = new TimeSpan(8, 0, 0).RangeTimeSpan(new TimeSpan(1, 30, 0), 9);

            var offset = rnd.Next(3, 8);

            foreach (var i in Range(NamesGroupTrain.Count, offset))
            {
                context.GroupTrain.Add(new GroupTrain
                {
                    Capacity = rnd.Next(5, 50),
                    DayOfWeek = rnd.Next(0, 7),
                    IdTrainer = trainers.Choice().Id,
                    Name = NamesGroupTrain.Choice(),
                    Time = timesRange.Choice(),
                });
            }
            context.SaveChanges();
            

        }
    }
}
