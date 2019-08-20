using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication.ServiceModel
{

        public class GetArticleByWhatFind : IReturn<GetArticleByWhatFindResponse>
        {
            public string WhatFind { get; set; }
        }
        public class GetArticleByWhatFindResponse
        {
            public List<Article> result { get; set; }
        }

}
