using ParlarTest.Controllers.ViewModels;
using ParlarTest.Core.Enum;
using ParlarTest.Data.Entity;
using ParlarTest.Entity.Models;

namespace ParlarTest.Extentions;

public static class UserMapper
{
    public static User RegisterVM_toStudentUser(this AdminRegisterViewModel viewModel)
    {
        var model = new User()
        {
            UserName = viewModel.UserName,
            HashedPassword = viewModel.Password,
            Role = UserType.STUDENT
        };

        return model;
    }
        
    public static User RegisterVM_toAdmin(this AdminRegisterViewModel viewModel)
    {
        var model = new User
        {
            UserName = viewModel.UserName,
            HashedPassword = viewModel.Password,
            Role = UserType.ADMIN
        };

        return model;
    }
}