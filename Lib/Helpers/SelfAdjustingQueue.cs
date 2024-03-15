namespace JailbreakExtras;

public partial class JailbreakExtras
{
    private class SelfAdjustingQueue<T>
    {
        private Queue<T> Queue;
        private int MaxSize;

        public SelfAdjustingQueue(int maxSize)
        {
            this.MaxSize = maxSize;
            Queue = new Queue<T>(maxSize);
        }

        public T Enqueue(T item)
        {
            var dequeue = default(T);
            if (Queue.Count >= MaxSize)
            {
                dequeue = Queue.Dequeue(); // Remove the oldest item
            }
            Queue.Enqueue(item); // Add the new item
            return dequeue;
        }

        public T Dequeue()
        {
            if (Queue.Count == 0)
            {
                return default(T);
            }
            return Queue.Dequeue();
        }

        public List<T> ToList()
        {
            return Queue.ToList();
        }
    }
}