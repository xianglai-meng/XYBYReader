using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYBYReader
{
    /// <summary>
    /// 完结状态
    /// </summary>
    public enum CompleteStatus { Continued, Completed, Stopped };
    /// <summary>
    /// 书目类型
    /// Modern：现代；Ancient：古代；Romance：浪漫、言情；Battle：战争；Fantasy：玄幻；Occult：灵异；Science：科幻；Game：游戏。
    /// </summary>
    public enum BookAttribute { Modern = 1, Ancient = 4, Romance = 8, Battle = 16, Fantasy = 32, Occult = 64, Science = 128, Game = 256 };

    public abstract class BookClass
    {
        private string bookName;
        private string author;
        private string chapterCount;
        /// <summary>
        /// 完结状态
        /// </summary>
        private int completeStatus;
        private string lastChapter;
        private DateTime lastUpdate;
        /// <summary>
        /// 前言、简述
        /// </summary>
        private string preface;
        /// <summary>
        /// 小说种类
        /// </summary>
        private int bookAttributes;
        /// <summary>
        /// 标签
        /// </summary>
        private List<string> lable;

        public string BookName { get => bookName; set => bookName = value; }
        public string Author { get => author; set => author = value; }
        public string ChapterCount { get => chapterCount; set => chapterCount = value; }
        public int CompleteStatus { get => completeStatus; set => completeStatus = value; }
        public string LastChapter { get => lastChapter; set => lastChapter = value; }
        public DateTime LastUpdate { get => lastUpdate; set => lastUpdate = value; }
        public string Preface { get => preface; set => preface = value; }
        public int BookType { get => bookAttributes; set => bookAttributes = value; }
        public List<string> Lable { get => lable; set => lable = value; }
    }
    public class WebBookClass : BookClass
    {
        private string bookAddress;

        public string BookAddress { get => bookAddress; set => bookAddress = value; }
    }
    //我是一个老好人
}
