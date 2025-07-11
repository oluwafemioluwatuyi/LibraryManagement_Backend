using System;
using LibraryManagement.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.DTOs.SearchParams;

[ModelBinder(BinderType = typeof(SearchParamsModelBinder))]
public class SearchParams
{
    public string SearchTerm { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
}
