using AddressBookApi.Data;
using AddressBookApi.DTOs;
using AddressBookApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApi.Controllers;

[ApiController]
[Route("api/contacts")]
public class ContactsController : ControllerBase
{
    private readonly AddressBookContext _context;

    public ContactsController(AddressBookContext context)
    {
        _context = context;
    }

   
    [HttpGet]
    public async Task<ActionResult<List<Contact>>> GetContacts()
    {
        var contacts = await _context.Contacts
            .AsNoTracking()
            .ToListAsync();

        return Ok(contacts);
    }

    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if (contact is null)
        {
            return NotFound();
        }

        return Ok(contact);
    }

    
    [HttpPost]
    public async Task<ActionResult<Contact>> CreateContact(
        CreateContactDto request)
    {
        var contact = new Contact
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = request.Email.Trim(),
            PhoneNumber = request.PhoneNumber.Trim(),
            Address = request.Address.Trim(),
            City = request.City.Trim(),
            Country = request.Country.Trim(),
            IsFavorite = request.IsFavorite
        };

        _context.Contacts.Add(contact);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetContact),
            new { id = contact.Id },
            contact);
    }



    
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateContact(
        int id,
        UpdateContactDto request)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if (contact is null)
        {
            return NotFound();
        }

        contact.FirstName = request.FirstName.Trim();
        contact.LastName = request.LastName.Trim();
        contact.Email = request.Email.Trim();
        contact.PhoneNumber = request.PhoneNumber.Trim();
        contact.Address = request.Address.Trim();
        contact.City = request.City.Trim();
        contact.Country = request.Country.Trim();
        contact.IsFavorite = request.IsFavorite;

        await _context.SaveChangesAsync();

        return NoContent();
    }




        // Delete an existing contact.
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if (contact is null)
        {
            return NotFound();
        }

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();

        return NoContent();
    }





}