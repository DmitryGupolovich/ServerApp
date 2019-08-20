using ServiceStack;

namespace WebApplication.ServiceModel
{
    public class DoWork : IReturn<DoWorkResponse>
    {
        public string url { get; set; }

    }
    public class DoWorkResponse
    {
        public string result { get; set; }
    }
}
