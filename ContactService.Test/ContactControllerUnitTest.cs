using AutoMapper;
using ContactService.Api.Configuration;
using ContactService.Api.Controllers;
using ContactService.Api.Data;
using ContactService.Api.Model;
using ContactService.Api.Repository;
using ContactService.Application.Dto;
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
        public static string connectionString = "Server=localhost;Port=5432;User Id=admin;Password=admin;Database=ContactDB_Test;SSL Mode=Disable;";

        static ContactControllerUnitTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<ContactContext>()
                .UseNpgsql(connectionString)
                .Options;
        }

        public ContactControllerUnitTest()
        {
            var context = new ContactContext(dbContextOptions);

            context.Database.Migrate();

            repository = new ContactRepository(context);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            mapper = mapperConfig.CreateMapper();

            //insert demo data
            ContactData.DEMO.ForEach(c =>
            {
                var contact = mapper.Map<Contact>(c);
                var insertedId = repository.Create(contact).ConfigureAwait(false).GetAwaiter().GetResult();
            });

        }

        #region Insert 

        [Fact]
        public async void InsertContact_ShouldReturnOkResult()
        {
            var controller = new ContactController(repository, mapper);

            var data = await controller.Post(ContactData.VALID) as OkObjectResult;

            Assert.IsType<OkObjectResult>(data);

            var obj = data.Value as ContactDto;

            Assert.NotNull(obj);

            Assert.Equal(ContactData.VALID.FirstName, obj.FirstName);
            Assert.Equal(ContactData.VALID.LastName, obj.LastName);
        }

        [Fact]
        public async void InsertContact_ShouldReturnBadRequest()
        {
            var controller = new ContactController(repository, mapper);
        
            var data = await controller.Post(ContactData.UNVALID);

            Assert.IsType<BadRequestResult>(data);
        }

        #endregion

        #region Get

        [Fact]
        public async void GetContact_ShouldReturnOkResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = ContactData.DEMO[0].Id;

            var data = await controller.Get(id);

            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetContact_ShouldReturnNotFoundResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = Guid.NewGuid();

            var data = await controller.Get(id);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void GetContact_ShouldMatchResult()
        {
            var controller = new ContactController(repository, mapper);

            var demo = ContactData.DEMO[0];

            var data = await controller.Get(demo.Id) as OkObjectResult;

            Assert.IsType<OkObjectResult>(data);

            var obj = data.Value as ContactDto;

            Assert.NotNull(obj);

            Assert.Equal(demo.FirstName, obj.FirstName);
            Assert.Equal(demo.LastName, obj.LastName);
            Assert.Equal(demo.Company, obj.Company);

            demo.ContactDetails.ForEach(demoContactDetail =>
            {
                var contactDetail = obj.ContactDetails.Find(cd => cd.Id == demoContactDetail.Id);

                Assert.Equal(demoContactDetail.ContactDetailType, contactDetail.ContactDetailType);
                Assert.Equal(demoContactDetail.ContactDetailContent, contactDetail.ContactDetailContent);
            });
        }

        #endregion

        #region Get All  

        [Fact]
        public async void GetAllContact_ShouldReturnOkResult()
        {
            var controller = new ContactController(repository, mapper);

            var data = await controller.Get();

            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetAllContact_ShouldMatchResult()
        {
            var controller = new ContactController(repository, mapper);

            var data = await controller.Get() as OkObjectResult;

            Assert.IsType<OkObjectResult>(data);

            var obj = data.Value as List<ContactDto>;

            Assert.NotNull(obj);

            ContactData.DEMO.ForEach(demoContact =>
            {
                var contact = obj.Find(c => c.Id == demoContact.Id);

                Assert.NotNull(contact);

                Assert.Equal(demoContact.FirstName, contact.FirstName);
                Assert.Equal(demoContact.LastName, contact.LastName);
                Assert.Equal(demoContact.Company, contact.Company);
            });

        }

        #endregion

        #region Update

        [Fact]
        public async void UpdateContact_ShouldReturnOkResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = ContactData.DEMO[0].Id;

            var data = await controller.Get(id) as OkObjectResult;

            Assert.IsType<OkObjectResult>(data);

            var obj = data.Value as ContactDto;

            Assert.NotNull(obj);

            var contact = new ContactDto()
            {
                Id = id,
                FirstName = "Updated First Name",
                LastName = "Updated Last Name",
                Company = "Updated Company Name",
            };

            var updatedResult = await controller.Put(id, contact) as NoContentResult;

            Assert.IsType<NoContentResult>(updatedResult);

            var updatedData = await controller.Get(id) as OkObjectResult;

            Assert.IsType<OkObjectResult>(updatedData);

            var updatedObj = updatedData.Value as ContactDto;

            Assert.NotNull(updatedObj);

            Assert.Equal(updatedObj.FirstName, contact.FirstName);
            Assert.Equal(updatedObj.LastName, contact.LastName);
            Assert.Equal(updatedObj.Company, contact.Company);
        }

        [Fact]
        public async void UpdateContact_ShouldReturnBadRequest()
        {
            var controller = new ContactController(repository, mapper);

            var id = ContactData.DEMO[0].Id;

            var contact = new ContactDto()
            {
                Id = id,
                FirstName = "Updated First Name",
                //LastName = "Updated Last Name", 
                Company = "Updated Company Name",
            };

            // LastName field is required
            var updatedResult = await controller.Put(id, contact);

            Assert.IsType<BadRequestResult>(updatedResult);
        }

        [Fact]
        public async void UpdateContact_ShouldReturnNotFoundResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = Guid.NewGuid();

            var contact = new ContactDto()
            {
                Id = id,
                FirstName = "Updated First Name",
                LastName = "Updated Last Name",
                Company = "Updated Company Name",
            };

            var updatedResult = await controller.Put(id, contact);

            Assert.IsType<NotFoundResult>(updatedResult);
        }

        #endregion

        #region Delete

        [Fact]
        public async void DeleteContact_ShouldReturnOkResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = ContactData.DEMO[0].Id;

            var data = await controller.Delete(id);

            Assert.IsType<NoContentResult>(data);

            data = await controller.Get(id);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void DeleteContact_ShouldReturnNotFoundResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = Guid.NewGuid();

            var data = await controller.Delete(id);

            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void DeleteContactDetail_ShouldReturnOkResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = ContactData.DEMO[0].Id;
            var detailId = ContactData.DEMO[0].ContactDetails[0].Id;

            var data = await controller.DeleteDetail(id, detailId);

            Assert.IsType<NoContentResult>(data);

            var getData = await controller.Get(id) as OkObjectResult;

            Assert.IsType<OkObjectResult>(data);

            var obj = getData.Value as ContactDto;

            Assert.NotNull(obj);

            var contactDetail = obj.ContactDetails.Find(cd => cd.Id == detailId);

            Assert.Null(contactDetail);
        }

        [Fact]
        public async void DeleteContactDetail_ShouldReturnNotFoundResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = ContactData.DEMO[0].Id;
            var detailId = Guid.NewGuid();

            var data = await controller.DeleteDetail(id, detailId);

            Assert.IsType<NotFoundResult>(data);
        }

        #endregion

    }
}
