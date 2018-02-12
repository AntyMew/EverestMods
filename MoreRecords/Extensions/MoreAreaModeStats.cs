namespace Celeste.Mod.MoreRecords
{
	public class MoreAreaModeStats
	{
		public long TheoryTime { get; set; } = 0L;
		public long TheoryFullClearTime { get; set; } = 0L;

		public MoreAreaModeStats Clone()
		{
			return new MoreAreaModeStats {
				TheoryTime = this.TheoryTime,
				TheoryFullClearTime = this.TheoryFullClearTime
			};
		}
	}
}