using ReportService.Api.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Api.Repository
{
    public interface IReportRepository
    {
        public Task<Guid> Create(Report report);

        public Task<bool> Update(Report report);

        public Task<bool> Delete(Guid id);

        public Task<IEnumerable<Report>> Get();

        public Task<Report> Get(Guid id);

    }
}
