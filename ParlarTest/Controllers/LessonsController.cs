using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParlarTest.Controllers.ViewModels;
using ParlarTest.Core.Enum;
using ParlarTest.Core.Exceptions;
using ParlarTest.Data.DB;
using ParlarTest.Extentions;
using ParlarTest.UseCases;


namespace ParlarTest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LessonsController : ControllerBase
{
    private readonly MyDBContext _context = new();
    private LessonCRUD_UseCase crudUseCase;
    private LessonFilterUseCase FilterUseCase;

    public LessonsController()
    {
        crudUseCase = new LessonCRUD_UseCase(_context);
        FilterUseCase = new LessonFilterUseCase(_context);
    }

    [Authorize]
    [HttpGet("/GetLessonById/{id}")]
    public async Task<ActionResult<LessonRetrieveViewModel>> GetLessonById(int id)
    {
        try
        {
            return await FilterUseCase.RetrieveLessonById(id);
        }
        catch (Exception e)
        {
            return e switch
            {
                NotFoundException => BadRequest(e.Message),
                NullReferenceException => BadRequest(e.Message),
                _ => Problem(e.Message)
            };
        }
    }

    [Authorize]
    [HttpGet("/GetLessons/")]
    public async Task<ActionResult<IEnumerable<LessonRetrieveViewModel>>> GetLessons()
    {
        try
        {
            var lessons = await crudUseCase.Retrieve();
            return lessons;
        }
        catch (Exception e)
        {
            if (e is NotFoundException)
                return BadRequest(e.Message);

            return Problem(e.Message);
        }
    }

    [Authorize(Policy = AuthorizePolicy.RequireAdminRole)]
    [HttpPost("/CreateLesson/")]
    public async Task<ActionResult<LessonCreateViewModel>> CreateLesson(LessonCreateViewModel lessonCreate)
    {
        try
        {
            await crudUseCase.Create(lessonCreate);
            return Ok("Lesson created successfully.");
        }
        catch (Exception e)
        {
            if (e is NotFoundException)
                return BadRequest(e.Message);
            if (e is LessonLimitException)
                return BadRequest(e.Message);

            return Problem(e.Message);
        }
    }

    [Authorize(Policy = AuthorizePolicy.RequireAdminRole)]
    [HttpPut("/UpdateLesson/")]
    public async Task<ActionResult<LessonUpdateViewModel>> UpdateLesson(LessonUpdateViewModel lessonVM)
    {
        try
        {
            await crudUseCase.Update(lessonVM);
            return NoContent();
        }
        catch (Exception e)
        {
            return e switch
            {
                NotFoundException => BadRequest(e.Message),
                NullReferenceException => BadRequest(e.Message),
                _ => Problem(e.Message)
            };
        }
    }
    
    [Authorize(Policy = AuthorizePolicy.RequireAdminRole)]
    [HttpDelete("/DeleteLesson/{id}")]
    public async Task<ActionResult> DeleteLesson(int id)
    {
        try
        {
            await crudUseCase.Delete(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return e switch
            {
                NotFoundException => BadRequest(e.Message),
                NullReferenceException => BadRequest(e.Message),
                _ => Problem(e.Message)
            };
        }
    }
    
    [Authorize]
    [HttpGet("/GetLessonsByStudentId/{studentId}")]
    public async Task<ActionResult<IEnumerable<LessonRetrieveViewModel>>> RetrieveLessonByStudentId(int studentId)
    {
        try
        {
            var lessons = await FilterUseCase.RetrieveLessonsByStudentId(studentId);
            return lessons;
        }
        catch (Exception e)
        {
            return e switch
            {
                NotFoundException => BadRequest(e.Message),
                NullReferenceException => BadRequest(e.Message),
                _ => Problem(e.Message)
            };
        }
    }
    
    
}