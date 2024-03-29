﻿using System.Reflection;
using System.Text;
using Newtonsoft.Json;
//using System.Text.Json;

namespace DungeonsOfDoomBlazor.Helpers
{
    public static class JsonSerializationHelper
    {
        public static IList<T> DeserializeResourceStream<T>(string resourceNamespace)
        {
            try
            {
                var assembly = typeof(T).GetTypeInfo().Assembly;
                var resourceStream = assembly.GetManifestResourceStream(resourceNamespace);
                //StreamReader reader;
                using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
                {
                    var json = reader.ReadToEnd();
                    var elements = JsonConvert.DeserializeObject<List<T>>(json);
                    //var elements = JsonSerializer.Deserialize<IList<T>>(json);
                    return elements;
                }
            }
            catch (JsonException)
            {
                throw;
            }
            catch(Exception ex) 
            {
                throw new InvalidOperationException
                    ($"Error trying to load embedded resource for: {resourceNamespace}.", ex);
            }
        }
    }
}
