using System;
using Newtonsoft.Json;

namespace EventStoreLearning.Common.EventSourcing
{
    public static class JsonEventDeserializer<T> where T : Event
    {
        public static T Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
