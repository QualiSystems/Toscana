using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Toscana.Engine
{
    internal interface ICloudServiceArchiveValidator
    {
        bool ValidateCloudServiceArchive(ToscaCloudServiceArchive toscaCloudServiceArchive, out List<ValidationResult> validationResults);
    }

    internal class CloudServiceArchiveValidator : ICloudServiceArchiveValidator
    {
        private readonly IToscaValidator<ToscaCloudServiceArchive> cloudServiceValidator;

        public CloudServiceArchiveValidator(IToscaValidator<ToscaCloudServiceArchive> cloudServiceValidator)
        {
            this.cloudServiceValidator = cloudServiceValidator;
        }

        public bool ValidateCloudServiceArchive(ToscaCloudServiceArchive toscaCloudServiceArchive, out List<ValidationResult> validationResults)
        {
            return cloudServiceValidator.TryValidateRecursively(toscaCloudServiceArchive, out validationResults);
        }
    }
}