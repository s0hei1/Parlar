using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParlarTest.Data.DB;
using ParlarTest.Data.Entity;
using ParlarTest.Entity.Models;

namespace ParlarTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class Test : ControllerBase
{
    private readonly MyDBContext _db = new();

    [HttpGet("/Students/")]
    public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
    {
        var students = await _db.Students.ToListAsync();

        return students;
    }
    
    [HttpGet("/Users/")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var students = await _db.Users.ToListAsync();

        return students;
    }
    
    [HttpGet("/Lessons/")]
    public async Task<ActionResult<List<Lesson>>> GetLessons()
    {
        var students = await _db.Lessons
            .Include(s=> s.Students)
            .ToListAsync();

        return students;
    }
    

}