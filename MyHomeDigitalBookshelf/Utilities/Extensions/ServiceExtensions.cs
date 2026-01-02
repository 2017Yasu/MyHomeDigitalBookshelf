using System;
using Microsoft.Extensions.DependencyInjection;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Utilities.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddAttributedServices(this IServiceCollection services, params Type[] implementationTypes)
    {
        foreach (var type in implementationTypes)
        {
            var serviceTypes = type.GetInterfaces().
                Where(t => t.Namespace != null && t.Namespace.StartsWith("MyHomeDigitalBookshelf")).
                ToArray();
            if (serviceTypes.Length == 0)
            {
                services.AddAttributedService(type);
                continue;
            }
            foreach (var serviceType in serviceTypes)
            {
                services.AddAttributedService(serviceType, type);
            }
        }

        return services;
    }

    public static IServiceCollection AddAttributedService(this IServiceCollection services, Type implementationType)
    {
        if (implementationType.GetCustomAttributes(typeof(SingletonServiceAttribute), false).FirstOrDefault() is SingletonServiceAttribute singletonAttr)
        {
            if (string.IsNullOrWhiteSpace(singletonAttr.Key))
            {
                services.AddSingleton(implementationType);
            }
            else
            {
                throw new InvalidOperationException("Keyed singleton services are required to implement any interface.");
            }
        }

        if (implementationType.GetCustomAttributes(typeof(ScopedServiceAttribute), false).FirstOrDefault() is ScopedServiceAttribute scopedAttr)
        {
            if (string.IsNullOrWhiteSpace(scopedAttr.Key))
            {
                services.AddScoped(implementationType);
            }
            else
            {
                services.AddKeyedScoped(implementationType, scopedAttr.Key);
            }
        }

        if (implementationType.GetCustomAttributes(typeof(TransientServiceAttribute), false).FirstOrDefault() is TransientServiceAttribute transientAttr)
        {
            if (string.IsNullOrWhiteSpace(transientAttr.Key))
            {
                services.AddTransient(implementationType);
            }
            else
            {
                services.AddKeyedTransient(implementationType, transientAttr.Key);
            }
        }

        return services;
    }

    public static IServiceCollection AddAttributedService(this IServiceCollection services, Type serviceType, Type implementationType)
    {
        if (implementationType.GetCustomAttributes(typeof(SingletonServiceAttribute), false).FirstOrDefault() is SingletonServiceAttribute singletonAttr)
        {
            if (string.IsNullOrWhiteSpace(singletonAttr.Key))
            {
                services.AddSingleton(serviceType, implementationType);
            }
            else
            {
                services.AddKeyedSingleton(serviceType, singletonAttr.Key, implementationType);
            }
        }

        if (implementationType.GetCustomAttributes(typeof(ScopedServiceAttribute), false).FirstOrDefault() is ScopedServiceAttribute scopedAttr)
        {
            if (string.IsNullOrWhiteSpace(scopedAttr.Key))
            {
                services.AddScoped(serviceType, implementationType);
            }
            else
            {
                services.AddKeyedScoped(serviceType, scopedAttr.Key, implementationType);
            }
        }

        if (implementationType.GetCustomAttributes(typeof(TransientServiceAttribute), false).FirstOrDefault() is TransientServiceAttribute transientAttr)
        {
            if (string.IsNullOrWhiteSpace(transientAttr.Key))
            {
                services.AddTransient(serviceType, implementationType);
            }
            else
            {
                services.AddKeyedTransient(serviceType, transientAttr.Key, implementationType);
            }
        }

        return services;
    }
}
