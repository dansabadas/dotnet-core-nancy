using Serilog;
using Serilog.Events;

namespace ShoppingCart.Library
{
  public class ILoggerFactory
  {
    public static ILogger ConfigureLogger()
    {
      return new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.ColoredConsole(
           LogEventLevel.Verbose,
           "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({CorrelationToken}) {Message}{NewLine}{Exception}")
        .CreateLogger();
    }
  }
}
