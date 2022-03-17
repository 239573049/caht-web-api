namespace Chat.WebCore.Models;

public class PagingVM<T>
{
    public int Count { get; set; }
    public T? Data { get; set; }
    public PagingVM(T? data, int count)
    {
        Count = count;
        Data = data;
    }
}
