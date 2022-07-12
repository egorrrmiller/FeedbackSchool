using System.Collections.Generic;
using FeedbackSchool.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FeedbackSchool.ViewModels.IndexViewModels;

public class IndexViewModel
{
    public List<SelectListItem> SchoolListItems { get; set; }
    public List<SelectListItem> ClassListItems { get; set; }

    public FeedbackModel Feedback { get; set; }
}