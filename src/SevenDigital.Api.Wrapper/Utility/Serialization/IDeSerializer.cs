namespace SevenDigital.Api.Wrapper.Utility.Serialization
{
    public interface IDeSerializer<T>
    {
        T DeSerialize(string response);
    }

}