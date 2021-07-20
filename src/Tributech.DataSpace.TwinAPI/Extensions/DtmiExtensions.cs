using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Tributech.DataSpace.TwinAPI.Extensions {

	/// <summary>
	/// Extensions for handling of digital twin model identifiers (DTMI).
	/// </summary>
	public static class DtmiExtensions {
		// This code is under MIT licence, copied from https://github.com/Azure/azure-sdk-for-net/blob/main/sdk/modelsrepository/Azure.IoT.ModelsRepository/src/DtmiConventions.cs#L23
		// A DTMI has three components: scheme, path, and version.
		// Scheme and path are separated by a colon. Path and version are separated by a semicolon i.e. <scheme> : <path> ; <version>.
		// The scheme is the string literal "dtmi" in lowercase. The path is a sequence of one or more segments, separated by colons.
		// The version is a sequence of one or more digits. Each path segment is a non-empty string containing only letters, digits, and underscores.
		// The first character may not be a digit, and the last character may not be an underscore.
		// The version length is limited to nine digits, because the number 999,999,999 fits in a 32-bit signed integer value.
		// The first digit may not be zero, so there is no ambiguity regarding whether version 1 matches version 01 since the latter is invalid.
		private static readonly Regex s_validDtmiRegex = new Regex(@"^dtmi:(?<path>[A-Za-z](?:[A-Za-z0-9_]*[A-Za-z0-9])?(?::[A-Za-z](?:[A-Za-z0-9_]*[A-Za-z0-9])?)*);(?<version>[1-9][0-9]{0,8})$");

		/// <summary>
		/// Indicates whether a given digital twin model identifier (DTMI) value is well-formed.
		/// </summary>
		public static bool IsValidDtmi(this string dtmi) => IsValidDtmi(dtmi, out Match _);

		private static bool IsValidDtmi(this string dtmi, out Match regexMatch) {
			if (!string.IsNullOrEmpty(dtmi) && s_validDtmiRegex.Match(dtmi) is Match m && m.Success) {
				regexMatch = m;
				return true;
			}
			regexMatch = null;
			return false;
		}

		/// <summary>
		/// Convert digital twin model identifier (DTMI) to Neo4J label.
		/// </summary>
		/// <param name="dtmi">The digital twin model identifier (DTMI).</param>
		/// <returns></returns>
		public static string ToLabel(this string dtmi) {
			if (!IsValidDtmi(dtmi, out Match regexMatch)) {
				throw new ArgumentException($"Invalid DTMI '{dtmi}'.", nameof(dtmi));
			}

			string path = regexMatch.Groups["path"].Value;
			string reversedPascalCasePath = string.Join("", path.Split(":").Reverse().Select(p => p.Substring(0, 1).ToUpper() + p.Substring(1)));
			string version = regexMatch.Groups["version"].Value;

			return $"{reversedPascalCasePath}V{version}";
		}
	}
}
