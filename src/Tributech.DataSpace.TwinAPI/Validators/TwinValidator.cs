using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Tributech.DataSpace.TwinAPI.Infrastructure.CatalogAPI;
using Tributech.DataSpace.TwinAPI.Model;

namespace Tributech.DataSpace.TwinAPI.Validators {
	public class TwinValidator : AbstractValidator<DigitalTwin> {
		private readonly CatalogAPIClient _catalogAPI;
		public TwinValidator(CatalogAPIClient catalogAPI) {
			_catalogAPI = catalogAPI;
			RuleFor(x => x.Metadata).NotNull().WithMessage("$metadata cant be empty");
			RuleFor(x => x.Metadata.ModelId)
				.NotEmpty()
				.WithMessage("$model cant be empty")
				.Must(dtmi => dtmi.Contains("dtmi:"))
				.WithMessage("Invalid $model needs to start with dtmi:");
			RuleFor(x => x).CustomAsync(ValidateAgainstSchema);
		}

		private async Task ValidateAgainstSchema(DigitalTwin twin, CustomContext context, CancellationToken cancelationToken) {
			SchemaValidationError validationResult = await _catalogAPI.ValidateAsync(twin.ToExpandoObject());

			if (validationResult.Success) {
				return;
			}

			foreach(SchemaErrorObject error in validationResult.Errors) {
				context.AddFailure(error.PropertyName, error.Message);
			}
		}
	}
}
