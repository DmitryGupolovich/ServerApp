/* Options:
Date: 2019-08-14 15:03:43
Version: 5,60
Tip: To override a DTO option, remove "//" prefix before updating
BaseUrl: http://localhost:56350

//GlobalNamespace: 
//MakePartial: True
//MakeVirtual: True
//MakeInternal: False
//MakeDataContractsExtensible: False
//AddReturnMarker: True
//AddDescriptionAsComments: True
//AddDataContractAttributes: False
//AddIndexesToDataMembers: False
//AddGeneratedCodeAttributes: False
//AddResponseStatus: False
//AddImplicitVersion: 
//InitializeCollections: True
//ExportValueTypes: False
//IncludeTypes: 
//ExcludeTypes: 
//AddNamespaces: 
//AddDefaultXmlNamespace: http://schemas.servicestack.net/types
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;
using WebApp.ServiceModel;


namespace WebApp.ServiceModel
{

    public partial class Article
    {
        public virtual int Id { get; set; }
        public virtual string HeaderArticle { get; set; }
        public virtual string UrlArticle { get; set; }
        [StringLength(2147483647)]
        public virtual string FullText { get; set; }

        public virtual string Text { get; set; }
        public virtual DateTime? LastUpdated { get; set; }
        public virtual string EntityText { get; set; }
    }

    public partial class DoWork
        : IReturn<DoWorkResponse>
    {
        public virtual string url { get; set; }
    }

    public partial class DoWorkResponse
    {
        public virtual string result { get; set; }
    }

    public partial class GetArticle
        : IReturn<GetArticleResponse>
    {
    }

    public partial class GetArticleById
        : IReturn<GetArticleByIdResponse>
    {
        public virtual int Id { get; set; }
    }

    public partial class GetArticleByIdResponse
    {
        public GetArticleByIdResponse()
        {
            result = new List<Article>{};
        }

        public virtual List<Article> result { get; set; }
    }

    public partial class GetArticleResponse
    {
        public GetArticleResponse()
        {
            result = new List<Article>{};
        }

        public virtual List<Article> result { get; set; }
    }

    [Route("/hello/{Name}")]
    public partial class Hello
        : IReturn<HelloResponse>
    {
        public virtual string Name { get; set; }
    }

    public partial class HelloResponse
    {
        public virtual string Result { get; set; }
    }
}

