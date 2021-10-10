using System.ComponentModel.DataAnnotations;

namespace FeedbackSchool.Models
{
    public class Admin
    {
        [Required(ErrorMessage = "Заполните поле для удаления отзывов!")]
        public string FeedbackNumber { get; set; }
    }
}