using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FeedbackSchool.Models
{
    public class FeedbackList
    {
        [Display(Name = "Выберите школу, в которой обучаетесь")]
        [Required(ErrorMessage = "Выберите школу, в которой обучаетесь")]
        [NotNull]
        public string School { get; set; }
        
        [Display(Name = "Выберите класс, в котором обучаетесь")]
        [Required(ErrorMessage = "Выберите класс, в котором обучаетесь")]
        [NotNull]
        public string Class { get; set; }
        
        [Display(Name = "Представьтесь")]
        [Required(ErrorMessage = "Представьтесь")]
        [NotNull]
        public string Name { get; set; }
        
        [Display(Name = "Отзыв о вашей школе")]
        [Required(ErrorMessage = "Отзыв о вашей школе")]
        [NotNull]
        public string Feedback { get; set; }
        
        [Display(Name = "Ваши любимые уроки")]
        [AllowNull]
        public string FavoriteLessons { get; set; }
        public string DateTime { get; set; }
        public int Id { get; set; }
    }
}