using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReportService.Api.Configuration;
using ReportService.Api.Controllers;
using ReportService.Api.Data;
using ReportService.Api.Dto;
using ReportService.Api.Model;
using ReportService.Api.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ReportService.Test
{
    public class ReportControllerUnitTest
    {
        private IReportRepository repository;
        IMapper mapper;
        private DbContextOptions<ReportContext> dbContextOptions { get; set; }
        private ReportData reportData = new ReportData();
        public static string connectionString = "Server=localhost;Port=5433;User Id=admin;Password=admin;Database=ReportDB_Test;SSL Mode=Disable;";

        static ReportControllerUnitTest()
        {
        }

        public ReportControllerUnitTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<ReportContext>()
                .UseNpgsql(connectionString)
                .Options;

            var context = new ReportContext(dbContextOptions);

            context.Database.Migrate();

            repository = new ReportRepository(context);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            mapper = mapperConfig.CreateMapper();

            //insert demo data
            for (int i = 0; i < reportData.DEMO.Count; i++)
            {
                var report = mapper.Map<Report>(reportData.DEMO[i]);
                var insertedId = repository.Create(report).ConfigureAwait(false).GetAwaiter().GetResult();
                reportData.DEMO[i] = mapper.Map<ReportDto>(report);
            }

            context.ChangeTracker.Clear();
        }


        #region Insert 

        //[Fact]
        //public async void GenerateReport_ShouldReturnOkResult()
        //{
            
        //}

        #endregion

        #region Get

        [Fact]
        public async void GetReport_ShouldReturnOkResult()
        {
            var controller = new ReportController(repository, mapper, null);

            var id = reportData.DEMO[0].Id;

            var data = await controller.Get(id);

            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetReport_ShouldReturnNotFoundResult()
        {
            var controller = new ReportController(repository, mapper, null);

            var id = Guid.NewGuid();

            var data = await controller.Get(id);

            Assert.IsType<NotFoundObjectResult>(data);
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetAllReport_ShouldReturnOkResult()
        {
            var controller = new ReportController(repository, mapper, null);

            var data = await controller.Get();

            Assert.IsType<OkObjectResult>(data);
        }

        #endregion

        #region Delete

        [Fact]
        public async void DeleteReport_ShouldReturnOkResult()
        {
            var controller = new ReportController(repository, mapper, null);

            var id = reportData.DEMO[0].Id;

            var data = await controller.Delete(id);

            Assert.IsType<NoContentResult>(data);

            data = await controller.Get(id);

            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public async void DeleteReport_ShouldReturnNotFoundResult()
        {
            var controller = new ReportController(repository, mapper, null);

            var id = Guid.NewGuid();

            var data = await controller.Delete(id);

            Assert.IsType<NotFoundObjectResult>(data);
        }

        #endregion

    }
}
