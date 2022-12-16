namespace StreamHelper.Core.Extensions;

public static class ObjectExtensions
{
    public static List<TItem> AddToNewList<TItem>(this TItem item)
        => new() { item };
}