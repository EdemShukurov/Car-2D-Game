using System.Collections;
using System.Collections.Generic;

public class CircularDoublyLinkedList<T> : IEnumerable<T>
{

    private DoublyNode<T> head; // the first element
    private int count;

    public int Count { get { return count; } }
    public bool IsEmpty { get { return count == 0; } }

    public T GetActiveElement()
    {
        DoublyNode<T> current = head;
        if (current == null)
            return default(T);

        do
        {
            if (current.IsActive)
                return current.Data;

            current = current.Next;
        }
        while (current != head);

        return default(T);
    }

    public void SetActiveElement(T data)
    {
        DoublyNode<T> current = head;
        if (current == null)
            return;

        do
        {
            if (current.Data.Equals(data))
                current.IsActive = true;
            else
                current.IsActive = false;

            current = current.Next;
        }
        while (current != head);
    }

    public T GetPreviousElementFromActiveElemenet()
    {
        DoublyNode<T> current = head;
        if (current == null)
            return default(T);

        do
        {
            if (current.IsActive)
                return current.Previous.Data;

            current = current.Next;
        }
        while (current != head);

        return default(T);
    }

    public T GetNextElementFromActiveElemenet()
    {
        DoublyNode<T> current = head;
        if (current == null)
            return default(T);

        do
        {
            if (current.IsActive)
                return current.Next.Data;

            current = current.Next;
        }
        while (current != head);

        return default(T);
    }

    public List<T> GetRequiredList()
    {
        DoublyNode<T> current = head;
        if (current == null)
            return null;

        var list = new List<T>();

        do
        {
            if (current.IsActive)
            {
                list.Add(current.Previous.Data);
                list.Add(current.Data);
                list.Add(current.Next.Data);
                break;
            }
        }
        while (current != head);

        return list;
    }

    /// <summary>
    /// Get required elements as a list
    /// </summary>
    /// <param name="data"></param>
    /// <param name="requiredQuantity"></param>
    /// <returns>List of elements</returns>
    public List<T> GetElements(T data, int requiredQuantity)
    {
        DoublyNode<T> requiredNode = GetNode(data);

        if (requiredNode != null && Count >= requiredQuantity)
        {
            List<T> elements = new List<T>();
            for (int i = 0; i < requiredQuantity; i++)
            {
                elements.Add(requiredNode.Data);
                requiredNode = requiredNode.Next;
            }

            return elements;
        }

        return null;
    }

    /// <summary>
    /// Get required elements as a list
    /// </summary>
    /// <param name="data"></param>
    /// <param name="requiredQuantity"></param>
    /// <returns>List of elements</returns>
    public List<T> GetAllElements()
    {
        DoublyNode<T> current = head;
        if (current == null)
            return null;

        List<T> list = new List<T>();

        do
        {
            list.Add(current.Data);

            current = current.Next;
        }
        while (current != head);

        return list;
    }


    /// <summary>
    /// Get node
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public DoublyNode<T> GetNode(T data)
    {
        DoublyNode<T> current = head;
        if (current == null)
            return null;

        do
        {
            if (current.Data.Equals(data))
                return current;
            else
                current = current.Next;
        }
        while (current != head);

        return null;
    }

    /// <summary>
    /// Add element
    /// </summary>
    /// <param name="data">adding element</param>
    public void Add(T data)
    {
        DoublyNode<T> node = new DoublyNode<T>(data);

        if (head == null)
        {
            head = node;
            head.Next = node;
            head.Previous = node;
        }
        else
        {
            node.Previous = head.Previous;
            node.Next = head;
            head.Previous.Next = node;
            head.Previous = node;
        }
        count++;
    }

    /// <summary>
    /// Remove element
    /// </summary>
    /// <param name="data">element</param>
    /// <returns>if element was removed return true</returns>
    public bool Remove(T data)
    {
        DoublyNode<T> current = head;

        DoublyNode<T> removedItem = null;
        if (count == 0) return false;

        // search node
        do
        {
            if (current.Data.Equals(data))
            {
                removedItem = current;
                break;
            }
            current = current.Next;
        }
        while (current != head);

        if (removedItem != null)
        {
            // if the single element is needed to remove
            if (count == 1)
                head = null;
            else
            {
                // if the first element is needed to remove
                if (removedItem == head)
                {
                    head = head.Next;
                }
                removedItem.Previous.Next = removedItem.Next;
                removedItem.Next.Previous = removedItem.Previous;
            }
            count--;
            return true;
        }
        return false;
    }

    public void Clear()
    {
        head = null;
        count = 0;
    }

    public bool Contains(T data)
    {
        DoublyNode<T> current = head;
        if (current == null) return false;
        do
        {
            if (current.Data.Equals(data))
                return true;
            current = current.Next;
        }
        while (current != head);
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)this).GetEnumerator();
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        DoublyNode<T> current = head;
        do
        {
            if (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
        while (current != head);
    }
}
