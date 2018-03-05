using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace puzzle {
	/// <summary>
	/// 创建拼图块
	/// </summary>
	class BlockFactory {
		private FrameworkElement win;
		public BlockFactory(FrameworkElement win) {
			this.win = win;
		}
		public TextBlock CreateBlock(int index, double size) {
			TextBlock tb = new TextBlock();
			tb.Style = (Style)win.FindResource("piece");
			tb.Text = (index).ToString();
			tb.Width = size;
			tb.Height = size;
			tb.FontSize = size / 100 * 75;
			tb.Name = "block" + index.ToString();
			win.RegisterName(tb.Name, tb);

			return tb;
		}

		public void DeleteBlock(TextBlock tb) {
			if (tb.Name != "")
				win.UnregisterName(tb.Name);	
		}
	}
}
