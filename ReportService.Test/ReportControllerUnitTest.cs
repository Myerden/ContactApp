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
        private static DbContextOptions<ReportContext> dbContextOptions { get; set; }
        public static string connectionString = "Server=reportapidb;Port=5432;User Id=admin;Password=admin;Database=ReportDB_Test;SSL Mode=Disable;";

        static ReportControllerUnitTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<ReportContext>()
                .UseNpgsql(connectionString)
                .Options;
        }

        public ReportControllerUnitTest()
        {
            var context = new ReportContext(dbContextOptions);

            context.Database.Migrate();

            repository = new ReportRepository(context);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            mapper = mapperConfig.CreateMapper();

            //insert demo data
            ReportData.DEMO.ForEach(c =>
            {
                var report = mapper.Map<Report>(c);
                var insertedId = repository.Create(report).ConfigureAwait(false).GetAwaiter().GetResult();
            });

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

            var id = ReportData.DEMO[0].Id;

            var data = await controller.Get(id);

            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetReport_ShouldReturnNotFoundResult()
        {
            var controller = new ReportController(repository, mapper, null);

            var id = Guid.NewGuid();

            var data = await controller.Get(id);

            Assert.IsType<NotFoundResult>(data);
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

            var id = ReportData.DEMO[0].Id;

            var data = await controller.Delete(id);

            Assert.IsType<NoContentResult>(data);

            data = await controller.Get(id);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void DeleteContact_ShouldReturnNotFoundResult()
        {
            var controller = new ReportController(repository, mapper, null);

            var id = Guid.NewGuid();

            var data = await controller.Delete(id);

            Assert.IsType<NotFoundResult>(data);
        }

        #endregion

    }
}
