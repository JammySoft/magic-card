using Autofac;
using MagicCard.Library.Interfaces;

namespace MagicCard.Library.IoC
{
  /// <summary>
  /// AutoFac module - Allows access to interfaces and classes without making implementation public.
  /// </summary>
  public class DefaultModule : Module
  {
    protected override void Load(ContainerBuilder builder)
    {
      builder.RegisterType<CardRepository>().As<ICardRepository>();
    }
  }
}
