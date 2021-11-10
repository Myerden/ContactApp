using AutoMapper;
using ContactService.Api.Configuration;
using ContactService.Api.Controllers;
using ContactService.Api.Data;
using ContactService.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContactService.Test
{
    public class ContactControllerUnitTest
    {
        private IContactRepository repository;
        IMapper mapper;
        private static DbContextOptions<ContactContext> dbContextOptions { get; set; }
        public static string connectionString = "Server=localhost;Port=5432;User Id=admin;Password=admin;Database=ContactDB;SSL Mode=Disable;";

        static ContactControllerUnitTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<ContactContext>()
                .UseNpgsql(connectionString)
                .Options;
        }

        public ContactControllerUnitTest()
        {
            var context = new ContactContext(dbContextOptions);

            repository = new ContactRepository(context);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            mapper = mapperConfig.CreateMapper();
        }

        #region Insert 

        [Fact]
        public async void InsertContact_ShouldReturnOkResult()
        {
            //Arrange  
            var controller = new ContactController(repository, mapper);

            //Act  
            var data = await controller.Post(ContactData.VALID);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void InsertContact_ShouldReturnBadRequest()
        {
            //Arrange  
            var controller = new ContactController(repository, mapper);

            //Act              
            var data = await controller.Post(ContactData.UNVALID);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

    }
}
