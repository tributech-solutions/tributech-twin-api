using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Tributech.DataSpace.TwinAPI.Model;
using Tributech.Dsk.Api.Clients.CatalogApi;

namespace Tributech.DataSpace.TwinAPI.Validators {
	public class TwinValidator : AbstractValidator<DigitalTwin> {
		private readonly CatalogAPIClient _catalogAPI;
		private readonly ILogger<TwinValidator> _logger;

		public TwinValidator(ILogger<TwinValidator> logger,
							CatalogAPIClient catalogAPI) {
			_catalogAPI = catalogAPI;
			_logger = logger;

			RuleFor(x => x.Metadata).NotNull().WithMessage("$metadata cant be empty");
			RuleFor(x => x.Metadata.ModelId)
				.NotEmpty()
				.WithMessage("$model cant be empty")
				.Must(dtmi => dtmi.Contains("dtmi:"))
				.WithMessage("Invalid $model needs to start with dtmi:");
			RuleFor(x => x).CustomAsync(ValidateAgainstSchema);
		}

		private async Task ValidateAgainstSchema(DigitalTwin twin, CustomContext context, CancellationToken cancelationToken) {
			JSONSchema4 _schema = await _catalogAPI.ValidateSchemaAsync(twin.Metadata.ModelId);

			JsonSchema schema = JsonSchema.Parse(JObject.FromObject(_schema).ToString());
			JObject person = JObject.FromObject(twin.ToExpandoObject());

			bool valid = person.IsValid(schema, out IList<string> errorMessages);

			_logger.LogDebug(@"Validate incoming twin with Id {0}", twin.Id);

			if (valid) {
				return;
			}

			foreach(string error in errorMessages) {
				context.AddFailure(error);
			}
		}
	}
}
