using Microsoft.EntityFrameworkCore;
using ParlarTest.Core.Base;
using ParlarTest.Core.Exceptions;
using ParlarTest.Data.DB;
using ParlarTest.Data.Entity;
using ParlarTest.Entity.Models;
using ParlarTest.Extentions;

namespace ParlarTest.UseCases;

public class StudentActionsUseCase : BaseUseCase
{
    public StudentActionsUseCase(MyDBContext myDbContext) : base(myDbContext) { }


    public async Task AddLesson(int lessonId, int studentId)
    {
        TableExists();

        var student = await db.Students.FindAsync(studentId);

        
        var lesson = await db.Lessons
            .Include(l => l.Students)
            .FirstOrDefaultAsync(l => l.Id == lessonId);
        
        if (student == null || lesson == null)
            throw new NullReferenceException();
        
        var count = lesson.Students.Count;
        var limit = lesson.Limit;
        if (count >= limit)
            throw new LessonLimitException($"The {lesson.Name}'s capacity is full");
        
        lesson.Students.Add(student);
        await db.SaveChangesAsync();
    }

    public async Task RemoveLesson(int lessonId, int studentId)
    {
        TableExists();
        var student = await db.Students
            .Include(s => s.Leesons)
            .FirstOrDefaultAsync(s => s.Id == studentId);
        
        var lesson = await db.Lessons.FindAsync(lessonId);
        
        if (student == null || lesson == null)
            throw new NullReferenceException();
        
        lesson.Students.Remove(student);
        await db.SaveChangesAsync();
    }


    protected override bool TableExists()
    {
        if (db.Lessons == null || db.Students == null)
            throw new NotFoundException("Entity set 'Lessons or Students'  is null.");
        return true;
    }
}