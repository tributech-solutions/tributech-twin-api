using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Tributech.DataSpace.TwinAPI.Model;
using FluentValidation;

namespace Tributech.DataSpace.TwinAPI.Validators {
	public static class RegisterValidators {
        public static IServiceCollection AddValidators(this IServiceCollection services, IConfiguration configuration) 
        {
			services.AddTransient<IValidator<DigitalTwin>, TwinValidator>();
			//services.AddTransient<IValidator<Relationship>, RelationshipValidator>();

			return services;
        }
    }
}
