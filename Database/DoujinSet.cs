using System;
using System.Collections.Generic;

namespace Doujin_Interface.Database
{


    partial class DoujinSet
    {
        partial class KeepUpdatedDataTable
        {
        }

        partial class CharacterDataTable
        {


        }

        partial class DoujinDataDataTable
        {
            public void Init(List<Doujin> doujinshi)
            {
                foreach (Doujin doujin in doujinshi)
                {
                    Console.WriteLine($"Database: current nHID of {doujin.name} is {doujin.nhentaiId}");
                    var row = this.AddDoujinDataRow(this.Rows.Count, doujin.nhentaiId, doujin.mediaId, doujin.name, doujin.fullName, doujin.ArtistsConcat(),
                                                    doujin.CharactersConcat(), doujin.ParodysConcat(), doujin.GroupsConcat(), doujin.TagsConcat(), doujin.language,
                                                    doujin.favorised, doujin.pageCount, doujin.coverUrl, doujin.coverExt);
                    doujin.id = Rows.Count;





                }
            }
        }
    }
}
