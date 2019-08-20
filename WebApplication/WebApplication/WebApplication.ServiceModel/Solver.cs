using Abot.Crawler;
using Abot.Poco;
using AngleSharp.Dom;
using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Data;
using ServiceStack.Script;
using HtmlAgilityPack;
using AngleSharp.Extensions;
using EP.Ner;
using EP.Ner.Core;
using EP.Morph;
using System.Text.RegularExpressions;
namespace WebApplication.ServiceModel
{
   public class Solver
    {
        private PoliteWebCrawler crawler { get; set; }
        private string url { get; set; }
        private IDbConnectionFactory dbConnectionFactory { get; set; }
     
        public Solver(string url, IDbConnectionFactory dbConnectionFactory)
        {
            this.dbConnectionFactory = dbConnectionFactory;
            this.url = url;
            CrawlConfiguration crawlConfig = new CrawlConfiguration();
            crawlConfig.CrawlTimeoutSeconds = 0;
            crawlConfig.MaxConcurrentThreads = 20;
            crawlConfig.MaxPagesToCrawl = 100;
            crawlConfig.MaxCrawlDepth = 100;

            crawler = new PoliteWebCrawler(crawlConfig, null, null, null, null, null, null, null, null);

            crawler.PageCrawlStartingAsync += crawler_ProcessPageCrawlStarting;
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;

            using (var db = dbConnectionFactory.OpenDbConnection())
            {
                //db.CreateTableIfNotExists<Article>();
                db.DropAndCreateTable<Article>();
               // db.ExecuteSql("ALTER TABLE Article ALTER COLUMN FullText Text");
            }

        } 
        public void Solve()
        {
            CrawlResult result = crawler.Crawl(new Uri(url));
            
            if (result.ErrorOccurred)
                Console.WriteLine("Crawl of {0} completed with error: {1}", result.RootUri.AbsoluteUri, result.ErrorException.Message);
            else
                Console.WriteLine("Crawl of {0} completed without error.", result.RootUri.AbsoluteUri);
        }

        void crawler_ProcessPageCrawlStarting(object sender, PageCrawlStartingArgs e)
        {
            PageToCrawl pageToCrawl = e.PageToCrawl;
            new HelloResponse { Result = $"About to crawl link {pageToCrawl.Uri.AbsoluteUri} which was found on page {1}, {pageToCrawl.ParentUri.AbsoluteUri}!" };
        }

        void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            CrawledPage crawledPage = e.CrawledPage;
            string uri = crawledPage.Uri.AbsoluteUri;

            if (crawledPage.WebException != null || crawledPage.HttpWebResponse.StatusCode != HttpStatusCode.OK)
                new HelloResponse { Result = $"Crawl of page failed {uri}!" };

            else
                new HelloResponse { Result = $"Crawl of page succeeded {uri}!" };

            var htmlAgilityPackDocument = crawledPage.HtmlDocument; //Html Agility Pack parser

            Extract(e, htmlAgilityPackDocument);
        }
        string RemoveQuotes(string articleText)
        {
            return Regex.Replace(articleText,"\"",String.Empty);

        }
        void Extract(PageCrawlCompletedArgs page, HtmlDocument htmlAgilityPackDocument)
        {
            try
            {
                DateTime date = default(DateTime);
                string entityText = "";
                StringBuilder text = new StringBuilder();

                StringBuilder headerArticle = new StringBuilder();
                StringBuilder articleText = new StringBuilder();

                var t2 = htmlAgilityPackDocument.DocumentNode.SelectSingleNode("//div[@class='news-detail']");
                if (t2 != null)
                {
                    // You need to add an * to the xpath. The * means you want to select any element.
                    // With an xpath query you can also use "." to indicate the search should start at the current node.
                    HtmlNode HeaderArticleNode = t2.SelectSingleNode(".//*[@class='name']");
                    if (HeaderArticleNode != null)
                    {
                        headerArticle.Append(RemoveQuotes(HeaderArticleNode.InnerText));
                    }

                    HtmlNode TextNode = t2.SelectSingleNode(".//*[@id='detailText']");
                    if (TextNode != null)
                    {
                        
                        if (TextNode.InnerText != "")
                        {
                            
                            articleText.Append(RemoveQuotes(TextNode.InnerText));

                            ProcessorService.Initialize(MorphLang.RU | MorphLang.EN);
                            EP.Ner.Geo.GeoAnalyzer.Initialize();
                            EP.Ner.Org.OrganizationAnalyzer.Initialize();
                            EP.Ner.Person.PersonAnalyzer.Initialize();

                            //// создаём экземпляр обычного процессора
                            using (Processor proc = ProcessorService.CreateProcessor())
                            {
                                // анализируем текст
                                AnalysisResult ar = proc.Process(new SourceOfAnalysis(articleText.ToString().Trim()));

                                // результирующие сущности
                                foreach (var e in ar.Entities)
                                {
                                    //  if (e.GetType().Name.Equals("GeoReferent"))
                                    //e.GetType().Name + " " + e;
                                    entityText += e.ToString() + " ";
                                }
                            }
                        }


                    }

                    /*
                    // Вариант с перебором всех дочерних узлов и использованием Descendants
                    HtmlNodeCollection childNodes = t2.ChildNodes;
                    foreach (var nNode in childNodes.Descendants("h1"))
                    {
                        if (nNode.NodeType == HtmlNodeType.Element)
                        {
                            name = nNode.InnerText;
                        }
                    }
                    */


                }
                htmlAgilityPackDocument.DocumentNode.SelectNodes("//style|//script").ToList().ForEach(n => n.Remove());

                var xpath = "//text()[not(normalize-space())]";
                var emptyNodes = htmlAgilityPackDocument.DocumentNode.SelectNodes(xpath);

                //replace each and all empty text nodes with single new-line text node
                foreach (HtmlNode emptyNode in emptyNodes)
                {
                    emptyNode.ParentNode
                             .ReplaceChild(HtmlTextNode.CreateNode(Environment.NewLine)
                                            , emptyNode
                                           );
                }

                string FullText = htmlAgilityPackDocument.DocumentNode.InnerHtml.AsString();

                var bla = htmlAgilityPackDocument.DocumentNode.SelectSingleNode("//span[@class='news-date-time news_date']");
                if (bla != null)
                {
                    bla.InnerHtml.ToString();
                    date = DateTime.Parse(bla.InnerText);
                }
                
                if (headerArticle.Length != 0 & articleText.Length != 0)
                {
                    using (var db = dbConnectionFactory.OpenDbConnection())
                    {
                        db.Insert(new Article()
                        {
                           // HeaderArticle = HeaderArticle,
                           HeaderArticle = headerArticle.ToString().Trim(),
                            UrlArticle = page.CrawledPage.Uri.AbsoluteUri,
                            FullText = page.CrawledPage.HtmlDocument.DocumentNode.OuterHtml,
                            Text = articleText.ToString().Trim(),
                            LastUpdated = date,
                            EntityText = entityText,
                        }
                        );
                    }
                }
            }
            catch (Exception e) {
                Console.WriteLine("", e.Message);
            }
            
        }

    }
}
