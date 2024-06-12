using System;
using System.Collections.Generic;
using System.Linq;

public static class DictionaryDiff
{
    public static void FindDiff<TKey, TValue>(Dictionary<TKey, TValue> original, Dictionary<TKey, TValue> modified, out Dictionary<TKey, TValue> added, out Dictionary<TKey, TValue> removed)
    {
        added = modified.Where(entry => !original.ContainsKey(entry.Key)).ToDictionary(entry => entry.Key, entry => entry.Value);
        removed = original.Where(entry => !modified.ContainsKey(entry.Key)).ToDictionary(entry => entry.Key, entry => entry.Value);
    }
}