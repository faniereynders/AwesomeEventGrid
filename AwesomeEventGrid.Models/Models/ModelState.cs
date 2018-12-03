using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AwesomeEventGrid.Abstractions.Models
{
    public class ModelState: IReadOnlyDictionary<string, string[]>
    {
        private readonly Dictionary<string, string[]> errors;
        public ModelState()
        {
            errors = new Dictionary<string, string[]>();
        }
        public string[] this[string key] => errors[key];

        public bool IsValid => !errors.Any();

        public IEnumerable<string> Keys => errors.Keys;

        public IEnumerable<string[]> Values => errors.Values;

        public int Count => errors.Count;

        public void AddError(string key, string error)
        {
            if (errors.ContainsKey(key))
            {
                var keyErrors = errors[key].ToList();
                keyErrors.Add(error);
                errors[key] = keyErrors.ToArray();
            }
            else
            {
                errors.Add(key, new string[] { error });
            }
        }

        public void Reset()
        {
            errors.Clear();
        }

        public bool ContainsKey(string key)
        {
            return errors.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<string, string[]>> GetEnumerator()
        {
            return errors.GetEnumerator();
        }

        public bool TryGetValue(string key, out string[] value)
        {
            return errors.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return errors.GetEnumerator();
        }
    }
}
