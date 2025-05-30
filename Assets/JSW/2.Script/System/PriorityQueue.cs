using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<(T item, int priority)> heap = new();

    public int Count => heap.Count;

    public void Enqueue(T item, int priority)
    {
        heap.Add((item, priority));
        int c = heap.Count - 1;
        while (c > 0)
        {
            int p = (c - 1) / 2;
            if (heap[c].priority >= heap[p].priority) break;
            (heap[c], heap[p]) = (heap[p], heap[c]);
            c = p;
        }
    }

    public T Dequeue()
    {
        int li = heap.Count - 1;
        (heap[0], heap[li]) = (heap[li], heap[0]);
        T ret = heap[li].item;
        heap.RemoveAt(li);
        --li;
        int p = 0;
        while (true)
        {
            int c = p * 2 + 1;
            if (c > li) break;
            int rc = c + 1;
            if (rc <= li && heap[rc].priority < heap[c].priority) c = rc;
            if (heap[p].priority <= heap[c].priority) break;
            (heap[p], heap[c]) = (heap[c], heap[p]);
            p = c;
        }
        return ret;
    }

    public bool Contains(T item)
    {
        return heap.Exists(e => EqualityComparer<T>.Default.Equals(e.item, item));
    }
}
