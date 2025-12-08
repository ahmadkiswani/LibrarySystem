using LibrarySystem.Shared.DTOs;
using LibrarySystem.Shared.DTOs.Helper;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Helpers
{
    public static class ValidationHelper
    {
        public static ValidationResultDto ValidateDto(ControllerBase controller, object dto)
        {
            var result = new ValidationResultDto();

            if (dto == null)
            {
                result.IsValid = false;
                result.Errors.Add("Request body is required.");
                return result;
            }

            if (!controller.ModelState.IsValid)
            {
                result.IsValid = false;

                foreach (var kvp in controller.ModelState)
                {
                    var fieldName = kvp.Key;
                    var errors = kvp.Value.Errors;

                    foreach (var error in errors)
                    {
                        var message = string.IsNullOrWhiteSpace(fieldName)
                            ? error.ErrorMessage
                            : $"{fieldName}: {error.ErrorMessage}";

                        result.Errors.Add(message);
                    }
                }

                return result;
            }

            result.IsValid = true;
            return result;
        }
    }
}
