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

namespace GuessNumber {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {

		public MainWindow() {
			InitializeComponent();
			var binding = new CommandBinding(ApplicationCommands.New);
			binding.Executed += NewGame;
			CommandBindings.Add(binding);
			binding = new CommandBinding(ApplicationCommands.Close);
			binding.Executed += AppClose;
			CommandBindings.Add(binding);

			newGame();
			textbox1.Focus();
        }

		private void newGame() {
			GenerateNums();
			imgPanel.Visibility = Visibility.Hidden;
			statusPanel.Visibility = Visibility.Visible;
			statusLabel.Text = startGame;
		}

		private void NewGame(object sender, ExecutedRoutedEventArgs e) {
			newGame();
        }

		private void GenerateNums() {
			Random r = new Random();
			int i = 0;
			while (i < 4) {
				nums[i] = r.Next(0, 9);
				int j = i-1;
				while (j >= 0 && nums[j--] != nums[i]);
				if (j == -1) i++;
			}
		}

		private void AppClose(object sender, ExecutedRoutedEventArgs e) {
			Close();
		}

		private int[] nums = new int[4];
		private static readonly string startGame = "开始猜数字吧!";
		private static readonly string winGame = "恭喜你获得胜利!";

		private void button_Click(object sender, RoutedEventArgs e) {
			JudgeGame();
		}

		private TextBox GetInnerTextbox(TextBox textbox) {
			TextBox innerTextbox = (TextBox)textbox.Template.FindName("textBox", textbox);
			return innerTextbox;
		}

		private string GetTextFromTextbox(TextBox textbox) {
			return GetInnerTextbox(textbox).Text;
		}

		private void JudgeGame() {
			imgPanel.Visibility = Visibility.Visible;
			statusPanel.Visibility = Visibility.Hidden;
			int[] judgeNums = new int[4];
			int correctNum = 0;
			judgeNums[0] = Convert.ToInt32(GetTextFromTextbox(textbox1));
			judgeNums[1] = Convert.ToInt32(GetTextFromTextbox(textbox2));
			judgeNums[2] = Convert.ToInt32(GetTextFromTextbox(textbox3));
			judgeNums[3] = Convert.ToInt32(GetTextFromTextbox(textbox4));

			if (judgeNums[0] == nums[0]) {
				img1.Source = new BitmapImage(new Uri("correct.bmp", UriKind.Relative));
				correctNum++;
			} else
				img1.Source = new BitmapImage(new Uri("error.bmp", UriKind.Relative));

			if (judgeNums[1] == nums[1]) {
				img2.Source = new BitmapImage(new Uri("correct.bmp", UriKind.Relative));
				correctNum++;
			} else
				img2.Source = new BitmapImage(new Uri("error.bmp", UriKind.Relative));

			if (judgeNums[2] == nums[2]) {
				img3.Source = new BitmapImage(new Uri("correct.bmp", UriKind.Relative));
				correctNum++;
			} else
				img3.Source = new BitmapImage(new Uri("error.bmp", UriKind.Relative));

			if (judgeNums[3] == nums[3]) {
				img4.Source = new BitmapImage(new Uri("correct.bmp", UriKind.Relative));
				correctNum++;
			} else
				img4.Source = new BitmapImage(new Uri("error.bmp", UriKind.Relative));

			if (correctNum == 4) {
				imgPanel.Visibility = Visibility.Hidden;
				statusPanel.Visibility = Visibility.Visible;
				statusLabel.Text = winGame;
			}
		}

		private static bool IsNumKey(KeyEventArgs e) {
			if (e.Key == Key.Tab)
				return true;

			if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)) {
				return true;
			} else if ((e.Key >= Key.D0 && e.Key <= Key.D9) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift) {
				return true;
			} else
				return false;
		}

		private void DealKey(object sender, KeyEventArgs e) {
			TextBox t = (TextBox)sender;
			TextBox textBox = (TextBox)t.Template.FindName("textBox", t);
			if (IsNumKey(e)) {
				if (e.Key != Key.Tab)
					textBox.Text = "";
				e.Handled = false;
			} else
				e.Handled = true;
		}

		private void textBox_KeyDown(object sender, KeyEventArgs e) {
			DealKey(sender, e);
			if (e.Key == Key.Enter) {
				JudgeGame();
				e.Handled = true;
			}
		}

		private void textbox_GotFocus(object sender, RoutedEventArgs e) {
			TextBox tb = GetInnerTextbox((TextBox)sender);
			if (tb != null)
				tb.Focus();
        }

		private void textbox1_KeyUp(object sender, KeyEventArgs e) {
			if (IsNumKey(e))
				textbox2.Focus();
		}

		private void textbox2_KeyUp(object sender, KeyEventArgs e) {
			if (IsNumKey(e))
				textbox3.Focus();
		}

		private void textbox3_KeyUp(object sender, KeyEventArgs e) {
			if (IsNumKey(e))
				textbox4.Focus();
		}

		private void textbox4_KeyUp(object sender, KeyEventArgs e) {
			if (IsNumKey(e))
				textbox1.Focus();
		}
	}
}
