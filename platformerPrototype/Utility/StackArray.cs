#region using

using System;

#endregion

namespace platformerPrototype.Utility {
    public class StackArray<T> : IDisposable {
        private T[] _arr;
        private Int32 _head;

        public StackArray(Int32 capacity, Int32 padding = 2) {
            Length = capacity;
            if (padding < 2)
                padding = 2;
            _arr = new T[capacity + padding];
        }

        public Int32 Length { get; internal set; }
        public Boolean Disposed { get; internal set; }

        public T this[Int32 i] {
            get {
                if (_head + i >= Length)
                    return _arr[_head + i - Length];

                return _arr[_head + i];
            }
            set {
                if (i >= Length)
                    throw new IndexOutOfRangeException();

                if (_head + i >= _arr.Length)
                    _arr[_head + i - _arr.Length] = value;
                else
                    _arr[_head + i] = value;
            }
        }

        public void Dispose() {
            if (Disposed)
                return;

            _arr = null;
            _head = 0;
            Disposed = true;
        }

        public void Insert(T newItem) {
            _head--;
            if (_head < 0)
                _head += Length;
            _arr[_head] = newItem;
        }
    }
}
