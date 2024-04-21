namespace judge.system
{
    public interface IHasId<TId>
    {
        TId Id { get; }
    }
}
