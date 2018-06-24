using System;
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
    public partial class ReadChapterFrm : Form
    {
        HtmlAgilityPack.HtmlDocument document;
        string chapterAddress = "";
        List<BookChapterClass> bookChapterList = null;
        List<WebBookClass> bookInfoList = null;
        TreeNode nextNode = null;
        public ReadChapterFrm()
        {
            InitializeComponent();
        }

        private void ReadChapterFrm_Load(object sender, EventArgs e)
        {
            // string firstChapter = "https://www.readnovel.com/chapter/6969133904551803/18709155986044689";
            // string firstChapter = "https://www.readnovel.com/book/7635085504787403#Catalog";
            string firstChapter = "https://www.readnovel.com/search?kw=%E4%BA%B2%E5%85%B5%E6%98%AF%E5%A5%B3%E5%A8%83";
            LoadWebBook(firstChapter);


            FindBookInfo("");
            FindCatalog(bookInfoList[0].BookAddress + "#Catalog");

            LoadBookTree();
            if (tvBook != null)
            {
                nextNode = tvBook.Nodes[0].Nodes[2];

                List<BookChapterClass> bl = new List<BookChapterClass>();
                bl = bookChapterList.Where(x => x.Id == (int)nextNode.Tag).ToList();
                string chapterAddress = bl[0].ChapterAddress;
                LoadWebBook(chapterAddress);
                LoadBook();

                tvBook.SelectedNode = nextNode;
                tvBook.Focus();
            }
        }
        /// <summary>
        /// 加载网页形式的书
        /// </summary>
        /// <param name="address"></param>
        private void LoadWebBook(string address)
        {
            WebRequest request = null;
            WebResponse response = null;
            StreamReader sreader = null;
            WebHeaderCollection headerCollection = null;
            string datetime = string.Empty;
            try
            {
                request = WebRequest.Create(address);
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
            catch (Exception ex) { }
            finally
            {
                if (request != null)
                { request.Abort(); request = null; }
                if (response != null)
                { response.Close(); response = null; }
                if (headerCollection != null)
                { headerCollection.Clear(); headerCollection = null; }
                if (sreader != null)
                {
                    sreader.Close();
                    sreader = null;
                }
            }
        }
        /// <summary>
        /// 加载正文
        /// </summary>
        private void LoadBook()
        {
            if (document != null)
            {
                document = null;
                document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(richTextBox1.Text);
                // 腹黑女
                var name = document.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[4]/div[1]/div[1]/div[1]/div[1]/h3[1]");
                richTextBox1.Clear();
                richTextBox1.Text = name.InnerHtml;
                var res = document.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[4]/div[1]/div[1]/div[1]/div[2]");
                string chapter = res.InnerHtml.ToString().Replace("<p>", "\n");

                // 亲兵是女娃
                //richTextBox1.Clear();

                //var name = document.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[4]/div[1]/div[2]/div[1]/div[1]/h3").InnerText;
                //richTextBox1.Text = name.ToString();
                //string chapter = document.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[4]/div[1]/div[2]/div[1]/div[2]").InnerText;

                richTextBox1.AppendText(chapter);
            }
            //richTextBox1.Select(0, 0);
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            //FindCatalog("https://www.readnovel.com/book/7635085504787403#Catalog");//https://www.readnovel.com/book/7635085504787403
            //JumpChapter(5);
            FindBookInfo("");
            FindCatalog(bookInfoList[0].BookAddress + "#Catalog");

            LoadBookTree();
            if (tvBook != null)
            {
                tvBook.SelectedNode = tvBook.Nodes[0].Nodes[2];
            }
        }

        private void btnNextChapter_Click(object sender, EventArgs e)
        {
            FindNextChapter();
            LoadWebBook(chapterAddress);
            LoadBook();

            tvBook.SelectedNode = nextNode;
        }
        /// <summary>
        /// 查找下一章节
        /// </summary>
        private void FindNextChapter()
        {
            if (document != null)
            {
                string nextChapter = document.DocumentNode.SelectSingleNode(@"/html[1]/body[1]/div[1]/div[4]/div[1]/div[1]")
                    .Attributes["data-nurl"].Value.ToString();
                chapterAddress = "https:" + nextChapter;

            }
        }
        /// <summary>
        /// 找出书目录
        /// </summary>
        /// <param name="address"></param>
        private void FindCatalog(string address)
        {
            LoadWebBook(address);

            if (document != null)
            {
                document = null;
                document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(richTextBox1.Text);

                bookChapterList = new List<BookChapterClass>();
                BookChapterClass bookChapter = null;

                HtmlNodeCollection node = document.DocumentNode.SelectNodes(@"/html[1]/body[1]/div[1]/div[3]/div[3]/div[2]/div[1]/ul/li");
                int id = 10001;
                foreach (var item in node)
                {
                    bookChapter = new BookChapterClass();
                    bookChapter.Id = id;
                    bookChapter.ChapterRowId = Convert.ToInt16(item.Attributes["data-rid"].Value.ToString());
                    bookChapter.ChapterAddress = "https:" + item.FirstChild.Attributes["href"].Value.ToString();
                    bookChapter.ChapterName = item.FirstChild.InnerText;

                    bookChapterList.Add(bookChapter);
                    id++;
                }
            }
        }
        /// <summary>
        /// 跳转指定章节
        /// </summary>
        /// <param name="chapterId"></param>
        private void JumpChapter(int chapterId)
        {
            chapterAddress = "https:" + bookChapterList.Find(x => x.ChapterRowId == chapterId).ChapterAddress.ToString();

            LoadWebBook(chapterAddress);
            LoadBook();
        }

        private void FindBookInfo(string address)
        {
            if (document == null)
            {
                document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(richTextBox1.Text);

                bookInfoList = new List<WebBookClass>();
                WebBookClass bookInfo;

                HtmlNodeCollection node = document.DocumentNode.SelectNodes(@"/html/body/div/div[2]/div[2]/div[1]/div[1]/div/ul/li");
                string path;
                foreach (var item in node)
                {
                    bookInfo = new WebBookClass();
                    path = item.ChildNodes[3].XPath;
                    bookInfo.Author = item.ChildNodes[3].SelectSingleNode(path + @"/p[1]/a").InnerText;
                    bookInfo.BookName = item.ChildNodes[3].ChildNodes[1].InnerText;
                    //bookInfo.chapterCount = item.ChildNodes[3].SelectSingleNode(path+@"");
                    //bookInfo.completeStatus = item.ChildNodes[3].ChildNodes[1].InnerText;
                    //bookInfo.lastChapter = item.ChildNodes[3].ChildNodes[1].InnerText;
                    //bookInfo.lastUpdate = item.ChildNodes[3].ChildNodes[1].InnerText;
                    bookInfo.Preface = item.ChildNodes[3].SelectSingleNode(path + @"/p[2]").InnerText;

                    bookInfo.BookAddress = "https:" + item.ChildNodes[3].SelectSingleNode(path + @"/h4/a").Attributes["href"].Value.ToString();

                    bookInfoList.Add(bookInfo);
                }
            }
        }

        private void LoadBookTree()
        {

            TreeNode bookNode = new TreeNode();
            bookNode.Text = bookInfoList[0].BookName;
            bookNode.Tag = 101;


            for (int i = 0; i < bookChapterList.Count; i++)
            {
                TreeNode chapterNode = new TreeNode();
                chapterNode.Tag = bookChapterList[i].Id;
                chapterNode.Text = bookChapterList[i].ChapterName;
                bookNode.Nodes.Add(chapterNode);

            }

            tvBook.Nodes.Add(bookNode);
            tvBook.ExpandAll();
        }

        private void tvBook_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (tvBook.SelectedNode != null)
            {
                if (tvBook.SelectedNode != e.Node)
                {
                    List<BookChapterClass> bl = new List<BookChapterClass>();
                    bl = bookChapterList.Where(x => x.Id == (int)e.Node.Tag).ToList();
                    string chapterAddress = bl[0].ChapterAddress;
                    LoadWebBook(chapterAddress);
                    LoadBook();
                    nextNode = e.Node;
                }

            }

        }

        private void btnDownLoad_Click(object sender, EventArgs e)
        {

            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = tvBook.Nodes[0].Text;
            dialog.Filter = "txt files(*.txt)|*.txt";
            DialogResult dr = dialog.ShowDialog();
            if (dr == DialogResult.OK && dialog.FileName.Length > 0)
            {
                richTextBox1.SaveFile(dialog.FileName, RichTextBoxStreamType.PlainText);
            }
        }
    }
}
