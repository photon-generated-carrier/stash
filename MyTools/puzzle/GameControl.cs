using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace puzzle {
	/// <summary>
	/// 游戏逻辑控制
	/// </summary>
	class GameControl {
		private int blankX;
		public int BlankX {
			get {
				return blankX;
			}
		}

		private int blankY;
		public int BlankY {
			get {
				return blankY;
			}
		}

		private List<int> blockStatus = new List<int>();
		public List<int> BlockStatus {
			get {
				return blockStatus;
			}
		} // 各格子状态

		private GameState gameStatus = GameState.Idle;
		public GameState GameStatus {
			get {
				return gameStatus;
			}
		}

		private int size;

		public void NewGame(int size) {
			this.size = size;
			int[] array = RandomArray(size);
			blockStatus.Clear();
			foreach (int c in array)
				blockStatus.Add(c);
			blankX = size - 1;
			blankY = size - 1;
			gameStatus = GameState.Running;
		}

		public int BlankIndex {
			get {
				return BlankX * size + BlankY;
			}
		}

		public int GetNeighborBlockIndex(Direction d) {
			switch (d) {
				case Direction.Left:
					if (BlankY == 0)
						return -1;
					return BlankIndex - 1;
				case Direction.Up:
					if (BlankX == 0)
						return -1;
					return BlankIndex - size;
				case Direction.Right:
					if (BlankY == size - 1)
						return -1;
					return BlankIndex + 1;
				case Direction.Down:
					if (BlankX == size - 1)
						return -1;
					return BlankIndex + size;
				default:
					return -1;
			}
		}

		private void SwapWithBlank(int x, int y) {
			int indexBlock = x * size + y;
			int indexBlank = BlankX * size + BlankY;
			// 复杂写法
			//int indexMin, indexMax;
			//if (indexBlank > indexBlock) {
			//	indexMax = indexBlank;
			//	indexMin = indexBlock;
			//} else {
			//	indexMax = indexBlock;
			//	indexMin = indexBlank;
			//}

			//var temp = blockStatus[indexMax];
			//blockStatus.RemoveAt(indexMax);
			//blockStatus.Insert(indexMin, temp);
			//temp = blockStatus[indexMin + 1];
			//blockStatus.RemoveAt(indexMin + 1);
			//blockStatus.Insert(indexMax, temp);

			int temp = blockStatus[indexBlock];
			blockStatus[indexBlock] = blockStatus[indexBlank];
			blockStatus[indexBlank] = temp;
			blankX = x;
			blankY = y;
		}

		// 返回游戏是否胜利
		public bool Do(Direction d) {
			if (gameStatus != GameState.Running)
				return false;

			switch(d) {
				case Direction.Left:
					if (BlankY == size - 1)
						return false;
					SwapWithBlank(BlankX, BlankY + 1);
					break;
				case Direction.Up:
					if (BlankX == size - 1)
						return false;
					SwapWithBlank(BlankX + 1, BlankY);
					break;
				case Direction.Right:
					if (BlankY == 0)
						return false;
					SwapWithBlank(BlankX, BlankY - 1);
					break;
				case Direction.Down:
					if (BlankX == 0)
						return false;
					SwapWithBlank(BlankX - 1, BlankY);
					break;
				default:
					return false;
			}

			return Judge();
		}

		private bool Judge() {
			for (int i = 0; i < size * size - 1; i++) {
				if (blockStatus[i] != i + 1)
					return false;
			}

			gameStatus = GameState.Win;
			return true;
		}

		static private bool HasSolution(int[] array) {
			// 计算逆序数奇偶
			int index = 0;
			int ans = 0;
			for (int i = 0; i < array.Length; i++) {
				if (array[i] == 0) {
					index = i;
					continue;
				}
				for (int j = i + 1; j < array.Length; j++) {
					if (array[j] == 0)
						continue;
					if (array[j] < array[i])
						ans ^= 1;
				}
			}
			return ans == 0;
		}
		static private int[] RandomArray(int size) {
			int[] array = new int[size * size];
			for (int i = 0; i < size * size - 1; i++)
				array[i] = i + 1;
			Random rnd = new Random();

			// 由结果倒退初始状态，随机效果很差
			//int x = size - 1;
			//int y = size - 1;
			//int[] dx = { -1, 0, 1, 0 };
			//int[] dy = { 0, 1, 0, -1 };
			//int times = size * size * size * size;
			//while (times > 0) {
			//	int d = rnd.Next(0, 4);
			//	int dstX = dx[d] + x;
			//	int dstY = dy[d] + y;
			//	if ((0 <= dstX && dstX < size) && (0 <= dstY && dstY < size)) {
			//		int temp = array[x * size + y];
			//		array[x * size + y] = array[dstX * size + dstY];
			//		array[dstX * size + dstY] = temp;
			//		x = dstX;
			//		y = dstY;
			//		times--;
			//	}
			//}
			//int index = x * size + y;
			//while(x < size - 1) {
			//	int temp = array[index];
			//	array[index] = array[index + size];
			//	array[index + size] = temp;
			//	x++;
			//	index += size;
			//}
			//while (y < size - 1) {
			//	int temp = array[index];
			//	array[index] = array[index + 1];
			//	array[index + 1] = temp;
			//	y++;
			//	index++;
			//}

			// 随机交换生成数据
			int min = 0;
			int max = size * size - 1;
			int index = 0;
			while (min != max) {
				int r = rnd.Next(min++, max);

				int temp = array[index];
				array[index++] = array[r];
				array[r] = temp;
			}

			if (!HasSolution(array)) {
				//改变逆序数奇偶性
				int temp = array[array.Length - 3];
				array[array.Length - 3] = array[array.Length - 2];
				array[array.Length - 2] = temp;
			}

			return array;
		}

		public enum Direction {
			Left = 0,
			Up,
			Right,
			Down
		}

		public enum GameState {
			Idle = 0,
			Running,
			Win
		}
	}
}
