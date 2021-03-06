﻿using System.IO;
using Newtonsoft.Json;
using RestSharp.Serializers;
using RestSharp.Deserializers;

namespace SunriseClock.Serializer
{
    public class JsonSerializer : ISerializer, IDeserializer
    {
        private readonly Newtonsoft.Json.JsonSerializer _serializer;

        public JsonSerializer(Newtonsoft.Json.JsonSerializer serializer)
        {
            this._serializer = serializer;
        }

        public string ContentType
        {
            get { return "application/json"; }
            set {  } // per Interface.
        }
        public string DateFormat { get; set; }
        public string Namespace { get; set; }
        public string RootElement { get; set; }

        public string Serialize(object obj)
        {
            using (var stringWriter = new StringWriter())
            {
                using (var jsonTextWriter = new JsonTextWriter(stringWriter))
                {
                    _serializer.Serialize(jsonTextWriter, obj);

                    return stringWriter.ToString();
                }
            }
        }

        public T Deserialize<T>(RestSharp.IRestResponse response)
        {
            var content = response.Content;
            
            using (var stringReader = new StringReader(content))
            {
                using (var jsonTextReader = new JsonTextReader(stringReader))
                {
                    return _serializer.Deserialize<T>(jsonTextReader);
                }
            }
        }

        public static JsonSerializer Default => new JsonSerializer(new Newtonsoft.Json.JsonSerializer()
        {
            NullValueHandling = NullValueHandling.Ignore,
        });
    }
}
