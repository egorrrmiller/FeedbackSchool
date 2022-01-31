using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace FeedbackSchool.Models;

public class FeedbackModel
{
    [Required(ErrorMessage = "Заполните поле для удаления отзывов!")]
    [NotMapped]
    public string FeedbackNumber { get; set; }

    [AllowNull] public string School { get; set; }

    [AllowNull] public string Class { get; set; }

    [Key] public string Id { get; set; }
}