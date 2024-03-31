using MKopa.Core.Entities.Providers;

namespace MKopa.Core.Services.Restful
{
    public interface ISmsSenderServiceFactory
    {
        ISmsSenderService GetSmsSenderService();
    }
}
