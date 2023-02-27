using FluentValidation;
using UnitMgr.Domain.AggregatesModel.IdentityDTOs;

namespace UnitMgr.Admin.Pages.Identity.Roles;

public class RoleDtoValidator : AbstractValidator<RoleDto>
{
    public RoleDtoValidator()
    {
        RuleFor(v => v.Name)
             .MaximumLength(256)
             .NotEmpty();
    }
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<RoleDto>.CreateWithOptions((RoleDto)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };

}
