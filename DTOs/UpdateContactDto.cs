using System.ComponentModel.DataAnnotations;

namespace AddressBookApi.DTOs;

public class UpdateContactDto
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(254)]
    public string Email { get; set; } = string.Empty;

    [StringLength(30)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(300)]
    public string Address { get; set; } = string.Empty;

    [StringLength(100)]
    public string City { get; set; } = string.Empty;

    [StringLength(100)]
    public string Country { get; set; } = string.Empty;

    public bool IsFavorite { get; set; }
}