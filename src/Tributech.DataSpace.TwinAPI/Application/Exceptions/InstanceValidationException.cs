using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Tributech.DataSpace.TwinAPI.Application.Exceptions {
	public class InstanceValidationException : Exception {
		public const string DefaultErrorMessage = "One or more validation errors occurred.";
		public InstanceValidationException() {
			Errors = new Dictionary<string, string[]>();
		}

		public InstanceValidationException(IDictionary<string, string[]> errors, string message = DefaultErrorMessage) : base(message) {
			Errors = errors;
		}

		public InstanceValidationException(string message, Exception innerException) : base(message, innerException) {
			Errors = new Dictionary<string, string[]>();
		}

		protected InstanceValidationException(SerializationInfo info, StreamingContext context) : base(info, context) {
			Errors = new Dictionary<string, string[]>();
		}

		public IDictionary<string, string[]> Errors { get; }
	}
}
