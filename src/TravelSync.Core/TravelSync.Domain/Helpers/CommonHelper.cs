using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TravelSync.Domain.Helpers;

public static class CommonHelper
{
    /// <summary>
    /// Convert object sang string.
    /// </summary>
    /// <param name="obj">Object cần chuyển đổi</param>
    /// <param name="camelCase">[Option, default=false]: Chuyển đổi dưới dạng camelCase.</param>
    /// <param name="indented">[Option, default=false]: Có thụt dòng hay không.</param>
    /// <returns>Json string.</returns>
    public static string ToJsonString(this object obj, bool camelCase = false, bool indented = false)
    {
        if (obj is null) return string.Empty;

        if (!camelCase && !indented)
            return JsonConvert.SerializeObject(obj);

        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = camelCase ? new CamelCasePropertyNamesContractResolver() : null,
            Formatting = indented ? Formatting.Indented : Formatting.None,
        };

        return JsonConvert.SerializeObject(obj, settings);
    }
}
