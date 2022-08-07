﻿using AuditSeverityAPI.Models;
using AuditSeverityAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditSeverityAPI.Repositories
{
    public interface IAuditSeverityRepos
    {
        Task<Audit> PostAudit(AuditDetails item);

        public Dictionary<string, int> GetInternaAndSOXNoCount(string auditType);

        public int GetAuditid(int id);
    }
}
