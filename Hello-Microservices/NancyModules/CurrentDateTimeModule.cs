using System;
using Nancy;

namespace Hello_Microservices.NancyModules
{
  public class CurrentDateTimeModule : NancyModule
  {
    public CurrentDateTimeModule()
    {
      Get("/test", _ => DateTime.UtcNow);
    }
  }
}
