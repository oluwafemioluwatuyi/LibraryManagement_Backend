using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using LibraryManagement.DTOs.SearchParams;

namespace LibraryManagement.ModelBinders;

public class SearchParamsModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var query = bindingContext.HttpContext.Request.Query;

        var searchTerm = query.TryGetValue("searchTerm", out var term)
            ? term.ToString()
            : string.Empty;

        var pageNumber = int.TryParse(query["pageNumber"], out var pn) && pn > 0 ? pn : 1;
        var pageSize = int.TryParse(query["pageSize"], out var ps) && ps > 0 ? Math.Min(ps, 50) : 10;

        var result = new SearchParams
        {
            SearchTerm = searchTerm.Trim(),
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        bindingContext.Result = ModelBindingResult.Success(result);
        return Task.CompletedTask;
    }
}
