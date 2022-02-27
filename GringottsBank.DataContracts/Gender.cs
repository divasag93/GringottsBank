using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace GringottsBank.DataContracts
{
    [Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Gender
    {
        Male, Female, Others, Unknown
    }
}
