using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MazeGame {
	/// <summary>
	/// NewGameWin.xaml 的交互逻辑
	/// </summary>
	public partial class NewGameWin : Window {
		int height;
		int width;
		MazeType mazeType;
		public int GameHeight {
			get {
				return height;
			}
		}
		public int GameWidth {
			get {
				return width;
			}
		}
		public MazeType MazeType {
			get {
				return mazeType;
			}
		}
		public NewGameWin() {
			InitializeComponent();
			MazeType[] types = (MazeType[])Enum.GetValues(typeof(MazeType));
			cbType.ItemsSource = types;
			cbType.SelectedIndex = 0;
		}

		private void Button_OK_Click(object sender, RoutedEventArgs e) {
			if (!int.TryParse(textHeight.Text, out height)) {
				MessageBox.Show("高度值非法！");
				return;
            }
			if (height <= 0 && height > 99) {
				MessageBox.Show("高度值非法！");
				return;
			}
			if (!int.TryParse(textWidth.Text, out width)) {
				MessageBox.Show("宽度值非法！");
				return;
			}
			if (width <= 0 && width > 99) {
				MessageBox.Show("宽度值非法！");
				return;
			}

			mazeType = (MazeType)cbType.SelectedIndex;
			DialogResult = true;
			Close();
		}

		private void Button_Cancel_Click(object sender, RoutedEventArgs e) {
			DialogResult = false;
			Close();
		}
	}
}
