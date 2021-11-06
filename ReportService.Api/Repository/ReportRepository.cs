using ReportService.Api.Data;
using ReportService.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Api.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportContext _dbContext;

        public ReportRepository(ReportContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Create(Report report)
        {
            report.CreatedAt = DateTime.Now;
            await _dbContext.AddAsync(report);
            await _dbContext.SaveChangesAsync();
            return report.Id;
        }

        public async Task<bool> Update(Report report)
        {
            try
            {
                report.UpdatedAt = DateTime.Now;
                _dbContext.Entry(report).State = EntityState.Modified;
                _dbContext.Entry(report).Property(c => c.CreatedAt).IsModified = false;
                
                var num = await _dbContext.SaveChangesAsync();
                return num > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var report = await Get(id);
                _dbContext.Reports.Remove(report);
                var num = await _dbContext.SaveChangesAsync();
                return num > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Report>> Get()
        {
            return await _dbContext.Reports.ToListAsync();
        }

        public async Task<Report> Get(Guid id)
        {
            return await _dbContext.Reports.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

    }
}
