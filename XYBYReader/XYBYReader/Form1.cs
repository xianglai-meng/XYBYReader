﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;

namespace XYBYReader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadWeb();
        }
        private void LoadWeb()
        {
            WebRequest request = null;
            WebResponse response = null;
            StreamReader sreader = null;
            
            WebHeaderCollection headerCollection = null;
            string datetime = string.Empty;
            try
            {
                request = WebRequest.Create("https://www.readnovel.com/chapter/6969133904551803/18709155986044689");
                request.Timeout = 3000;
                request.Credentials = CredentialCache.DefaultCredentials;
                WebProxy myProxy = new WebProxy();
                request.Proxy = myProxy;
                response = (WebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                sreader = new StreamReader(stream, Encoding.UTF8);
                string strReader = sreader.ReadToEnd();
                richTextBox1.Text = strReader;
            }
            catch (Exception ex) {  }
            finally
            {
                if (request != null)
                { request.Abort(); request = null; }
                if (response != null)
                { response.Close(); response = null; }
                if (headerCollection != null)
                { headerCollection.Clear(); headerCollection = null; }
                if (sreader!=null)
                {
                    sreader.Close();
                    sreader = null;
                }
            }
        }
        private void CreatBook()
        {
            HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(richTextBox1.Text);
            var name = document.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[4]/div[1]/div[1]/div[1]/div[1]/h3[1]");
            richTextBox1.Clear();
            richTextBox1.Text = name.InnerHtml;
            var res = document.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[4]/div[1]/div[1]/div[1]/div[2]");
            string chapter = res.InnerHtml.ToString().Replace("<p>","\n");

            
            richTextBox1.AppendText(chapter);
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            CreatBook();
        }
    }
}