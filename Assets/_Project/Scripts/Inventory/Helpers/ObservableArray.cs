using System;
using System.Collections.Generic;
using System.Linq;

namespace Systems.Inventory {
    public interface IObservableArray<T> {
        event Action<T[]> AnyValueChanged;

        int Count { get; }
        T this[int index] { get; }
        
        void Swap(int index1, int index2);
        void Clear();
        bool TryAdd(T item);
        bool TryRemove(T item);
    }

    [Serializable]
    public class ObservableArray<T> : IObservableArray<T> {
        private T[] items;

        public event Action<T[]> AnyValueChanged = delegate { };
        public int Count => items.Count(i => i != null);
        public T this[int index] => items[index];

        public ObservableArray(int size = 20, IList<T> initialList = null) {
            items = new T[size];
            if (initialList != null) {
                initialList.Take(size).ToArray().CopyTo(items, 0);
                Invoke();
            }
        }

        void Invoke() => AnyValueChanged.Invoke(items);

        public void Swap(int index1, int index2) {
            (items[index1], items[index2]) = (items[index2], items[index1]);
            Invoke();
        }

        public void Clear() {
            items = new T[items.Length];
            Invoke();
        }

        public bool TryAdd(T item) {
            for (var i = 0; i < items.Length; i++) {
                if (items[i] != null) continue;
                items[i] = item;
                Invoke();
                return true;
            }

            return false;
        }

        public bool TryRemove(T item) {
            for (var i = 0; i < items.Length; i++) {
                if (!EqualityComparer<T>.Default.Equals(items[i], item)) continue;
                items[i] = default;
                Invoke();
                return true;
            }

            return false;
        }
    }
}