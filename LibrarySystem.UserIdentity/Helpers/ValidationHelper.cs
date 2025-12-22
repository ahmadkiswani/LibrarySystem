using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.UserIdentity.Helpers
{
    public static class ValidationHelper
    {
        public static List<string> Validate(object dto)
        {
            var context = new ValidationContext(dto);
            var results = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(
                dto,
                context,
                results,
                validateAllProperties: true
            );

            if (!isValid)
            {
                return results
                    .Select(r => r.ErrorMessage ?? "Validation error")
                    .ToList();
            }

            return new List<string>();
        }
    }
}
