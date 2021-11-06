﻿using AutoMapper;
using ReportService.Api.Dto;
using ReportService.Api.Model;
using ReportService.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;

        public ReportController(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
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

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReportDto reportDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var report = _mapper.Map<Report>(reportDto);
            await _reportRepository.Create(report);
            reportDto = _mapper.Map<ReportDto>(report);
            return CreatedAtAction(nameof(Get), new { id = reportDto.Id }, reportDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ReportDto reportDto)
        {
            if (await _reportRepository.Get(id) == null)
            {
                return NotFound("Report not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reportDto.Id)
            {
                return BadRequest();
            }

            var report = _mapper.Map<Report>(reportDto);
            await _reportRepository.Update(report);
            return NoContent();
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
