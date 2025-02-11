namespace BlazorGoogle.Development.Data
{
    public interface IDatabaseSettings
    {
        string ConnectionString
        {
            get;
        }

        int Timeout
        {
            get;
            set;
        }
    }
}
