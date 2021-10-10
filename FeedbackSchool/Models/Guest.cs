using System.ComponentModel.DataAnnotations;

namespace FeedbackSchool.Models
{
    public class Guest
    {
        [Display(Name = "Выберите школу, в которой обучаетесь")]
        [Required(ErrorMessage = "Выберите школу, в которой обучаетесь")]
        public string School { get; set; }
        
        [Display(Name = "Выберите класс, в котором обучаетесь")]
        [Required(ErrorMessage = "Выберите класс, в котором обучаетесь")]
        public string Class { get; set; }
        
        [Display(Name = "Представьтесь")]
        [Required(ErrorMessage = "Представьтесь")]
        public string Name { get; set; }
        
        [Display(Name = "Отзыв о вашей школе")]
        [Required(ErrorMessage = "Отзыв о вашей школе")]
        public string Feedback { get; set; }
        
        [Display(Name = "Ваши любимые уроки")]
        [Required(ErrorMessage = "Ваши любимые уроки")]
        public string FavoriteLessons { get; set; }
        public string DateTime { get; set; }
        public int Id { get; set; }
    }
}