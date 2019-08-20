using ServiceStack;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WebApp.ServiceModel;

namespace ClientApplication
{
    public partial class Form1 : Form
    {
        ServiceStack.IServiceClient client;
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            
            string url = "https://belaruspartisan.by/politic/471971/";
            var articles = client.Send(new DoWork { url = url }); //Get(new DoWork { url = url });
            MessageBox.Show(articles.result);
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            client = new JsonServiceClient("http://localhost:57556").WithCache();
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            var articles = client.Send(new GetArticleById { Id = Int32.Parse(textBox1.Text) }); 

            articleBindingSource.DataSource = articles.result;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            var article = client.Get(new GetArticle());
            articleBindingSource.DataSource = article.result;
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            var hello = client.Send(new Hello { Name = "test value" }); //Get(new DoWork { url = url });
            MessageBox.Show(hello.Result);
        }
    }
}
