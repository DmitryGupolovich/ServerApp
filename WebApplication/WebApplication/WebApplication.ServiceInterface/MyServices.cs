using ServiceStack;
using ServiceStack.Data;
using WebApplication.ServiceModel;

namespace WebApplication.ServiceInterface
{
    public class MyServices : Service
    {
        public IDbConnectionFactory DbConnectionFactory { get; set; }
        DbRepository repository;
        public MyServices(DbRepository _repository)
        {
            repository = _repository;
        }

        public object Any(Hello request)
        {
            return new HelloResponse { Result = $"Hello, {request.Name}!" };
        }

        public object Any(DoWork request)
        {

            var solver = new Solver(request.url, DbConnectionFactory);
            solver.Solve();

            return new DoWorkResponse()
            { result = "Parsing is done" };

        }
        public object Any(GetArticle request)
        {
            var result = repository.GetArticles();
            return new GetArticleResponse()
            {
                result = result
            };
        }
        public object Any(GetArticleById getArticleById)
        {
            var result = repository.GetArticlesById(getArticleById.Id);

            return new GetArticleByIdResponse()
            { result = result.InList() };

        }
        public object Any(GetArticleByWhatFind getArticleByWhatFind)
        {
            var result = repository.GetArticleByWhatFind(getArticleByWhatFind.WhatFind);

            return new GetArticleByWhatFindResponse()
            { result = result };

        }

    }
}