namespace judge.system
{
    public interface IHasMetaData<TMetadata>
    {
        Task<TMetadata> GetMetadata();

        Task SetMetadata(TMetadata value);
    }
}
