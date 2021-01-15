using Prevail.Inventory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prevail.Data
{
    public class ContainerCache : IDictionary<int, Container>
    {
        readonly Dictionary<int, Container> Containers = new Dictionary<int, Container>();

        public Container this[int key] { get => ((IDictionary<int, Container>)Containers)[key]; set => ((IDictionary<int, Container>)Containers)[key] = value; }

        public ICollection<int> Keys => ((IDictionary<int, Container>)Containers).Keys;

        public ICollection<Container> Values => ((IDictionary<int, Container>)Containers).Values;

        public int Count => ((ICollection<KeyValuePair<int, Container>>)Containers).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<int, Container>>)Containers).IsReadOnly;

        public void Add(int key, Container value)
        {
            ((IDictionary<int, Container>)Containers).Add(key, value);
        }

        public void Add(KeyValuePair<int, Container> item)
        {
            ((ICollection<KeyValuePair<int, Container>>)Containers).Add(item);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<int, Container>>)Containers).Clear();
        }

        public bool Contains(KeyValuePair<int, Container> item)
        {
            return ((ICollection<KeyValuePair<int, Container>>)Containers).Contains(item);
        }

        public bool ContainsKey(int key)
        {
            return ((IDictionary<int, Container>)Containers).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<int, Container>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<int, Container>>)Containers).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<int, Container>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<int, Container>>)Containers).GetEnumerator();
        }

        public bool Remove(int key)
        {
            return ((IDictionary<int, Container>)Containers).Remove(key);
        }

        public bool Remove(KeyValuePair<int, Container> item)
        {
            return ((ICollection<KeyValuePair<int, Container>>)Containers).Remove(item);
        }

        public bool TryGetValue(int key, out Container value)
        {
            return ((IDictionary<int, Container>)Containers).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Containers).GetEnumerator();
        }
    }
}
