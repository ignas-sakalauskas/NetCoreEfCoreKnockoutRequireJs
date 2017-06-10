using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApplication1.Web.Helpers
{
    /// <summary>
    /// Format helpers
    /// </summary>
    public static class FormatHelpers
    {
        /// <summary>
        /// Formats return string with error messages when Model is invalid
        /// </summary>
        /// <param name="modelStateValues">Invalid model state values</param>
        /// <returns>Formatted error message as string</returns>
        public static string FormatValidationErrorMessage(IEnumerable<ModelStateEntry> modelStateValues)
        {
            if (modelStateValues == null)
            {
                throw new ArgumentNullException(nameof(modelStateValues));
            }

            var sb = new StringBuilder();
            sb.AppendLine("Invalid model. Errors:");

            foreach (var value in modelStateValues)
            {
                foreach (var error in value.Errors)
                {
                    sb.AppendLine(error.ErrorMessage);
                }
            }

            return sb.ToString();
        }
    }
}
