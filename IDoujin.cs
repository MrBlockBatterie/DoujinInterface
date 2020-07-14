using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Interface
{
    interface IDoujin
    {
        int id { get; set; }
        int nhentaiId { get; set; }
        int mediaId { get; set; }
        int pageCount { get; set; }
        string name { get; set; }
        string fullName { get; set; }
        string coverExt { get; set; }
        string thumbnailExt { get; set; }
        string language { get; set; }
        bool translated { get; set; }
        bool favorised { get; set; }
        string coverUrl { get; set; }
        List<string> pageExt { get; set; }
        List<string> group { get; set; }
        List<string> character { get; set; }
        List<string> parodys { get; set; }
        List<string> artists { get; set; }
        List<string> tags { get; set; }

    }
}
