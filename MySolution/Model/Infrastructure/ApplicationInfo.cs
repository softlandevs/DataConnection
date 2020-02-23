using DataInfoFramework.Annotation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Infrastructure
{
    [ManagedClass]
    public class ApplicationInfo : Base.ManagedObjectBase
    {
        [ManagedStringProperty]
        public string Title { get; set; }

        [ManagedStringProperty]
        public string Description { get; set; }


        #region "-- Online --"

        [ManagedBoolProperty]
        public bool AlwaysOnline { get; set; }

        [ManagedBoolProperty]
        public bool MultiplayerAlwaysOnline { get; set; }

        #endregion


        #region "-- Multiplayer --"

        [ManagedBoolProperty]
        public bool HasMultiplayer { get; set; }

        [ManagedIntProperty]
        public int MaxPlayerCount { get; set; }

        #endregion


        #region "-- Resources --"

        #endregion

    }
}
