﻿using AutoMapper;
using ContactService.Api.Dto;
using ContactService.Api.Model;
using ContactService.Api.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ContactService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;

        public ContactController(IContactRepository contactRepository, IMapper mapper)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var contacts = await _contactRepository.Get();
            var contactsDto = _mapper.Map<IEnumerable<ContactDto>>(contacts);
            return Ok(contactsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var contact = await _contactRepository.Get(id);
            if (contact == null)
            {
                return NotFound("Contact not found");
            }
            var contactDto = _mapper.Map<ContactDto>(contact);
            return Ok(contactDto);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ContactDto contactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contact = _mapper.Map<Contact>(contactDto);
            await _contactRepository.Create(contact);
            contactDto = _mapper.Map<ContactDto>(contact);
            return CreatedAtAction(nameof(Get), new { id = contactDto.Id }, contactDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] ContactDto contactDto)
        {
            if (await _contactRepository.Get(id) == null)
            {
                return NotFound("Contact not found");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactDto.Id)
            {
                return BadRequest();
            }

            var contact = _mapper.Map<Contact>(contactDto);
            await _contactRepository.Update(contact);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await _contactRepository.Get(id) == null)
            {
                return NotFound("Contact not found");
            }

            await _contactRepository.Delete(id);
            return NoContent();
        }
    }
}
