using System;

namespace Celeste.Mod.MoreRecords
{
	public class MoreAreaStats
	{
		public int ID { get; set; }
		public MoreAreaModeStats[] Modes { get; set; }

		public MoreAreaStats() : this(0) { }

		public MoreAreaStats(int id)
		{
			this.ID = id;
			int length = Enum.GetValues(typeof(AreaMode)).Length;
			this.Modes = new MoreAreaModeStats[length];
			for (int i = 0; i < Modes.Length; i++)
				this.Modes[i] = new MoreAreaModeStats();
		}

		public MoreAreaStats Clone()
		{
			var stats = new MoreAreaStats(this.ID);
			for (int i = 0; i < Modes.Length; i++)
				stats.Modes[i] = this.Modes[i];
			return stats;
		}
	}
}