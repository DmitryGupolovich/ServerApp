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
            //  
            string url = "https://belaruspartisan.by/enjoy/473087/";


             var articles = client.Send(new DoWork { url = url }); //Get(new DoWork { url = url });

            articleBindingSource.DataSource = articles.result;
            
            //var articles = client.Get<DoWorkResponse>( url);
 
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            client = new JsonServiceClient("http://localhost:56350").WithCache();
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            var articles = client.Send(new GetArticleById { Id = Int32.Parse(textBox1.Text) }); //Get(new DoWork { url = url });

            articleBindingSource.DataSource = articles.result;
        }
    }
}
