using System.ComponentModel.DataAnnotations;

namespace AddressBookApi.DTOs;

public class UpdateFavoriteDto
{
    [Required]
    public bool? IsFavorite { get; set; }
}