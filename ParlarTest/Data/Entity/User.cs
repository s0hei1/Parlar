using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ParlarTest.Core.Enum;

namespace ParlarTest.Data.Entity;

public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required] public UserType Role { get; set; }
    [Required] public string UserName { get; set; }

    [Required] public string HashedPassword { get; set; }
}