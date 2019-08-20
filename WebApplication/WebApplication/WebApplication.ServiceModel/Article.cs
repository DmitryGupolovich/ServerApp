using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication.ServiceModel
{
    public class Article
    {
        [AutoIncrement] // Creates Auto primary key
        public int Id { get; set; }
        //полный HTML текст страницы,
        //название статьи,
        //адрес(url), 
        //текст статьи без разметки,
        //дату статьи.
        public string HeaderArticle { get; set; }
        public string UrlArticle { get; set; }

        [ServiceStack.DataAnnotations.StringLength(ServiceStack.DataAnnotations.StringLengthAttribute.MaxText)]
        public string FullText { get; set; }
        [ServiceStack.DataAnnotations.StringLength(ServiceStack.DataAnnotations.StringLengthAttribute.MaxText)]
        public string Text { get; set; }
        public DateTime? LastUpdated { get; set; }
        [ServiceStack.DataAnnotations.StringLength(ServiceStack.DataAnnotations.StringLengthAttribute.MaxText)]
        public string EntityText { get; set; }

        public static implicit operator List<object>(Article v)
        {
            throw new NotImplementedException();
        }
    }
}
