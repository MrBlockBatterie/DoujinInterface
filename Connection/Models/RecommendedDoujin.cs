using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doujin_Interface.Connection.Models
{
    public class RecommandationBindingModel
    {
        public string FriendName { get; set; }
        public int DoujinId { get; set; }

    }
    public class RecommendedByUserViewModel
    {
        public string Username { get; set; }
        public int[] Ids { get; set; }

    }
    public class RecommendationViewModel
    {
        public RecommendedByUserViewModel[] Users { get; set; }
    }
}
