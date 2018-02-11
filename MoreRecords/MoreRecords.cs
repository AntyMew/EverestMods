using System;
using System.Collections.Generic;
using MonoMod;
using Celeste;

namespace Celeste.Mod.MoreRecords
{
    public class MoreRecords : EverestModule
    {
        public static MoreRecords Instance;
        public override Type SettingsType => typeof(MoreRecordsSettings);
        public static MoreRecordsSettings Settings => (MoreRecordsSettings) Instance._Settings;

        public MoreRecords()
        {

        }

        public override void Load()
        {
            Everest.Events.OuiJournal.OnEnter += JournalOnEnter;
        }

        public override void Unload()
        {

        }

        public void JournalOnEnter(OuiJournal journal, Oui from)
        {
            // Todo
        }
    }
}