using System;

namespace shared.Extensions
{
    public static class DictionaryExtensions
    {
        public static void AddAfter<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey keyToFind, TKey newKey, TValue newValue)
        {
            // Create a new dictionary to hold the modified content
            var newDictionary = new Dictionary<TKey, TValue>();

            // Flag to indicate if the key to find has been found
            bool keyFound = false;

            // Iterate through the original dictionary
            foreach (var kvp in dictionary)
            {
                // Add the current key-value pair to the new dictionary
                newDictionary.Add(kvp.Key, kvp.Value);

                // If the current key matches the key to find, add the new key-value pair
                if (EqualityComparer<TKey>.Default.Equals(kvp.Key, keyToFind))
                {
                    newDictionary.Add(newKey, newValue);
                    keyFound = true;
                }
            }

            // If the key to find is not found, add the new key-value pair at the end
            if (!keyFound)
            {
                newDictionary.Add(newKey, newValue);
            }

            // Clear the original dictionary
            dictionary.Clear();

            // Copy the content of the new dictionary back to the original dictionary
            foreach (var kvp in newDictionary)
            {
                dictionary.Add(kvp.Key, kvp.Value);
            }
        }
    }
}
