using Microsoft.Extensions.DependencyInjection;

namespace DDzia.NetCore.DependencyInjection
{
    /// <summary>
    /// Specify contract from IoC module.
    /// </summary>
    public interface IIoCModule
    {
        /// <summary>
        /// Add registrations declared in module.
        /// </summary>
        /// <param name="services">Service collection.</param>
        void Register(IServiceCollection services);
    }
}