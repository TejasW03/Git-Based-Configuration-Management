using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

public class Cluster
{
    public string Name { get; set; }
    public string Region { get; set; }
    public int Nodes { get; set; }
    
    public string ToYaml()
    {
        var serializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return serializer.Serialize(this);
    }

    public static Cluster FromYaml(string yaml)
    {
        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
        return deserializer.Deserialize<Cluster>(yaml);
    }
}
