using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Exceptions
{
    public class ValidationException:ApplicationException
    {

        public Dictionary<string , string[]> Errors { get; } = new Dictionary<string, string[]>();
        public ValidationException():base("One or more validation failures have occurred.")
        {
            
        }

        public ValidationException(IEnumerable<ValidationFailure> failure):this()
        {
            Errors = failure
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}
