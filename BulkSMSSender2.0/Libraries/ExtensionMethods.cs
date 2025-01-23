using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.Text;

public static class ExtensionMethods
{
    public static bool IsNullOrEmptyString(this string[] array)
    {
        if (array == null || array.Length == 0 || array[0] == "") return true;
        else return false;
    }

    public static bool IsNullOrEmpty(this StringBuilder builder)
    {
        return builder == null || builder.Length == 0;
    }

    public static bool IsNullOrEmpty<T>(this T[] array)
    {
        if (array == null || array.Length == 0) return true;
        else return false;
    }

    public static bool IsNullOrEmpty<T>(this List<T> list)
    {
        if (list == null || list.Count == 0 || list.Capacity == 0) return true;
        else return false;
    }
    public static bool IsNullOrEmpty<T>(this HashSet<T> hashSet)
    {
        if (hashSet == null || hashSet.Count == 0) return true;
        else return false;
    }

    public static void OpenInExplorer(this string fileWithPath)
    {
        if (fileWithPath != null && File.Exists(fileWithPath))
        {
            Process.Start(fileWithPath);
        }
    }

    public static int ToIntMilimeters(this float floatToRound)
    {
        floatToRound = floatToRound * 1000f;
        return (int)floatToRound;
    }

    public static void AddIfNotContains<T>(this List<T> list, T t)
    {
        if (!list.Contains(t))
        {
            list.Add(t);
        }
    }

    public static string ReplaceAt(this string input, int startIndex, int endIndex, string insert)
    {
        int length = (endIndex - startIndex) + 1;

        return input.Remove(startIndex, length).Insert(startIndex, insert);
    }

    public static StringBuilder ReplaceAt(this StringBuilder input, int startIndex, int endIndex, string insert)
    {
        int length = (endIndex - startIndex) + 1;

        return input.Remove(startIndex, length).Insert(startIndex, insert);
    }

    public static StringBuilder ReplaceAt(this StringBuilder input, int startIndex, int endIndex, StringBuilder insert)
    {
        int length = (endIndex - startIndex) + 1;

        return input.Remove(startIndex, length).Insert(startIndex, insert);
    }

    public static string RemoveAllWhitespaces(this string input)
    {
        return string.Join("", input.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
    }

    public static void ResizeWithJoin<T>(this T[] array, T[] secondArray)
    {
        int lengthBuffer = array.Length;
        Array.Resize(ref array, array.Length + secondArray.Length);
        secondArray.CopyTo(array, lengthBuffer);
    }
    public static void Add<T>(this T[] array, T element)
    {
        Array.Resize(ref array, array.Length + 1);
        array[^1] = element;
    }

    public static T[] Remove<T>(this T[] array, int index)
    {
        if (index >= 0 && index < array.Length)
        {
            return array.Where((a, i) => i != index).ToArray();
        }
        else
        {
            throw new ArgumentOutOfRangeException("Nieprawidłowy index");
        }
    }

    /// <summary>
    /// returns true if replace is made
    /// </summary>
    /// <returns></returns>
    public static bool AddIfNotContainsOrReplace<K, V>(this Dictionary<K, V> dictionary, K keyBefore, K keyAfter, V value)
    {
        bool result = dictionary.Remove(keyBefore);
        dictionary.Add(keyAfter, value);
        return result;
    }

    public static bool Replace<K, V>(this Dictionary<K, V> dictionary, K keyBefore, K keyAfter, V value)
    {
        if (dictionary.Remove(keyBefore))
        {
            dictionary.Add(keyAfter, value);
            return true;
        }
        else { return false; }
    }
    public static bool RemoveByValueSlow<K, V>(this Dictionary<K, V> dictionary, V value)
    {
        return dictionary.Remove(dictionary.FirstOrDefault(x => x.Value.Equals(value)).Key);
    }

    public static T Clone<T>(this T source)
    {
        var serialized = JsonConvert.SerializeObject(source);
        return JsonConvert.DeserializeObject<T>(serialized);
    }

    public static bool GetBit<T>(this T value, int index) where T : struct, IConvertible
    {
        int comparableValue = Convert.ToInt32(value);
        return (comparableValue & (1 << index)) != 0;
    }
    public static bool GetBit(this long value, int index)
    {
        return (value & (1L << index)) != 0;
    }

    public static int SetBit<T>(this T value, int index, bool state) where T : struct, IConvertible
    {
        int comparableValue = Convert.ToInt32(value);
        int mask = 1 << index;

        return state ? (comparableValue | mask) : (comparableValue & ~mask);
    }
    public static long SetBit(this long value, int index, bool state)
    {
        long mask = 1L << index;
        return state ? (value | mask) : (value & ~mask);
    }

    public static string ToStringPretty(this float value, string formatInteger = "F0", string formatDecimal = "F2")
    {
        float valueModulus = value % 1f;
        return valueModulus > 0.999f || valueModulus < 0.001f ? value.ToString(formatInteger) : value.ToString(formatDecimal);
    }

    public static float ParseFastF(this string input)
    {
        if (input.Contains("e") || input.Contains("E"))
            return float.Parse(input, CultureInfo.InvariantCulture);

        float result = 0;
        int pos = 0;
        int len = input.Length;

        if (len == 0) return float.NaN;
        char c = input[0];
        float sign = 1;
        if (c == '-')
        {
            sign = -1;
            ++pos;
            if (pos >= len) return float.NaN;
        }

        while (true) // breaks inside on pos >= len or non-digit character
        {
            if (pos >= len) return sign * result;
            c = input[pos++];
            if (c < '0' || c > '9') break;
            result = (result * 10.0f) + (c - '0');
        }

        if (c != '.' && c != ',') return float.NaN;
        float exp = 0.1f;
        while (pos < len)
        {
            c = input[pos++];
            if (c < '0' || c > '9') return float.NaN;
            result += (c - '0') * exp;
            exp *= 0.1f;
        }
        return sign * result;
    }
    public static int ParseFastI(this string input)
    {
        int result = 0;
        bool isNegative = (input[0] == '-');

        for (int i = (isNegative) ? 1 : 0; i < input.Length; i++)
            result = result * 10 + (input[i] - '0');
        return (isNegative) ? -result : result;
    }

    /// <summary>
    /// adds string key with iteration at end if key is already used and returns proper key
    /// </summary>
    public static bool TryAddWithIteration<T>(this Dictionary<string, T> dictionary, string key, T t, out string keyFinal, out int iterator)
    {
        if (dictionary.TryAdd(key, t)) { keyFinal = key; iterator = -1; return true; }
        else
        {
            for (int i = 0; i < int.MaxValue; i++)
            {
                keyFinal = key + i;
                if (dictionary.TryAdd(keyFinal, t)) { iterator = i; return true; }
            }

            keyFinal = null;
            iterator = -1;
            return false;
        }
    }
    public static bool TryAddWithIteration<T>(this Dictionary<int, T> dictionary, int key, T t, out int keyFinal)
    {
        if (dictionary.TryAdd(key, t)) { keyFinal = key; return true; }
        else
        {
            int amount;

            if (key > 0)
                amount = -1;
            else
                amount = 1;

            for (int i = key; i < int.MaxValue && i > int.MinValue; i += amount)
            {
                if (dictionary.TryAdd(i, t)) { keyFinal = i; return true; }
            }

            keyFinal = 0;
            return false;
        }
    }
}