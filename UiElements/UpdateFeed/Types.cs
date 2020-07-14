using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Interface.UpdateFeed
{
    public enum Types
    {
        [Description("Artist")]
        Arist = 0,
        [Description("Group")]
        Group = 1,
        [Description("Character")]
        Character = 2,
        [Description("Parody")]
        Parody = 3,
        [Description("Tag")]
        Tag = 4
    }
}

