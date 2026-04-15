using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Filters
{
    public class ValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if(context.ModelState.IsValid == false)
            {
                var errorDetails = new List<ErrorDetail>();
                foreach (var modelState in context.ModelState)
                {
                    string fieldName = modelState.Key;
                    var errors = modelState.Value.Errors;

                    foreach (var error in errors)
                    {
                        var errorDetail = new ErrorDetail
                        {
                            Field = fieldName,
                            Message = error.ErrorMessage,
                            Code = "ValidationError"
                        };
                        errorDetails.Add(errorDetail);
                    }
                }
                var apiResponse = new ApiResponse(400, "Validation Failed", errorDetails);
                context.Result = new BadRequestObjectResult(apiResponse);
            }
            base.OnActionExecuting(context);
        }
    }
}
