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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace 面包管理系统 {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

			DataBaseWrapper.ConnectDataBase();
			Bread.BreadTypes = DataBaseWrapper.GetBreadTypeTable();

			StoreHouse.CreateStoreHouse(100);
			store = StoreHouse.GetStoreHouse();
		}

		private void RefreshData() {
			Bread.BreadTypes = DataBaseWrapper.GetBreadTypeTable();
		}

		private void ButtonShowStore_Click(object sender, RoutedEventArgs e) {
			RefreshData();
			StoreHouseWin shWin = new StoreHouseWin();
			shWin.Owner = this;
			shWin.SetData(store);
			shWin.ShowDialog();
		}

		private void ButtonSeal_Click(object sender, RoutedEventArgs e) {
			RefreshData();
			SealWin sWin = new SealWin();
			sWin.Owner = this;
			sWin.store = store;
			sWin.Show();
		}

		private void ButtonProduce_Click(object sender, RoutedEventArgs e) {
			RefreshData();
			ProduceWin pWin = new ProduceWin();
			pWin.Owner = this;
			pWin.store = store;
			pWin.ShowDialog();
		}

		private void ButtonProduceNew_Click(object sender, RoutedEventArgs e) {
			RefreshData();
			ProduceNewTypeWin pWin = new ProduceNewTypeWin();
			pWin.Owner = this;
			pWin.ShowDialog();
		}

		StoreHouse store;

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			DataBaseWrapper.CloseDataBase();
        }
	}
}
