using System.ComponentModel;
using ParlarTest.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using ParlarTest.Core.Enum;

namespace ParlarTest.Entity.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        

        [DefaultValue(0)] public int IsVerified { get; set; }

        public List<Lesson> Leesons { get; } = new();
        
        public int UserId { get; set; }
        public User User { get; set; }
    }
}