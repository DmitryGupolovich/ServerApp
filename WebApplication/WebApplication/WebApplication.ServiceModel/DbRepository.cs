using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication.ServiceModel
{
    public class DbRepository
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }

        public List<Article> GetArticles()
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                return db.Select<Article>();
            }
        }

        public Article GetArticlesById(int articlesId)
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                return db.Select<Article>(s => s.Id == articlesId).FirstOrDefault();
            }
        }

        public List<Article> GetArticleByWhatFind(string WhatFind)
        {
            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                return db.Select(db.From<Article>()
                    .Where(s => s.EntityText.Contains(WhatFind))
                    .OrderBy("2,3"));
            }
        }

        public void CreateTable()
        {

            using (var db = DbConnectionFactory.OpenDbConnection())
            {
                //db.CreateTableIfNotExists<Article>();
                db.DropAndCreateTable<Article>();
                db.ExecuteSql("ALTER TABLE Article ALTER COLUMN FullText Text");
            }
        }



    }
}
