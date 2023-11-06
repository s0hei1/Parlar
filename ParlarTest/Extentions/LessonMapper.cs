using ParlarTest.Controllers.ViewModels;
using ParlarTest.Entity.Models;

namespace ParlarTest.Extentions
{
    public static class LessonMapper
    {
        public static Lesson ToLesson(this LessonCreateViewModel createViewModel)
        {
            var model = new Lesson

            {
                Name = createViewModel.Name,
                Limit = createViewModel.Limit
            };

            return model;
        }

        public static LessonRetrieveViewModel ToLessonRetrieveViewModel(this Lesson lesson)
        {
            var model = new LessonRetrieveViewModel
            {
                ID = lesson.Id,
                Name = lesson.Name,
                Limit = lesson.Limit
            };

            return model;
        }

        public static List<LessonRetrieveViewModel> ToLessonRetrieveViewModelsList(this List<Lesson> lessons)
        {
            return lessons.Select(lesson => lesson.ToLessonRetrieveViewModel()).ToList();
        }
        
    }
}