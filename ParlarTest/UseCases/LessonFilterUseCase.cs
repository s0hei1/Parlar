using Microsoft.EntityFrameworkCore;
using ParlarTest.Controllers.ViewModels;
using ParlarTest.Core.Base;
using ParlarTest.Data.DB;
using ParlarTest.Extentions;

namespace ParlarTest.UseCases;

public class LessonFilterUseCase : BaseUseCase
{
    public LessonFilterUseCase(MyDBContext myDbContext) : base(myDbContext)
    {
    }

    public async Task<List<LessonRetrieveViewModel>> RetrieveLessonsByStudentId(int studentId)
    {
        var student = await db.Students
            .Include(s => s.Leesons)
            .FirstOrDefaultAsync(s => s.Id == studentId);

        if (student == null)
            throw new NullReferenceException();

        var lessons = student.Leesons;
        
        return lessons.ToLessonRetrieveViewModelsList();

    }

    public async Task<LessonRetrieveViewModel> RetrieveLessonById(int Id)
    {
        TableExists();
        var lesson = await db.Lessons.FindAsync(Id);

        if (lesson == null)
            throw new NullReferenceException("the lesson does not exist");
        return lesson.ToLessonRetrieveViewModel();
    }

    protected override bool TableExists()
    {
        if (db.Lessons == null)
            throw new NotFoundException("Entity set 'Lessons'  is null.");

        return true;
    }
}