using ParlarTest.Controllers.ViewModels;
using ParlarTest.Core.Exceptions;
using ParlarTest.Data.Entity;

namespace ParlarTest.Extentions;

public static class Check
{
    public static void CheckPassword(this AdminRegisterViewModel viewModel)
    {
        if (viewModel.Password != viewModel.ReEnterPassword)
            throw new PasswordExceptions("the Passwords aren't same !!!");

        if (viewModel.Password.Length <= 4)
            throw new PasswordExceptions("the Password is short");
    }

    public static void VerifyPassword(this User user,string password)
    {
        if (!user.HashedPassword.Equals(password))
            throw new PasswordExceptions("the password is wrong");
    }
}