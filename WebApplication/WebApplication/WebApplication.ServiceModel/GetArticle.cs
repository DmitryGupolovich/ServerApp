using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ServiceStack;

namespace WebApplication.ServiceModel
{
    public class GetArticle : IReturn<GetArticleResponse>
    {

    }
    public class GetArticleResponse
    {
        public List<Article> result { get; set; }
    }
}
