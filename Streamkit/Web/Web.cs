using System;
using System.Collections;
using System.Collections.Generic;

namespace Streamkit.Web {
    public class UrlParams : IDictionary<string, string> {
        private Dictionary<string, string> param
                = new Dictionary<string, string>();

        public string this[string key] {
            get { return this.param[key]; }
            set { this.param[key] = value; }
        }

        public ICollection<string> Keys {
            get { return this.param.Keys; }
        }

        public ICollection<string> Values {
            get { return this.param.Values; }
        }

        public int Count {
            get { return this.param.Count; }
        }

        public bool IsReadOnly {
            get { return false; }
        }

        public void Add(string key, string value) {
            this.param.Add(key, value);
        }

        public void Add(KeyValuePair<string, string> item) {
            this.param.Add(item.Key, item.Value);
        }

        public void Clear() {
            this.param.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item) {
            foreach (KeyValuePair<string, string> kvp in this.param) {
                if (kvp.Key == item.Key && kvp.Value == item.Value) {
                    return true;
                }
            }
            return false;
        }

        public bool ContainsKey(string key) {
            return this.param.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex) {
            throw new System.NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator() {
            return this.param.GetEnumerator();
        }

        public bool Remove(string key) {
            return this.param.Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item) {
            if (this.param.ContainsKey(item.Key) 
                    && this.param[item.Key] == item.Value) {
                return this.Remove(item.Key);
            }
            return false;
        }

        public bool TryGetValue(string key, out string value) {
            value = null;
            if (this.param.ContainsKey(key)) {
                value = this.param[key];
                return true;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public override string ToString() {
            if (this.param.Count == 0) return null;

            List<string> kvpStrings = new List<string>();
            foreach (KeyValuePair<string, string> kvp in this.param) {
                kvpStrings.Add(kvp.Key + "=" + kvp.Value);
            }

            return "?" + String.Join("&", kvpStrings.ToArray());
        }
    }
}