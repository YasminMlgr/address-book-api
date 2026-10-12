using AddressBookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AddressBookApi.Data;

public class AddressBookContext : DbContext
{
    public AddressBookContext(DbContextOptions<AddressBookContext> options)
        : base(options)
    {
    }

    public DbSet<Contact> Contacts => Set<Contact>();
}