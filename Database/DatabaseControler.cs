using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Doujin_Interface.Database.DoujinSet;

namespace Doujin_Interface.Database
{
    class DatabaseControler
    {
        public const string favDataPath = "favs.xml";
        public const string cacheDataPath = "cache.xml";

        public static DoujinDataDataTable mainDataTable = new DoujinDataDataTable();
        public static DoujinDataDataTable favorites = new DoujinDataDataTable();
        public static DoujinDataDataTable localDummy = new DoujinDataDataTable();
        public static KeepUpdatedDataTable updates = new KeepUpdatedDataTable();
        public static DoujinCacheDataTable doujinCache = new DoujinCacheDataTable();
    }
}
