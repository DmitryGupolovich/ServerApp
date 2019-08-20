using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.ServiceModel
{
    public class GetArticleById : IReturn<GetArticleByIdResponse>
    {
        public int Id { get; set; }
    }
    public class GetArticleByIdResponse
    {
        public List<Article> result { get; set; }
    }
}
