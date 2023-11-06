using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParlarTest.Controllers.ViewModels;
using ParlarTest.Core.Enum;
using ParlarTest.Core.Exceptions;
using ParlarTest.Data.DB;
using ParlarTest.Extentions;
using ParlarTest.UseCases;

namespace ParlarTest.Controllers
{
    [Route("api/[controller]/{action}")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly MyDBContext _context = new();
        private StudentActionsUseCase actionsUseCase;
        private StudentCRUD_UseCase crudUseCase;
        private StudentFilterUseCase filterUseCase;

        public StudentsController()
        {
            actionsUseCase = new StudentActionsUseCase(_context);
            crudUseCase = new StudentCRUD_UseCase(_context);
            filterUseCase = new StudentFilterUseCase(_context);
        }


        [Authorize(Policy = AuthorizePolicy.RequireAdminRole)]
        [HttpGet("/GetStudents/")]
        public async Task<ActionResult<IEnumerable<StudentRetrieveViewModel>>> GetStudents()
        {
            try
            {
                return await crudUseCase.Retrieve();
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
        [HttpGet("/GetStudentById/{id}")]
        public async Task<ActionResult<StudentRetrieveViewModel>> GetStudent(int id)
        {
            try
            {
                return await filterUseCase.RetrieveStudentById(id);
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
        [HttpPut("/UpdateStudent/{id}")]
        public async Task<ActionResult<StudentUpdateViewModel>> PutStudent(StudentUpdateViewModel vm)
        {
            try
            {
                await crudUseCase.Update(vm);
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
        [HttpPost("/CreateStudent/")]
        public async Task<ActionResult<StudentCreateViewModel>> CreateStudent(StudentCreateViewModel vm)
        {
            try
            {
                await crudUseCase.Create(vm);
                return Ok();
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
        [HttpDelete("/DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
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

        [Authorize(Policy = AuthorizePolicy.RequireAdminRole)]
        [HttpGet("/GetStudentsByLessonId/{lessonId}")]
        public async Task<ActionResult<IEnumerable<StudentRetrieveViewModel>>> GetStudentsByLessonId(
            int lessonId)
        {
            try
            {
                return await filterUseCase.RetrieveStudentsByLessonId(lessonId);
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
        [HttpGet("/AddLessonForStudent/{studentId}/{lessonId}")]
        public async Task<ActionResult> AddLessonForStudent(int studentId, int lessonId)
        {
            try
            {
                await actionsUseCase.AddLesson(lessonId, studentId);
                return Ok("Successfully registered");
            }
            catch (Exception e)
            {
                return e switch
                {
                    NotFoundException => BadRequest(e.Message),
                    NullReferenceException => BadRequest(e.Message),
                    LessonLimitException => BadRequest(e.Message),
                    _ => Problem(e.Message)
                };
            }
        }

        [Authorize]
        [HttpGet("/RemoveLessonForStudent/{studentId}/{lessonId}")]
        public async Task<ActionResult> RemoveLessonForStudent(int studentId, int lessonId)
        {
            try
            {
                await actionsUseCase.RemoveLesson(lessonId, studentId);
                return Ok();
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
}