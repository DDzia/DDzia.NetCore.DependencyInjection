using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DDzia.NetCore.DependencyInjection
{
    /// <summary>
    /// Pack of extensions to <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add registrations of module.
        /// </summary>
        /// <typeparam name="T">Module type.</typeparam>
        /// <param name="services">Service collection.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddModule<T>(this IServiceCollection services)
            where T: IIoCModule, new()
        {
            new T().Register(services);
            return services;
        }

        /// <summary>
        /// Find all available modules of <paramref name="assembly"/> and add registrations of their.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="assembly">Assembly of modules.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddModulesFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var moduleTypes = assembly.GetTypes()
                .Where(x => x.IsClass &&
                            !x.IsAbstract &&
                            !x.IsGenericType &&
                            x.IsSubclassOf(typeof(IIoCModule))
                            && (
                                !x.GetConstructors().Any() && // has no ctors
                                x.GetConstructors().Any(ctor => ctor.IsPublic && !ctor.GetParameters().Any()) // or public ctor without parameters
                            ));

            foreach (var moduleType in moduleTypes)
            {
                var moduleInstance = (IIoCModule)Activator.CreateInstance(moduleType);
                moduleInstance.Register(services);
            }

            return services;
        }

        /// <summary>
        /// Find all available modules of assembly where <typeparamref name="T"/> is declared and add registrations of their.
        /// </summary>
        /// <typeparam name="T">Type where from will used assembly.</typeparam>
        /// <param name="services">Service collection.</param>
        /// <returns>Service collection.</returns>
        public static IServiceCollection AddModulesFromAssembly<T>(this IServiceCollection services)
        {
            return AddModulesFromAssembly(services, typeof(T).Assembly);
        }
    }
}
