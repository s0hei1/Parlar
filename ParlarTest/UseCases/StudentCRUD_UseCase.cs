using Microsoft.EntityFrameworkCore;
using ParlarTest.Controllers.ViewModels;
using ParlarTest.Core.Base;
using ParlarTest.Data.DB;
using ParlarTest.Data.Entity;
using ParlarTest.Entity.Models;
using ParlarTest.Extentions;

namespace ParlarTest.UseCases;

public class StudentCRUD_UseCase : BaseUseCase
{
    public StudentCRUD_UseCase(MyDBContext myDbContext) : base(myDbContext) { }

    public async Task Create(StudentCreateViewModel vm)
    {
        TableExists();
        db.Students.Add(vm.ToStudent());
        await db.SaveChangesAsync();
    }

    public async Task Update(StudentUpdateViewModel vm)
    {
        TableExists();
        var student = await db.Students.FindAsync(vm.Id);

        if (student == null)
            throw new NullReferenceException($"there is no student with name {vm.Name}");

        student.Name = vm.Name;
        student.IsVerified = vm.isVerified;

        await db.SaveChangesAsync();
    }


    public async Task<List<StudentRetrieveViewModel>> Retrieve()
    {
        TableExists();
        var students = await db.Students.ToListAsync();
        return students.ToStudentRetrieveVMList();
    }


    public async Task Delete(int id)
    {
        TableExists();
        var student = await db.Students.FindAsync(id);

        if (student == null)
            throw new NullReferenceException($"there is no student with id {id}");

        db.Students.Remove(student);
    }
    
    protected override bool TableExists()
    {
        if (db.Students == null)
            throw new NotFoundException("Entity set 'Students'  is null.");

        return true;
    }
}