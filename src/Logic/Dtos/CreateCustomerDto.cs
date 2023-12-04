using System.ComponentModel.DataAnnotations;

namespace Logic.Dtos;

public class CreateCustomerDto
{
    [Required]
    [MaxLength(100, ErrorMessage = "Name is too long")]
    public virtual string Name { get; set; }

    [Required] 
    [EmailAddress] public virtual string Email { get; set; }
}