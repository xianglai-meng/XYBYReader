using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XYBYReader
{
    public class BookChapterClass
    {
        private int chapterId;
        private string chapterName;
        private string chapterAddress;
        public string ChapterName { get => chapterName; set => chapterName = value; }
        public string ChapterAddress { get => chapterAddress; set => chapterAddress = value; }
        public int ChapterId { get => chapterId; set => chapterId = value; }
    }
}
