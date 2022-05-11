using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;
using Utilities;

namespace WebApi.Utils;

public class ValidationMetadataProvider : IValidationAttributeAdapterProvider, IValidationMetadataProvider
{
    private readonly ValidationAttributeAdapterProvider _originalProvider = new();

    public IAttributeAdapter? GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer? stringLocalizer)
    {
        var resourceCode = $"ErrorMessage.{attribute.GetType().Name}";

        var value = RS.GetValueOrDefault(resourceCode);

        if (value is not null)
            attribute.ErrorMessage = value;

        return _originalProvider.GetAttributeAdapter(attribute, stringLocalizer);
    }

    public void CreateValidationMetadata(ValidationMetadataProviderContext context)
    {
        var validators = context.ValidationMetadata.ValidatorMetadata;
        foreach (var validator in validators)
        {
            if (validator is not ValidationAttribute attribute)
                continue;

            var resourceCode = $"ErrorMessage.{attribute.GetType().Name}";

            var value = RS.GetValueOrDefault(resourceCode);

            if (value is not null)
                attribute.ErrorMessage = value;
        }
    }
}