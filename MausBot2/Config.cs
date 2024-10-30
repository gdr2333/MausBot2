using System.Collections.Concurrent;
using System.Text.Json.Serialization;

namespace MausBot2;
public class Config
{
    [JsonInclude]
    public Uri Uri { get; set; }

    [JsonInclude]
    public ConcurrentDictionary<string, object> sharedStorage { get; set; }

    [JsonInclude]
    public ConcurrentDictionary<string, ConcurrentDictionary<string, object>> localStorage { get; set; }
}