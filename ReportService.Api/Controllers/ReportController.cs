using AutoMapper;
using ReportService.Api.Dto;
using ReportService.Api.Model;
using ReportService.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        private readonly IBus _bus;
        private readonly IConfiguration _configuration;

        public ReportController(IReportRepository reportRepository, IMapper mapper, IBus bus, IConfiguration configuration)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _bus = bus;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var reports = await _reportRepository.Get();
            var reportsDto = _mapper.Map<IEnumerable<ReportDto>>(reports);
            return Ok(reportsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var report = await _reportRepository.Get(id);
            if (report == null)
            {
                return NotFound("Report not found");
            }
            var reportDto = _mapper.Map<ReportDto>(report);
            return Ok(reportDto);
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> GetFile(Guid id)
        {
            var report = await _reportRepository.Get(id);
            if (report == null)
            {
                return NotFound("Report not found");
            }
            else if(report.ReportStatus != ReportStatus.COMPLETED)
            {
                return NotFound("File not found");
            }

            FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(report.ReportPath), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = "report.xlsx"
            };

            return result;
        }

        [HttpPost("generate-report")]
        public async Task<IActionResult> Post()
        {
            Report report = new Report();
            report.ReportRequestDate = DateTime.Now;
            report.ReportStatus = ReportStatus.PREPARING;
            await _reportRepository.Create(report);
            var reportDto = _mapper.Map<ReportDto>(report);

            string rHost = _configuration["RabbitMQ:HostName"];
            string rQueue = _configuration["RabbitMQ:ReportQueue"];

            Uri uri = new Uri("rabbitmq://" + rHost + "/" + rQueue);
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(reportDto);

            return CreatedAtAction(nameof(Get), new { id = reportDto.Id }, reportDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _reportRepository.Get(id) == null)
            {
                return NotFound("Report not found");
            }

            await _reportRepository.Delete(id);
            return NoContent();
        }


    }
}
