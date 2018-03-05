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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace puzzle {
	/// <summary>
	/// MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

			picSizes[3] = 96;
			picSizes[5] = 66;
			picSizes[9] = 36;

			factory = new BlockFactory(this);
		}

		private int[] picSizes = new int[10];
		private BlockFactory factory;
		private GameControl game = new GameControl();
		private int picWidth;
		private int margin = 2;
		private int offset;
		private const double duration = 0.3;

		private void NewGame(int size) {
			foreach (var c in board.Children)
				factory.DeleteBlock((TextBlock)c);

			picWidth = picSizes[size];
			margin = 2;
			offset = picWidth + 2 * margin;
			int count = size * size;
			game.NewGame(size);

			board.Children.Clear();
			for (int i = 0; i < count - 1; i++) {
				TextBlock tb = factory.CreateBlock(game.BlockStatus[i], picWidth);
				int x = i / size;
				int y = i % size;
				Canvas.SetLeft(tb, y * offset);
				Canvas.SetTop(tb, x * offset);
				board.Children.Add(tb);
			}

			this.Width = (picWidth + margin * 2) * size + 18;
			this.Height = (picWidth + margin * 2) * size + 191;
		}

		private void NewGame(object sender, RoutedEventArgs e) {
			int size = 0;
			MenuItem mi = (MenuItem)sender;
			if ((string)mi.Header == "3x3")
				size = 3;
			else if ((string)mi.Header == "5x5")
				size = 5;
			else if ((string)mi.Header == "9x9")
				size = 9;
			else
				size = 3;

			NewGame(size);
		}

		private void Move(int index, GameControl.Direction d) {
			TextBlock tb = (TextBlock)FindName("block" + index);
			if (tb.HasAnimatedProperties) {
				return;
			}

			double start = 0;
			double end = 0;
            bool isX = true;
			
			switch(d) {
				case GameControl.Direction.Left:
					start = Canvas.GetLeft(tb);
					end = start - offset;
					isX = false;
					break;
				case GameControl.Direction.Up:
					start = Canvas.GetTop(tb);
					end = start - offset;
					isX = true;
					break;
				case GameControl.Direction.Right:
					start = Canvas.GetLeft(tb);
					end = start + offset;
					isX = false;
					break;
				case GameControl.Direction.Down:
					start = Canvas.GetTop(tb);
					end = start + offset;
					isX = true;
					break;
				default:
					break;
			}

			DoubleAnimation animation = new DoubleAnimation();
			animation.From = start;
			animation.To = end;
			animation.Duration = TimeSpan.FromSeconds(duration);
			animation.Completed += Animation_Completed;
			Storyboard.SetTarget(animation, tb);

			if (isX)
				tb.BeginAnimation(Canvas.TopProperty, animation);
			else
				tb.BeginAnimation(Canvas.LeftProperty, animation);

			if (game.Do(d)) {
				MessageBox.Show("你赢了！");
			}
		}

		private void Animation_Completed(object sender, EventArgs e) {
			AnimationTimeline timeline = (sender as AnimationClock).Timeline;
			TextBlock tb = (TextBlock)Storyboard.GetTarget(timeline);
			double currentLeft = Canvas.GetLeft(tb);
			double currentTop = Canvas.GetTop(tb);
			tb.BeginAnimation(Canvas.LeftProperty, null);
			tb.BeginAnimation(Canvas.TopProperty, null);
			Canvas.SetLeft(tb, currentLeft);
			Canvas.SetTop(tb, currentTop);
		}

		private void Window_KeyDown(object sender, KeyEventArgs e) {
			if (game.GameStatus != GameControl.GameState.Running)
				return;
			int index = -1;
			if (e.Key == Key.Down) {
				index = game.GetNeighborBlockIndex(GameControl.Direction.Up);
				if (index != -1) {
					Move(game.BlockStatus[index], GameControl.Direction.Down);
				}
				e.Handled = true;
			} else if (e.Key == Key.Up) {
				index = game.GetNeighborBlockIndex(GameControl.Direction.Down);
				if (index != -1) {
					Move(game.BlockStatus[index], GameControl.Direction.Up);
				}
				e.Handled = true;
			} else if (e.Key == Key.Left) {
				index = game.GetNeighborBlockIndex(GameControl.Direction.Right);
				if (index != -1) {
					Move(game.BlockStatus[index], GameControl.Direction.Left);
				}
				e.Handled = true;
			} else if (e.Key == Key.Right) {
				index = game.GetNeighborBlockIndex(GameControl.Direction.Left);
				if (index != -1) {
					Move(game.BlockStatus[index], GameControl.Direction.Right);
				}
				e.Handled = true;
			}
		}
	}
}
