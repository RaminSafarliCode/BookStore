using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Application.AppCode.Extenstions
{
    public static partial class Extension
    {
        public static List<ValidationError> GetErrors(this ModelStateDictionary modelState)
        {
            var errors = (from acar in modelState.Keys
                          where modelState[acar] != null && modelState[acar].Errors.Count > 0
                          select new ValidationError(acar, modelState[acar].Errors[0].ErrorMessage)
                          ).ToList();

            return errors;
        }
    }

    public class ValidationError
    {
        public string FieldName { get; set; }
        public string Message { get; set; }

        public ValidationError(string fieldName, string message)
        {
            this.FieldName = fieldName;
            this.Message = message;
        }
    }
}
