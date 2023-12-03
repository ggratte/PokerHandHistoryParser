using HandHistories.Objects.GameDescription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Parser.Utils.Extensions;

namespace HandHistories.Parser.FileIdentifiers.GGPoker
{
    class GGPokerFileIdentifier : IFileIdentifier
    {
        public SiteName Site
        {
            get { return SiteName.GGPoker; }
        }

        public bool Match(string filetext)
        {
            return filetext.StartsWithFast("Poker Hand #");
        }
    }
}
