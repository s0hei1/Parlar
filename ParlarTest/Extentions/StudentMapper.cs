using ParlarTest.Controllers.ViewModels;
using ParlarTest.Core.Enum;
using ParlarTest.Entity.Models;

namespace ParlarTest.Extentions
{
    public static class StudentMapper
    {
        public static StudentRetrieveViewModel ToStudentRetrieveViewModel(this Student studennt)
        {
            var model = new StudentRetrieveViewModel()
            {
                Id = studennt.Id,
                isVerified = studennt.IsVerified,
                Name = studennt.Name,
                userId = studennt.UserId
            };

            return model;
        }
        public static Student ToStudent(this StudentCreateViewModel vm)
        {
            var model = new Student
            {
                IsVerified = vm.isVerified,
                Name = vm.Name,
                UserId = vm.UserId
            };

            return model;
        }
        
        public static List<StudentRetrieveViewModel> ToStudentRetrieveVMList(this List<Student> students)
        {
            return students.Select(lesson => lesson.ToStudentRetrieveViewModel()).ToList();
        }
    }
}
