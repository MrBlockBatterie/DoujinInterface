using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Interface.Connection.Models
{
    public class DoujinSearchResult
    {
        public DoujinResult[] Result { get; set; }
        public int Num_pages { get; set; }
        public int Per_page { get; set; }
    }
    public class DoujinResult
    {
        public int Id { get; set; }
        public string Media_id { get; set; }
        public DoujinTitle Title {get;set;}
        public DoujinImages[] Images { get; set; }
        public string Scanlator { get; set; }
        public int Upload_date { get; set; }
        public DoujinTags[] Tags { get; set; }
        public int Num_pages { get; set; }
        public int Num_favorites { get; set; }

    }
    public class DoujinTitle
    {
        public string English { get; set; }
        public string Japanese { get; set; }
        public string Pretty { get; set; }

    }
    public class DoujinImages
    {
        public DoujinPages[] Pages { get; set; }
        public DoujinCover Cover { get; set; }
        public DoujinThumbnail Thumbnail { get; set; }
    }
    public class DoujinPages
    {
        public string T { get; set; }
        public int W { get; set; }
        public int H { get; set; }
    }
    public class DoujinCover
    {
        public string T { get; set; }
        public int W { get; set; }
        public int H { get; set; }
    }
    public class DoujinThumbnail
    {
        public string T { get; set; }
        public int W { get; set; }
        public int H { get; set; }
    }
    public class DoujinTags
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Count { get; set; }
    }
}
