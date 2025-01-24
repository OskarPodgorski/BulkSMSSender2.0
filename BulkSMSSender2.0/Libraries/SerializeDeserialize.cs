using Newtonsoft.Json;

public static class SerializeDeserialize
{
    public static T LoadPureDataFile<T>(string path) where T : class, new()
    {
        string json = string.Empty;

        if (File.Exists(path))
        {
            using (StreamReader reader = new(path))
            {
                json = reader.ReadToEnd();
            }

            T? deserialized = JsonConvert.DeserializeObject<T>(json);

            if (deserialized != null)
                return deserialized;
        }

        return new T();
    }

    public static void SavePureDataFile<T>(T pureData, string path) where T : class, new()
    {
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            throw new NotImplementedException();
        }
        else
        {
            string json = JsonConvert.SerializeObject(pureData, Formatting.Indented);

            if (string.IsNullOrEmpty(json) == false)
            {
                using (StreamWriter writer = new(path))
                {
                    writer.Write(json);
                }
            }
        }
    }

    public static async Task SavePureDataFileAsync<T>(T pureData, string path) where T : class, new()
    {
        if (!Directory.Exists(Path.GetDirectoryName(path)))
        {
            throw new NotImplementedException();
        }
        else
        {
            string json = JsonConvert.SerializeObject(pureData, Formatting.Indented);

            if (string.IsNullOrEmpty(json) == false)
            {
                using (StreamWriter writer = new(path))
                {
                    writer.Write(json);
                }
            }
        }
    }
}
