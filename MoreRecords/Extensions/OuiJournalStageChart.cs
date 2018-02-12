using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste.Mod.MoreRecords
{
	public class OuiJournalStageChart : OuiJournalPage
	{
		private Table table;
		protected Vector2 justify;
		protected float scale;
		protected Color color;

		protected virtual Cell[] GenerateHeader()
		{
			var headers = new List<Cell>();
			headers.Add(new TextCell(Dialog.Clean("journal_mode_normal"), justify, scale + 0.1f, color, 240f));
			headers.Add(new TextCell(Dialog.Clean("journal_mode_normal_fullclear"), justify, scale + 0.1f, color, 240f));
			if (SaveData.Instance.UnlockedModes >= 2)
				headers.Add(new TextCell(Dialog.Clean("journal_mode_bside"), justify, scale + 0.1f, color, 240f));
			if (SaveData.Instance.UnlockedModes >= 3)
				headers.Add(new TextCell(Dialog.Clean("journal_mode_cside"), justify, scale + 0.1f, color, 240f));
			return headers.ToArray();
		} 

		protected virtual Cell[] GenerateStats(AreaData area) => null;
		protected virtual Cell[] GenerateTotals(List<AreaData> areas) => null;
		protected virtual Cell GenerateGrandTotal(List<AreaData> areas) => null;

		public OuiJournalStageChart(OuiJournal journal) : this(journal, "placeholder") { }

		public OuiJournalStageChart(OuiJournal journal, string title) : base(journal)
		{
			this.PageTexture = "page";
			justify = new Vector2(0.5f, 0.5f);
			scale = 0.5f;
			color = Color.Black * 0.6f;
			Initialize(title);
		}

		private void Initialize(string title)
		{
			table = new Table();
			table.AddColumn(new TextCell(Dialog.Clean($"morerecs_journal_{title}"), new Vector2(1f, 0.5f), 0.7f, Color.Black * 0.7f));

			Cell[] header = GenerateHeader();
			if (header != null)
			{
				foreach (Cell cell in header)
				{
					if (cell != null)
						table.AddColumn(cell);
				}
			}

			var areas = new List<AreaData>();

			foreach (AreaStats data in SaveData.Instance.Areas)
			{
				AreaData area = AreaData.Get(data.ID);

				if (area.Interlude)
					continue;
				if (area.ID > SaveData.Instance.UnlockedAreas)
					break;

				areas.Add(area);

				Row row = table.AddRow();
				row.Add(new TextCell(Dialog.Clean(area.Name), new Vector2(1f, 0.5f), scale + 0.1f, color));

				Cell[] stats = GenerateStats(area);
				if (stats != null)
				{
					foreach (Cell cell1 in stats)
					{
						if (cell1 != null)
							row.Add(cell1);
						else
							row.Add(new IconCell("dot"));
					}
				}
			}
			
			Cell[] totals = GenerateTotals(areas);
			if (totals != null)
			{
				Row row2 = table.AddRow();
				row2.Add(new TextCell(Dialog.Clean("journal_totals"), new Vector2(1f, 0.5f), scale + 0.3f, color));
				foreach (Cell cell2 in totals)
				{
					if (cell2 != null)
						row2.Add(cell2);
					else
						row2.Add(new IconCell("dot"));
				}
			}

			Cell grandTotal = GenerateGrandTotal(areas);
			if (grandTotal != null)
			{
				grandTotal.SpreadOverColumns = 4;
				Row row3 = table.AddRow();
				row3.Add(new TextCell(Dialog.Clean("journal_grandtotal"), new Vector2(1f, 0.5f), scale + 0.3f, color));
				row3.Add(grandTotal);
			}
		}

		private void RenderStamps()
		{
			if (SaveData.Instance.AssistMode)
				GFX.Gui["fileselect/assist"].DrawCentered(new Vector2(1250f, 810f), Color.White * 0.5f, 1f, 0.2f);
			if (SaveData.Instance.CheatMode)
				GFX.Gui["fileselect/cheatmode"].DrawCentered(new Vector2(1400f, 860f), Color.White * 0.5f, 1f, 0f);
		}

		public override void Redraw(VirtualRenderTarget buffer)
		{
			base.Redraw(buffer);
			Draw.SpriteBatch.Begin();
			table.Render(new Vector2(60f, 20f));
			RenderStamps();
			Draw.SpriteBatch.End();
		}
	}
}