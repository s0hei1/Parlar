using Microsoft.EntityFrameworkCore;
using ParlarTest.Controllers.ViewModels;
using ParlarTest.Core.Base;
using ParlarTest.Data.DB;
using ParlarTest.Entity.Models;
using ParlarTest.Extentions;

namespace ParlarTest.UseCases;

public class StudentFilterUseCase : BaseUseCase
{
    public StudentFilterUseCase(MyDBContext myDbContext) : base(myDbContext)
    {
    }

    public async Task<StudentRetrieveViewModel> RetrieveStudentById(int id)
    {
        TableExists();
        var student = await db.Students.FindAsync(id);
        if (student == null)
            throw new NullReferenceException();

        return student.ToStudentRetrieveViewModel();
    }

    public async Task<List<StudentRetrieveViewModel>> RetrieveStudentsByLessonId(int lessonId)
    {
        var lesson = await db.Lessons
            .Include(l => l.Students)
            .FirstOrDefaultAsync(l=> l.Id == lessonId);

        if (lesson == null)
            throw new NullReferenceException();
        
        return lesson.Students.ToStudentRetrieveVMList();
    }


    protected override bool TableExists()
    {
        if (db.Students == null)
            throw new NotFoundException("Entity set 'Students'  is null");

        return true;
    }
}