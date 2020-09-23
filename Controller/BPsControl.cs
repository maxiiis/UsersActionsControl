using EFModels.MainDB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Controller
{
    public class BPsControl
    {
        MainDBContext mainDB = new MainDBContext();
        public List<BPdto> GetBP()
        {
            var BPs = mainDB.BPs.Select(
                        b => new BPdto
                        {
                            Номер = b.Id,
                            Система = b.System.Name,
                            Название = b.Name,
                            Случаев = b.BPCases.Count
                        }).ToList();
            return BPs;
        }

        public List<BPCasedto> GetCases()
        {
            var Cases = mainDB.BPCases.Select(
                           c => new BPCasedto
                           {
                               НомерСлучая = c.CaseId,
                               ВремяНачала = GetStartTimeCase(c.CaseId),
                               ВремяОкончания = GetEndTimeCase(c.CaseId)
                           }).ToList();
            return Cases;
        }

        public string GetBPName(long BPId)
        {
            return mainDB.BPs.FirstOrDefault(s => s.Id == BPId).Name;
        }

        public string GetBPbyName(string Name)
        {
            var BPname = mainDB.BPs.FirstOrDefault(s => s.Name == Name).Name;
            return BPname;
        }
        private static string GetStartTimeCase(long caseId)
        {
            if (caseId == -1)
                return "";
            var @case = new CaseBuilder().CreateCase(caseId);
            return @case.Begin.TimeStamp.ToShortDateString();
        }

        private static string GetEndTimeCase(long caseId)
        {
            if (caseId == -1)
                return "";
            var @case = new CaseBuilder().CreateCase(caseId);
            return @case.Events.Last().TimeStamp.ToShortDateString();
        }

        #region For Parametres
        public BPsDTOs GetBps(BPsDTOs _tasks)
        {
            var bp = mainDB.BPs.ToList();
            foreach (var b in bp)
            {
                var task = new BPsDTO()
                {
                    Id = b.Id,
                    Название = b.Name
                };

                _tasks.Add(task);
            }
            return _tasks;
        }
        #endregion
    }
    public class BPdto
    {
        public long Номер { get; set; }
        public string Система { get; set; }
        public string Название { get; set; }
        public int Случаев { get; set; }
    }

    public class BPCasedto
    {
        public long НомерСлучая { get; set; }
        public string ВремяНачала { get; set; }
        public string ВремяОкончания { get; set; }
    }

    #region For Parametres
    public class BPsDTO
    {
        public long Id { get; set; }
        public string Название { get; set; }
    }

    public class BPsDTOs : ObservableCollection<BPsDTO>
    {

    }
    #endregion
}
