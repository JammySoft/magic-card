using Autofac;
using MagicCard.Library.Interfaces;

namespace MagicCard.Library.IoC
{
    /// <summary>
    /// Provides initial access to MagicCard interfaces.
    /// </summary>
    public class DefaultModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Requests for ICardRespository will create a new instance of CardRepository.
            builder.RegisterType<CardRepository>().As<ICardRepository>();
        }
    }
}