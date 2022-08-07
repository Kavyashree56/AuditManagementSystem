using AuditSeverityAPI.Models;
using AuditSeverityAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditSeverityAPI.Repositories
{
    public class AuditSeverityRepos : IAuditSeverityRepos
    {
        private readonly AuditManagementSystemContext _context;

        public AuditSeverityRepos()
        {

        }

        public AuditSeverityRepos(AuditManagementSystemContext context)
        {
            _context = context;
        }

        public async Task<Audit> PostAudit(AuditDetails item)
        {
            Audit audit = null;
            if(item == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                audit = new Audit()
                {
                    Auditid = GetAuditid(item.Userid),
                    ProjectName = item.ProjectName,
                    ProjectManagerName = item.ProjectManagerName,
                    ApplicationOwnerName = item.ApplicationOwnerName,
                    AuditType = item.AuditType,
                    AuditDate = DateTime.Now,
                    ProjectExecutionStatus = item.ProjectExecutionStatus,
                    RemedialActionDuration = item.RemedialActionDuration,
                    Userid = item.Userid
                };
                await _context.Audit.AddAsync(audit);
                await _context.SaveChangesAsync();
            }
            return audit;
        }

        public Dictionary<string, int> GetInternaAndSOXNoCount(string auditType)
        {
            Dictionary<string, int> _internalandsoxdict = new Dictionary<string, int>();

            if (auditType == "Internal")
            {
                _internalandsoxdict.Add("Internal", 3);
            }
            else
            {
                if (auditType == "SOX")
                {
                    _internalandsoxdict.Add("SOX", 1);
                }
            }

            return _internalandsoxdict;


        }

        public int GetAuditid(int id)
        {
            int Id = _context.Audit.Single(user => user.Userid == id).Auditid;
            return Id;
        }
    }
}
