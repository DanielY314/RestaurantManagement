namespace RPWebApi.Services
{
    public class BaseService
    {
        protected string connectionString;

        public BaseService(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
    }
}
