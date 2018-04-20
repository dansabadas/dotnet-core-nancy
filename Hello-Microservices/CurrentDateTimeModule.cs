using System;
using Nancy;

namespace Hello_Microservices
{
  public class CurrentDateTimeModule : NancyModule
  {
    public CurrentDateTimeModule()
    {
      Get("/", _ => DateTime.UtcNow);
    }
  }
}
