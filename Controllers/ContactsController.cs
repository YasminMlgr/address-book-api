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

    // Get contacts with optional search and sorting.
    [HttpGet]
    [ProducesResponseType(typeof(List<Contact>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<List<Contact>>> GetContacts(
        [FromQuery] string? search = null,
        [FromQuery] ContactSortBy sortBy = ContactSortBy.Id,
        [FromQuery] bool descending = false,
        [FromQuery] bool favoritesOnly = false)
    {
        if (!Enum.IsDefined(sortBy))
        {
            return BadRequest("Invalid sorting field.");
        }

        var query = _context.Contacts.AsNoTracking();

                if (favoritesOnly)
        {
            query = query.Where(contact => contact.IsFavorite);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim().ToLowerInvariant();

            query = query.Where(contact =>
                contact.FirstName.ToLowerInvariant().Contains(term) ||
                contact.LastName.ToLowerInvariant().Contains(term) ||
                contact.Email.ToLowerInvariant().Contains(term) ||
                contact.PhoneNumber.ToLowerInvariant().Contains(term) ||
                contact.Address.ToLowerInvariant().Contains(term) ||
                contact.City.ToLowerInvariant().Contains(term) ||
                contact.Country.ToLowerInvariant().Contains(term));
        }

        var sortedQuery = sortBy switch
        {
            ContactSortBy.FirstName => descending
                ? query.OrderByDescending(contact => contact.FirstName)
                : query.OrderBy(contact => contact.FirstName),

            ContactSortBy.LastName => descending
                ? query.OrderByDescending(contact => contact.LastName)
                : query.OrderBy(contact => contact.LastName),

            ContactSortBy.City => descending
                ? query.OrderByDescending(contact => contact.City)
                : query.OrderBy(contact => contact.City),

            _ => descending
                ? query.OrderByDescending(contact => contact.Id)
                : query.OrderBy(contact => contact.Id)
        };

        var contacts = await sortedQuery
            .ThenBy(contact => contact.Id)
            .ToListAsync();

        return Ok(contacts);
    }

    // Get a contact by ID.
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await _context.Contacts.FindAsync(id);

        if (contact is null)
        {
            return NotFound();
        }

        return Ok(contact);
    }

    // Create a contact.
    [HttpPost]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

    // Update an existing contact.
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





        // Update only the favorite status of a contact.
    [HttpPatch("{id:int}/favorite")]
    [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Contact>> UpdateFavorite(
        int id,
        UpdateFavoriteDto request)
    {
        if (request.IsFavorite is null)
        {
            return BadRequest("IsFavorite is required.");
        }

        var contact = await _context.Contacts.FindAsync(id);

        if (contact is null)
        {
            return NotFound();
        }

        contact.IsFavorite = request.IsFavorite.Value;

        await _context.SaveChangesAsync();

        return Ok(contact);
    }





}