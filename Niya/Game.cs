using System;
using System.IO;

namespace Niya
{
	public class Game
	{
		public IPlayer P1 { get; set; }
		public IPlayer P2 { get; set; }
		public Board Board { get; set; }

		public Game (IPlayer p1, IPlayer p2)
		{
			P1 = p1;
			P2 = p2;
			Board = Board.MakeBoard ();
		}

		public void Play()
		{
			PrintBoard ();

			var p1turn = true;

			using (var log = new StreamWriter (
				string.Format("{0} moves.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")))) {

				while (Board.IsGameOver (P1) == GameEnd.NotDone && Board.IsGameOver(P2) == GameEnd.NotDone) {
					var move = null as RowCol;
					if (p1turn) {
						move = P1.GetNextMove (Board);
						if (IsLegal (Board, move)) {
							Board = Board.Play (P1, move);
							p1turn = false;
							log.WriteLine ("{0}: {1}", P1.Name, move);
						} else {
							Console.WriteLine ("Sorry, {0}, that's not legal. Try Again", P1.Name);
							continue;
						}
					} else {
						move = P2.GetNextMove (Board);
						if (IsLegal (Board, move)) {
							Board = Board.Play (P2, move);
							p1turn = true;
							log.WriteLine ("{0}: {1}", P2.Name, move);
						} else {
							Console.WriteLine ("Sorry, {0}, that's not legal. Try Again", P2.Name);
							continue;
						}
					}
					Console.WriteLine ("{0} moved at {1} {2} ({3})", 
						p1turn ? P2.Name : P1.Name, 
						move.Row + 1, 
						move.Col + 1, 
						Convert(Board.Tiles[move.Row, move.Col].TileType));
					PrintBoard ();
				}

				if (Board.IsGameOver (P1) == GameEnd.Win) 
				{
					Console.WriteLine ("Congrats {0} on your glorious victory", P1);
					log.WriteLine ("{0} wins", P1.Name);
				} 
				else if (Board.IsGameOver (P2) == GameEnd.Win) 
				{
					Console.WriteLine ("Congrats {0} on your glorious victory", P2);
					log.WriteLine ("{0} wins", P2.Name);
				} 
				else 
				{
					Console.WriteLine ("A tie! Well fought both players");
					log.WriteLine ("tie game");
				}
			}
		}

		public void PrintBoard()
		{
			for(int i = 0; i < 4; i++)
			{
				for(int j = 0; j < 4; j++)
				{
					var tile = Board.Tiles [i, j];
					Console.Write ("{0} {1}  ", Convert (tile.TileType), Convert (tile.Player));
				}
				Console.WriteLine ();
			}
		}

		private string Convert(TileType tt)
		{
			var s = "";
			if ((tt & TileType.Bird) > 0)
				s += "B";
			if ((tt & TileType.Cactus) > 0)
				s += "C";
			if ((tt & TileType.Coupon) > 0)
				s += "X";
			if ((tt & TileType.Flower) > 0)
				s += "F";
			if ((tt & TileType.Leaves) > 0)
				s += "L";
			if ((tt & TileType.Sun) > 0)
				s += "S";
			if ((tt & TileType.Tree) > 0)
				s += "T";
			if ((tt & TileType.Wind) > 0)
				s += "W";

			return s;
		}

		private string Convert(IPlayer p)
		{
			if (p == P1)
				return "1";
			else if (p == P2)
				return "2";
			else
				return " ";
		}

		private bool IsLegal(Board board, RowCol move)
		{
			foreach (var m in board.GetLegalMoves())
				if (m.Equals (move))
					return true;
			return false;
		}
	}
}

