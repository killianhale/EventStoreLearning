﻿using System;
using Newtonsoft.Json;

namespace EventStoreLearning.EventSourcing.EventStore.Deserializers
{
    public static class JsonEventDeserializer<T> where T : IEvent
    {
        public static T Deserialize(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings()
            {
                MaxDepth = 2,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                DateParseHandling = DateParseHandling.DateTime,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                DateFormatString = "YYYY-MM-DDTHH:mm:ss.sssZ"
            });
        }
    }
}
