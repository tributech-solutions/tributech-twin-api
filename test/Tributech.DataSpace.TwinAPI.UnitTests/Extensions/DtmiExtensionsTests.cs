using System;
using Tributech.DataSpace.TwinAPI.Extensions;
using Xunit;

namespace Tributech.DataSpace.TwinAPI.UnitTests {

	/// <summary>
	/// Tests for <see cref="DtmiExtensions"/>.
	/// </summary>
	public class DtmiExtensionsTests {

		[InlineData("dtmi:com:example:Thermostat;1", true)]
		[InlineData("dtmi:contoso:scope:entity;2", true)]
		[InlineData("dtmi:com:example:Thermostat:1", false)]
		[InlineData("dtmi:com:example::Thermostat;1", false)]
		[InlineData("dtmi:com:example::Thermostat;1234567890", false)]
		[InlineData("dtmi:com:example:Thermostat;", false)]
		[InlineData("dtmi:com:example:Thermostat", false)]
		[InlineData("dtmi:;", false)]
		[InlineData("com:example:Thermostat;1", false)]
		[InlineData("", false)]
		[InlineData(null, false)]
		[Theory]
		public void IsValidDtmi(string dtmi, bool expected) {
			// ACT
			bool actual = dtmi.IsValidDtmi();

			// ASSERT
			Assert.Equal(expected, actual);
		}

		[InlineData("dtmi:io:tributech:device:edge;1", "EdgeDeviceTributechIoV1")]
		[InlineData("dtmi:io:tributech:source:simulated;1", "SimulatedSourceTributechIoV1")]
		[InlineData("dtmi:io:tributech:source:simulated;123456789", "SimulatedSourceTributechIoV123456789")]
		[Theory]
		public void ToLabel_ShouldWork(string dtmi, string expected) {
			// ACT
			string actual = dtmi.ToLabel();

			// ASSERT
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void ToLabel_ShouldThrow() {
			const string invalidDtmi = "dmti:;";

			// ACT && ASSERT
			ArgumentException ex = Assert.Throws<ArgumentException>(() => invalidDtmi.ToLabel());
			Assert.Equal($"Invalid DTMI '{invalidDtmi}'. (Parameter 'dtmi')", ex.Message);
		}
	}
}
