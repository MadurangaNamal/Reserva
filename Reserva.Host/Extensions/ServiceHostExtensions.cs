using CoreWCF.Configuration;
using CoreWCF.Description;

namespace Reserva.Host.Extensions;

public static class ServiceHostExtensions
{
    public static void EnableDebugBehavior<TService>(
        this IServiceBuilder serviceBuilder,
        bool includeExceptionDetails) where TService : class
    {
        serviceBuilder.ConfigureServiceHostBase<TService>(host =>
        {
            var debugBehavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();

            if (debugBehavior == null)
            {
                debugBehavior = new ServiceDebugBehavior();
                host.Description.Behaviors.Add(debugBehavior);
            }

            debugBehavior.IncludeExceptionDetailInFaults = includeExceptionDetails;
        });
    }
}
