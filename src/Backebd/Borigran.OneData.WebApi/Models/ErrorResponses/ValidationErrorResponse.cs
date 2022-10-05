using System.Collections.Generic;

namespace Borigran.OneData.WebApi.Models.ErrorResponses
{
    public class ValidationErrorResponseItem
    {
        public string PropertyName { get; set; }

        public string Message { get; set; }
    }

    public class ValidationErrorResponse
    {
        public IEnumerable<ValidationErrorResponseItem> ValidatioErrors { get; set; }
    }
}
