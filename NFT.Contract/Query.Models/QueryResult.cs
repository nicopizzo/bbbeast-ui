namespace NFT.Contract.Query
{
    public class QueryResult<T>
    {
        public bool IsSuccess { get; set; } = true;
        public T Data { get; set; }
    }
}
