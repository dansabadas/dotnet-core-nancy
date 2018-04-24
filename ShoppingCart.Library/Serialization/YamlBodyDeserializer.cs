using Nancy.ModelBinding;
using Nancy.Responses.Negotiation;
using System.IO;
using YamlDotNet.Serialization;

namespace ShoppingCart.Library.Serialization
{
  public class YamlBodyDeserializer : IBodyDeserializer
  {
    public bool CanDeserialize(MediaRange mediaRange, BindingContext context)
      => mediaRange.Subtype.ToString().EndsWith("yaml");

    public object Deserialize(MediaRange mediaRange, Stream bodyStream, BindingContext context)
    {
      var yamlDeserializer = new Deserializer();
      var reader = new StreamReader(bodyStream);
      return yamlDeserializer.Deserialize(reader, context.DestinationType);
    }
  }
}
