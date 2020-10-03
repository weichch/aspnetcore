using System;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Diagnostics.HealthChecks.DependencyInjection
{
    internal class HealthCheckServiceOptionsValidation : IValidateOptions<HealthCheckServiceOptions>
    {
        public ValidateOptionsResult Validate(string name, HealthCheckServiceOptions options)
        {
            // Scan the list for duplicate names to provide a better error if there are duplicates.
            var duplicateNames = options.Registrations
                .GroupBy(c => c.Name, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateNames.Count > 0)
            {
                return ValidateOptionsResult.Fail($"Duplicate health checks were registered with the name(s): {string.Join(", ", duplicateNames)}");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
