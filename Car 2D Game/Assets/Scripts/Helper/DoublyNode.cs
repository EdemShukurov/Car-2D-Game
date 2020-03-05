public class DoublyNode<T>
{
    public DoublyNode(T data)
    {
        Data = data;
    }
    public bool IsActive { get; set; }
    public T Data { get; set; }
    public DoublyNode<T> Previous { get; set; }
    public DoublyNode<T> Next { get; set; }
}