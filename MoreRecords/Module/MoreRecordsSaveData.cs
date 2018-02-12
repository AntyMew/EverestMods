using System;
using System.Collections.Generic;

namespace Celeste.Mod.MoreRecords
{
	public class MoreRecordsSaveData : EverestModuleSaveData
	{
		public List<MoreAreaStats> Areas { get; set; } = new List<MoreAreaStats>();

		public MoreRecordsSaveData()
		{
			while (Areas.Count < AreaData.Areas.Count)
				Areas.Add(new MoreAreaStats(Areas.Count));
		}
	}
}