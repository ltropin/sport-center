using Microsoft.AspNetCore.Mvc.Rendering;
using SportCenter.Data;
using SportCenter.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportCenter.Extensions
{
    public static class Extensions
    {
        readonly static Random rnd = new Random((int)DateTime.Now.Ticks);

        public static readonly Dictionary<int, (string Short, string Long)> DayOfWeekMap = new Dictionary<int, (string Short, string Long)>
        {
            [0] = (Short: "Пн", Long: "Понедельник"),
            [1] = (Short: "Вт", Long: "Вторник"),
            [2] = (Short: "Ср", Long: "Среда"),
            [3] = (Short: "Чт", Long: "Четверг"),
            [4] = (Short: "Пт", Long: "Пятница"),
            [5] = (Short: "Сб", Long: "Суббота"),
            [6] = (Short: "Вс", Long: "Восскресенье"),
        };

        public static List<int> TermList = Enumerable.Range(1, 4).Select(x => x * 3).ToList();

        public static List<TimeSpan> TimeList = new TimeSpan(8, 0, 0).RangeTimeSpan(new TimeSpan(1, 30, 0), 9).ToList();

        public enum Roles
        {
            Client,
            Manager
        }

        public static Role FromRoleEnum(Roles roles, List<Role> allRoles)
        {
            return roles switch
            {
                Roles.Client => allRoles.Find(x => x.Name == "Клиент"),
                Roles.Manager => allRoles.Find(x => x.Name == "Менеджер"),
                _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(roles)),
            };
        }

        public static Roles FromRoleElement(Role role)
        {
            return role switch
            {
                { Name: "Клиент" } => Roles.Client,
                { Name: "Менеджер" } => Roles.Manager,
                _ => throw new ArgumentException(message: "invalid enum value", paramName: nameof(role)),
            };
        }

        public static readonly IEnumerable<SelectListItem> DayOfWeeks = Enum.GetValues(typeof(DayOfWeek))
                                                                             .Cast<DayOfWeek>()
                                                                             .Select(x => new SelectListItem
                                                                             {
                                                                                 Text = DayOfWeekMap[(int)x].Long,
                                                                                 Value = ((int) x).ToString()
                                                                             });

        public static IEnumerable<IEnumerable<T>> TakeMany<T>(this IEnumerable<T> data, int num)
        {
            var countD = data.Count();
            var output = new List<IEnumerable<T>>();
            for (int i = 0; i < countD; i+=num)
                output.Add(data.Skip(i).Take(num));

            return output;
        }

        public static T Choice<T>(this IEnumerable<T> data)
        {
            var count = data.Count();

            return data.ElementAt(rnd.Next(0, count - 1));
        }

        public static IEnumerable<TimeSpan> RangeTimeSpan(this TimeSpan start, TimeSpan step, int count) =>
            Enumerable.Range(0, count).Select(x => start + x * step);
    }
}
