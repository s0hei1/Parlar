using ParlarTest.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParlarTest.Entity.Models
{
    public class Lesson
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Limit { get; set; }

        public List<Student> Students { get; set; } = new();

    }
}
