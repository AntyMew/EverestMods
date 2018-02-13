using System;
using System.Collections.Generic;
using System.Reflection;
using MonoMod.Detour;
using Microsoft.Xna.Framework;
using Celeste;
using Monocle;

namespace Celeste.Mod.MoreRecords
{
    public class MoreRecords : EverestModule
    {
        public static MoreRecords Instance;
        public override Type SettingsType => typeof(MoreRecordsSettings);
        public static MoreRecordsSettings Settings => (MoreRecordsSettings) Instance._Settings;
        public override Type SaveDataType => typeof(MoreRecordsSaveData);
        public static MoreRecordsSaveData SaveData => (MoreRecordsSaveData) Instance._SaveData;

        public Vector2? LastRespawnPoint;
        public long LastCheckpointTime;
        public long DeathlessTime;

        private readonly static MethodInfo m_RegisterCompletion = typeof(SaveData).GetMethod("RegisterCompletion", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        private readonly static MethodInfo m_Update = typeof(Level).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        public MoreRecords()
        {
            Instance = this;
        }

        public override void Load()
        {
            Everest.Events.OuiJournal.OnEnter += JournalOnEnter;
            Everest.Events.LevelEnter.OnGo += LevelEnterOnGo;
            Everest.Events.Player.OnDie += PlayerOnDie;
            Everest.Events.Level.OnTransitionTo += LevelOnTransitionTo;

            Type t_MoreRecords = GetType();
            orig_RegisterCompletion = m_RegisterCompletion.Detour<d_RegisterCompletion>(t_MoreRecords.GetMethod("RegisterCompletion"));
            orig_Update = m_Update.Detour<d_Update>(t_MoreRecords.GetMethod("Update"));
        }

        public override void Unload()
        {
            RuntimeDetour.Undetour(m_RegisterCompletion);
        }

        private void JournalOnEnter(OuiJournal journal, Oui from)
        {
            journal.Pages.Add(new OuiJournalDeathless(journal));
        }

        private void LevelEnterOnGo(Session session, bool fromSaveData)
        {
            if (fromSaveData)
            {
                // TODO: save current deathless time in file
            }
            else
            {
                LastCheckpointTime = 0L;
                DeathlessTime = 0L;
            }
        }

        private void PlayerOnDie(Player player)
        {
            Level level = Engine.Scene as Level;
            LastCheckpointTime = level.Session.Time;
        }

        private void LevelOnTransitionTo(LevelData next, Vector2 direction)
        {
            Level level = Engine.Scene as Level;
            DeathlessTime += level.Session.Time - LastCheckpointTime;
            LastCheckpointTime = level.Session.Time;
        }

        public delegate void d_RegisterCompletion(SaveData self, Session session);
        public static d_RegisterCompletion orig_RegisterCompletion;
        public static void RegisterCompletion(SaveData self, Session session)
        {
            orig_RegisterCompletion(self, session);

            var module = MoreRecords.Instance;
            module.DeathlessTime += session.Time - module.LastCheckpointTime;
            long deathlessTime = module.DeathlessTime;
            var area = session.Area;
            var oldStats = MoreRecords.SaveData.Areas[area.ID].Modes[(int) area.Mode].Clone();

            if (session.StartedFromBeginning)
			{
				if (oldStats.TheoryTime <= 0L || deathlessTime < oldStats.TheoryTime)
					oldStats.TheoryTime = deathlessTime;
				if (area.Mode == AreaMode.Normal && session.FullClear)
				{
					if (oldStats.TheoryFullClearTime <= 0L || deathlessTime < oldStats.TheoryFullClearTime)
						oldStats.TheoryFullClearTime = deathlessTime;
				}
			}
            MoreRecords.SaveData.Areas[area.ID].Modes[(int) area.Mode] = oldStats.Clone();
        }

        public delegate void d_Update(Level self);
        public static d_Update orig_Update;
        public static void Update(Level self)
        {
            orig_Update(self);

            var module = MoreRecords.Instance;
            var currRespawnPoint = self.Session.RespawnPoint;

            if (module.LastRespawnPoint != currRespawnPoint)
            {
                module.LastRespawnPoint = currRespawnPoint;
                if (currRespawnPoint != null)
                {
                    module.DeathlessTime += self.Session.Time - module.LastCheckpointTime;
                    module.LastCheckpointTime = self.Session.Time;
                }
            }
        }
    }
}