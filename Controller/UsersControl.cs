using EFModels.LogsDB;
using EFModels.MainDB;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;

namespace Controller
{
    public class UsersControl
    {
        private LogDBContext logDB = new LogDBContext();
        private MainDBContext mainDB = new MainDBContext();

        public UserDTOs GetUsers(UserDTOs _tasks)
        {
            var us = logDB.Users.Where(s => s.Department != null).ToList();
            foreach (var u in us)
            {
                var task = new UserDTO()
                {
                    Логин = u.Id,
                    Фамилия = u.LastName,
                    Имя = u.FirstName,
                    Отчество = u.MiddleName,
                    Департамент = u.Department,
                    Филиал = u.Filial
                };

                _tasks.Add(task);
            }
            return _tasks;
        }

        public AccessDTOs GetAccesses(AccessDTOs access, UserDTO selectedUser)
        {
            access.Clear();

            var bps = mainDB.BPs.Include(s => s.System).ToList();

            foreach (var bp in bps)
            {
                var stndrtCase = bp.StandartCase;

                foreach (var ev in stndrtCase.Events)
                {
                    var task = new AccessDTO()
                    {
                        БП = bp.Name,
                        Система = bp.System.Name,
                        Этап = ev.Name,
                        Доступ = bp.AccessMatrix.Matrix[selectedUser.Логин][ev.Name]
                        //Доступ = logDB.EventLogs.Include(s => s.Activity).FirstOrDefault(s => s.ResourceId == selectedItem.Логин && s.Activity.ActivityText2 == ev.Name) != null ? true : false
                    };
                    access.Add(task);
                }
            }
            return access;
        }
    }
    public class UserDTO
    {
        public string Логин { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public string Филиал { get; set; }
        public string Департамент { get; set; }

    }

    public class UserDTOs : ObservableCollection<UserDTO>
    {

    }

    public class AccessDTO
    {
        public string Система { get; set; }
        public string БП { get; set; }
        public string Этап { get; set; }
        public bool Доступ
        {
            get; set;
        }

    }

    public class AccessDTOs : ObservableCollection<AccessDTO>
    {

    }
}
