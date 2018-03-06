using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 面包管理系统 {
	/// <summary>
	/// 面包制作窗口
	/// </summary>
	public partial class ProduceWin : Window {
		public ProduceWin() {
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e) {
			if (typeList.SelectedIndex < 0)
				return;
			store.Add(Producer.ProduceBreads(Bread.BreadTypes[typeList.SelectedIndex + 1].Type, int.Parse(breadCount.Text)));
			MessageBox.Show("制造完成！");
		}

		private List<string> typeNames = new List<string>();

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			for (int i = 1; i < Bread.BreadTypes.Length; i++) {
				typeNames.Add(Bread.BreadTypes[i].Name);
			}
			typeList.ItemsSource = typeNames;
			typeList.SelectedIndex = 0;
		}

		public StoreHouse store;

		private void Button_Click_1(object sender, RoutedEventArgs e) {
			Close();
		}
	}
}
