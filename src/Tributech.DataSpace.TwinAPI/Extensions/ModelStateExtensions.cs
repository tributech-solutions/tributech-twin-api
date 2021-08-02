using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Tributech.DataSpace.TwinAPI.Extensions {

	/// <summary>
	/// Extensions for <see cref="ModelStateDictionary"/>.
	/// </summary>
	public static class ModelStateExtensions {

		/// <summary>
		/// Add dictionary of errors to model state.
		/// </summary>
		/// <param name="modelState">The model state.</param>
		/// <param name="errors">The dictionary of model errors with property name as key.</param>
		/// <param name="keyPrefix">(Optional) prefix for model error key.</param>
		public static void AddModelErrors(this ModelStateDictionary modelState, IDictionary<string, string[]> errors, string keyPrefix = "") {
			foreach (KeyValuePair<string, string[]> error in errors) {
				foreach (string errorMessage in error.Value) {
					modelState.AddModelError($"{keyPrefix}{error.Key}", errorMessage);
				}
			}
		}
	}
}
