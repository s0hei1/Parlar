using Microsoft.EntityFrameworkCore;
using ParlarTest.Controllers.ViewModels;
using ParlarTest.Core.Base;
using ParlarTest.Core.Exceptions;
using ParlarTest.Data.DB;
using ParlarTest.Entity.Models;
using ParlarTest.Extentions;

namespace ParlarTest.UseCases;

public class LessonCRUD_UseCase : BaseUseCase
{
    public LessonCRUD_UseCase(MyDBContext context) : base(context)
    {
    }

    public async Task Create(LessonCreateViewModel vm)
    {
        TableExists();

        if (vm.Limit is < 1 or > 30)
            throw new LessonLimitException("the limit is out of range, set a number between 1 to 30");
        
        db.Lessons.Add(vm.ToLesson());
        await db.SaveChangesAsync();
    }

    public async Task<List<LessonRetrieveViewModel>> Retrieve()
    {
        TableExists();
        var lessons = await db.Lessons.ToListAsync();
        return lessons.ToLessonRetrieveViewModelsList();
    }


    public async Task Update(LessonUpdateViewModel vm)
    {
        TableExists();
        var lesson = await db.Lessons.FirstOrDefaultAsync(l => l.Id == vm.ID);

        if (lesson != null)
        {
            lesson.Name = vm.Name;
            lesson.Limit = vm.Limit;

            await db.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException(message: "the lesson does not exist");
        }
    }


    public async Task Delete(int id)
    {
        TableExists();
        var lesson = await db.Lessons.FindAsync(id);
        if (lesson != null)
        {
            db.Lessons.Remove(lesson);
            await db.SaveChangesAsync();
        }
        else
        {
            throw new NullReferenceException(message: "the lesson does not exist");
        }
    }
    

    protected override bool TableExists()
    {
        if (db.Lessons == null)
            throw new NotFoundException(message: "Entity set 'Lessons'  is null.");

        return true;
    }
}