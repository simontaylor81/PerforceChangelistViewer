using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerforceChangelistViewer.Util
{
	public static class Extensions
	{
		// Get the value in a dictionary for a given key, or if not found, return a fallback.
		public static TValue GetValueOrFallback<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue fallback)
		{
			TValue result;
			if (dict.TryGetValue(key, out result))
			{
				return result;
			}
			return fallback;
		}

		// Get the value in a dictionary for a given key, or if not found,
		// use the given delegate to return a fallback.
		public static TValue GetValueOrFallbackDelegate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> fallback)
		{
			TValue result;
			if (dict.TryGetValue(key, out result))
			{
				return result;
			}
			return fallback();
		}

		// Add an entry to a dictionary, and return the value added.
		public static TValue AddAndReturnValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
		{
			dict.Add(key, value);
			return value;
		}

		// Get a value in a dictionary for a given key, or if it's not found, use the delegate
		// to create a new value and add it, returning the new value.
		public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> newValueFunc)
		{
			TValue result;
			if (!dict.TryGetValue(key, out result))
			{
				result = newValueFunc();
				dict.Add(key, result);
			}
			return result;
		}
	}
}
