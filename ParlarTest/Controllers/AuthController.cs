using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParlarTest.Controllers.ViewModels;
using ParlarTest.Core;
using ParlarTest.Core.Exceptions;
using ParlarTest.Data.DB;
using ParlarTest.Entity.Models;
using ParlarTest.Extentions;

namespace ParlarTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly MyDBContext _db = new();

        [HttpPost("/StudentRegister/")]
        public async Task<ActionResult<StudentRegisterViewModel>> studentRegister(StudentRegisterViewModel? registerViewModel)
        {
            if (registerViewModel == null)
            {
                return BadRequest();
            }
            try
            {
                registerViewModel.CheckPassword();
                
                _db.Users.Add(registerViewModel.RegisterVM_toStudentUser());

                await _db.SaveChangesAsync();
                
                var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == registerViewModel.UserName);
                
                _db.Students.Add(new Student
                {
               
                    Name = registerViewModel.Name,
                    User = user,
                    UserId = user.Id
                });
                await _db.SaveChangesAsync();

                return Ok("The registeration was successfuly");
            }
            catch (Exception e)
            {
                if (e is PasswordExceptions)
                    return BadRequest(e.Message);

                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("/AdminRegister/")]
        public async Task<ActionResult<AdminRegisterViewModel>> adminRegister(AdminRegisterViewModel? registerViewModel)
        {
            if (registerViewModel == null)
            {
                return BadRequest();
            }
            try
            {
                registerViewModel.CheckPassword();
                _db.Users.Add(registerViewModel.RegisterVM_toAdmin());
                await _db.SaveChangesAsync();

                return Ok("The registeration was successfuly");
            }
            catch (Exception e)
            {
                if (e is PasswordExceptions)
                    return BadRequest(e.Message);

                return BadRequest(e.Message);
            }
        }
        
        [HttpPost("/Login/")]
        public async Task<ActionResult<LoginViewModel>> login(LoginViewModel viewModel)
        {
            try
            {
                var user = await _db.Users.SingleOrDefaultAsync(x => x.UserName == viewModel.UserName);
                
                user.VerifyPassword(viewModel.Password);
                var token = JWTGenerator.generate(user);
                return Ok(token);
            }
            catch (Exception e)
            {
                if (e is PasswordExceptions)
                    return Unauthorized(e.Message);
                
                return BadRequest(e.Message);
            }
        }
    }
}