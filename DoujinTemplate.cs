using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Interface
{
    class DoujinTemplate
    {
        public int id;
        public int hentaiId;
        public int mediaId;
        public int pageCount;
        public string name;
        public string fullName;
        public string coverExt;
        public string thumbnailExt;
        public string language;
        public bool translated;
        public bool favorised;
        public string coverUrl;
        public List<string> pageExt;
        public List<string> group;
        public List<string> character;
        public List<string> parodys;
        public List<string> artists;
        public List<string> tags;

        public string Artist()
        {
            if (artists.Count > 1)
            {
                return artists.First();
            }
            else
            {
                if (artists.Count != 0)
                {
                    return artists.First();
                }
                else
                {
                    return "none";
                }
            }
        }
        public string Parody()
        {
            if (parodys.Count > 1)
            {
                return null;
            }
            else
            {
                if (parodys.Count != 0)
                {
                    return parodys.First();
                }
                else
                {
                    return "none";
                }
            }
        }
        public string Character()
        {
            if (character.Count > 1)
            {
                return character.First();
            }
            else
            {
                if (character.Count != 0)
                {
                    return character.First();
                }
                else
                {
                    return "none";
                }

            }
        }
        public string Group()
        {
            if (group.Count > 1)
            {
                return group.First();
            }
            else
            {
                if (group.Count != 0)
                {
                    return group.First();
                }
                else
                {
                    return "none";
                }
            }

        }
        string ArtistsConcat()
        {
            return string.Join(",", artists);
        }
        string GroupsConcat()
        {
            return string.Join(",", group);
        }
        string CharactersConcat()
        {
            return string.Join(",", character);
        }
        public string ParodysConcat()
        {
            return string.Join(",", parodys);
        }
        public string TagsConcat()
        {
            return string.Join(",", tags);
        }
    }

}
