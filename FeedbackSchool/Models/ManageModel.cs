using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FeedbackSchool.Models;

public class ManageModel
{
    [AllowNull] public string School { get; set; }

    [AllowNull] public string Class { get; set; }

    [Key] public string Id { get; set; }
}