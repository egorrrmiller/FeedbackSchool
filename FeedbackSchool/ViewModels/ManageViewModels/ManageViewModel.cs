using System.Collections.Generic;
using FeedbackSchool.Models;

namespace FeedbackSchool.ViewModels.ManageViewModels;

public class ManageViewModel
{
    public List<ClassModel> ClassModels { get; set; }

    public List<SchoolModel> SchoolModels { get; set; }
}