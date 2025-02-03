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
            throw new DirectoryNotFoundException($"Directory does not exist: {Path.GetDirectoryName(path)}");
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
            throw new DirectoryNotFoundException($"Directory does not exist: {Path.GetDirectoryName(path)}");
        }

        string json = await Task.Run(() => JsonConvert.SerializeObject(pureData, Formatting.Indented));

        if (!string.IsNullOrEmpty(json))
        {
            await using (StreamWriter writer = new(path, false))
            {
                await writer.WriteAsync(json);
            }
        }
    }

    /// <summary>
    /// using StreamWriter to read all content of file as string
    /// </summary>
    /// <returns></returns>
    public static string ReadFileContent(string fullPath)
    {
        if (File.Exists(fullPath))
        {
            string readed;

            using StreamReader reader = new(fullPath);
            {
                try
                {
                    readed = reader.ReadToEnd();
                }
                catch
                {
                    reader.Close();
                    reader.Dispose();
                    return string.Empty;
                }
            }

            return readed;
        }
        return string.Empty;
    }
}
