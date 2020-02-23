using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class ModelContainer : ModelContainerBase
    {
        public ModelContainer()
        {
            Users = new Dictionary<string, Personal.User>();
            UserSettingss = new Dictionary<string, Personal.UserSettings>();
            UserStatus = new Dictionary<string, Personal.UserStatus>();

            ApplicationInfos = new Dictionary<string, Infrastructure.ApplicationInfo>();
            Configs = new Dictionary<string, Infrastructure.Config>();

            Polls = new Dictionary<string, Poll.Poll>();
            PollOptions = new Dictionary<string, Poll.PollOption>();
            PollVotes = new Dictionary<string, Poll.PollVote>();

            Resources = new Dictionary<string, Resource.Resource>();
        }

        public long VersionHead {get;set;}

        public Dictionary<string, Personal.User> Users { get; }
        public Dictionary<string, Personal.UserSettings> UserSettingss { get; }
        public Dictionary<string, Personal.UserStatus> UserStatus { get; }

        public Dictionary<string, Infrastructure.ApplicationInfo> ApplicationInfos { get; }
        public Dictionary<string, Infrastructure.Config> Configs { get; }

        public Dictionary<string, Poll.Poll> Polls { get; }
        public Dictionary<string, Poll.PollOption> PollOptions { get; }
        public Dictionary<string, Poll.PollVote> PollVotes { get; }

        public Dictionary<string, Resource.Resource> Resources { get; }
    }
}
