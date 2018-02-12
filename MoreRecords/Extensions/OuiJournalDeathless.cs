using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.MoreRecords
{
	public class OuiJournalDeathless : OuiJournalStageChart
	{
		private long[][] modes;
		private long[][] Modes
		{
			get
			{
				if (modes == null)
				{
					modes = new long[4][];
					var areas = MoreRecords.SaveData.Areas;
					for (int i = 0; i < 4; i++)
						modes[i] = new long[areas.Count];
					for (int i = 0; i < areas.Count; i++)
					{
						modes[0][i] = areas[i].Modes[0].TheoryTime;
						modes[0][i] = areas[i].Modes[0].TheoryFullClearTime;
						modes[0][i] = areas[i].Modes[1].TheoryTime;
						modes[0][i] = areas[i].Modes[2].TheoryTime;
					}
				}
				return modes;
			}
		}
		private long[] totals;
		private long[] Totals
		{
			get
			{
				if (totals == null)
				{
					totals = new long[4];
					for (int i = 0; i < Modes.Length; i++)
					{
						if (Modes[i].All(time => time > 0L))
							totals[i] = Modes[i].Sum();
						else
							totals[i] = 0L;
					}
				}
				return totals;
			}
		}

		public OuiJournalDeathless(OuiJournal journal) : base(journal, "deathless") { }

		protected override Cell[] GenerateStats(AreaData area)
		{
			var stats = MoreRecords.SaveData.Areas[area.ID];
			return new Cell[] {
				stats.Modes[0].TheoryTime > 0L ? new TextCell(Dialog.Time(stats.Modes[0].TheoryTime), justify, scale, color) : null,
				stats.Modes[0].TheoryFullClearTime > 0L ? new TextCell(Dialog.Time(stats.Modes[0].TheoryFullClearTime), justify, scale, color) : null,
				stats.Modes[1].TheoryTime > 0L ? new TextCell(Dialog.Time(stats.Modes[1].TheoryTime), justify, scale, color) : null,
				stats.Modes[2].TheoryTime > 0L ? new TextCell(Dialog.Time(stats.Modes[2].TheoryTime), justify, scale, color) : null
			};
		}

		protected override Cell[] GenerateTotals(List<AreaData> areas)
		{
			return Totals.Select(time => time > 0L ? new TextCell(Dialog.Time(time), justify, scale, color) : null).ToArray();
		}

		protected override Cell GenerateGrandTotal(List<AreaData> areas)
		{
			if (Totals.All(time => time > 0L))
				return new TextCell(Dialog.Time(Totals.Sum()), justify, scale, color);
			else
				return null;
		}
	}
}