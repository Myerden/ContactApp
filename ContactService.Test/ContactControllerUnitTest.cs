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
        private DbContextOptions<ContactContext> dbContextOptions { get; set; }
        private ContactData contactData = new ContactData();
        public static string connectionString = "Server=localhost;Port=5432;User Id=admin;Password=admin;Database=ContactDB_Test;SSL Mode=Disable;";


        public ContactControllerUnitTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<ContactContext>()
                .UseNpgsql(connectionString)
                .Options;

            var context = new ContactContext(dbContextOptions);

            context.Database.Migrate();

            repository = new ContactRepository(context);

            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            mapper = mapperConfig.CreateMapper();

            //insert demo data
            for (int i = 0; i < contactData.DEMO.Count; i++)
            {
                var contact = mapper.Map<Contact>(contactData.DEMO[i]);
                var insertedId = repository.Create(contact).ConfigureAwait(false).GetAwaiter().GetResult();
                contactData.DEMO[i] = mapper.Map<ContactDto>(contact);
            }

            context.ChangeTracker.Clear();
        }

        #region Insert 

        [Fact]
        public async void InsertContact_ShouldReturnOkResult()
        {
            var controller = new ContactController(repository, mapper);

            controller.ValidateModel(contactData.VALID);

            var data = await controller.Post(contactData.VALID);

            Assert.IsType<CreatedAtActionResult>(data);
        }

        [Fact]
        public async void InsertContact_ShouldReturnBadRequest()
        {
            var controller = new ContactController(repository, mapper);

            controller.ValidateModel(contactData.UNVALID);

            var data = await controller.Post(contactData.UNVALID);

            Assert.IsType<BadRequestObjectResult>(data);
        }

        #endregion

        #region Get

        [Fact]
        public async void GetContact_ShouldReturnOkResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = contactData.DEMO[0].Id;

            var data = await controller.Get(id);

            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void GetContact_ShouldReturnNotFoundResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = Guid.NewGuid();

            var data = await controller.Get(id);

            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public async void GetContact_ShouldMatchResult()
        {
            var controller = new ContactController(repository, mapper);

            var demo = contactData.DEMO[0];

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

            contactData.DEMO.ForEach(demoContact =>
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

            var id = contactData.DEMO[0].Id;

            contactData.DEMO[0].FirstName = "Updated First Name";
            contactData.DEMO[0].LastName = "Updated Last Name";
            contactData.DEMO[0].Company = "Updated Company Name";

            controller.ValidateModel(contactData.DEMO[0]);

            var updatedResult = await controller.Put(id, contactData.DEMO[0]);

            Assert.IsType<NoContentResult>(updatedResult);

            var updatedData = await controller.Get(id) as OkObjectResult;

            Assert.IsType<OkObjectResult>(updatedData);

            var updatedObj = updatedData.Value as ContactDto;

            Assert.NotNull(updatedObj);

            Assert.Equal(updatedObj.FirstName, contactData.DEMO[0].FirstName);
            Assert.Equal(updatedObj.LastName, contactData.DEMO[0].LastName);
            Assert.Equal(updatedObj.Company, contactData.DEMO[0].Company);
        }

        [Fact]
        public async void UpdateContact_ShouldReturnBadRequest()
        {
            var controller = new ContactController(repository, mapper);

            var id = contactData.DEMO[0].Id;

            var contact = new ContactDto()
            {
                Id = id,
                FirstName = "Updated First Name",
                //LastName = "Updated Last Name", 
                Company = "Updated Company Name",
            };

            controller.ValidateModel(contact);

            // LastName field is required
            var updatedResult = await controller.Put(id, contact);

            Assert.IsType<BadRequestObjectResult>(updatedResult);
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

            Assert.IsType<NotFoundObjectResult>(updatedResult);
        }

        #endregion

        #region Delete

        [Fact]
        public async void DeleteContact_ShouldReturnOkResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = contactData.DEMO[0].Id;

            var data = await controller.Delete(id);

            Assert.IsType<NoContentResult>(data);

            data = await controller.Get(id);

            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public async void DeleteContact_ShouldReturnNotFoundResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = Guid.NewGuid();

            var data = await controller.Delete(id);

            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public async void DeleteContactDetail_ShouldReturnOkResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = contactData.DEMO[0].Id;
            var detailId = contactData.DEMO[0].ContactDetails[0].Id;

            var data = await controller.DeleteDetail(id, detailId);

            Assert.IsType<NoContentResult>(data);

            var getData = await controller.Get(id) as OkObjectResult;

            Assert.IsType<OkObjectResult>(getData);

            var obj = getData.Value as ContactDto;

            Assert.NotNull(obj);

            var contactDetail = obj.ContactDetails.Find(cd => cd.Id == detailId);

            Assert.Null(contactDetail);
        }

        [Fact]
        public async void DeleteContactDetail_ShouldReturnNotFoundResult()
        {
            var controller = new ContactController(repository, mapper);

            var id = contactData.DEMO[0].Id;
            var detailId = Guid.NewGuid();

            var data = await controller.DeleteDetail(id, detailId);

            Assert.IsType<NotFoundObjectResult>(data);
        }

        #endregion

    }
}
