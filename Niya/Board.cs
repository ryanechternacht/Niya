using System;
using System.Collections.Generic;
using System.Linq;

namespace Niya
{
	public class Board
	{
		public Tile[,] Tiles { get; private set; }
		public Move LastMove { get { return Moves.LastOrDefault (); } }
		public List<Move> Moves { get; internal set; }


		private Board (Tile[,] tiles)
		{
			Tiles = tiles;
			Moves = new List<Move> ();
		}

		public static Board MakeBoard()
		{
			var tileList = ShuffleTiles (MakeTiles ()).ToList();
//			var tileList = MakeTiles().ToList(); // don't shuffle for testing
			var tiles = new Tile[4, 4];

			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++) 
				{
					var t = tileList [0];
					tileList.RemoveAt (0);
					tiles [i, j] = t;
				}

			return new Board (tiles);
		}

		public Board Play(IPlayer player, RowCol move)
		{
			var b = Clone ();

			var t = b.Tiles [move.Row, move.Col];
			if (t.Occupied)

				throw new Exception ("tile is occupied");

			t.Play (player);
			b.Moves.Add (new Move(player, move));

			return b;
		}

		public List<RowCol> GetLegalMoves()
		{
			if (LastMove == null) // we just started
				return new List<RowCol> 
				{
					new RowCol (0, 0), new RowCol (0, 1), new RowCol (0, 2), new RowCol (0, 3),
					new RowCol (1, 0), new RowCol (1, 1), new RowCol (1, 2), new RowCol (1, 3),
					new RowCol (2, 0), new RowCol (2, 1), new RowCol (2, 2), new RowCol (2, 3),
					new RowCol (3, 0), new RowCol (3, 1), new RowCol (3, 2), new RowCol (3, 3),
				};

			var last = Tiles[LastMove.RowCol.Row, LastMove.RowCol.Col];
			var list = new List<RowCol> ();
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++) 
				{
					var tile = Tiles [i, j];
					if (tile.Occupied)
						continue;
					if ((tile.TileType & last.TileType) > 0)
						list.Add (new RowCol (i, j));
				}

			return list;
		}

		public GameEnd IsGameOver(IPlayer player)
		{
			// check horizontals
			if (Tiles [0, 0].Player == player && Tiles [0, 1].Player == player
			   && Tiles [0, 2].Player == player && Tiles [0, 3].Player == player)
				return GameEnd.Win;
			if (Tiles [1, 0].Player == player && Tiles [1, 1].Player == player
				&& Tiles [1, 2].Player == player && Tiles [1, 3].Player == player)
				return GameEnd.Win;
			if (Tiles [2, 0].Player == player && Tiles [2, 1].Player == player
				&& Tiles [2, 2].Player == player && Tiles [2, 3].Player == player)
				return GameEnd.Win;
			if (Tiles [3, 0].Player == player && Tiles [3, 1].Player == player
				&& Tiles [3, 2].Player == player && Tiles [3, 3].Player == player)
				return GameEnd.Win;
					
			// check horizontals
			if (Tiles [0, 0].Player == player && Tiles [1, 0].Player == player
				&& Tiles [2, 0].Player == player && Tiles [3, 0].Player == player)
				return GameEnd.Win;
			if (Tiles [0, 1].Player == player && Tiles [1, 1].Player == player
				&& Tiles [2, 1].Player == player && Tiles [3, 1].Player == player)
				return GameEnd.Win;
			if (Tiles [0, 2].Player == player && Tiles [1, 2].Player == player
				&& Tiles [2, 2].Player == player && Tiles [3, 2].Player == player)
				return GameEnd.Win;
			if (Tiles [0, 3].Player == player && Tiles [1, 3].Player == player
				&& Tiles [2, 3].Player == player && Tiles [3, 3].Player == player)
				return GameEnd.Win;

			// check diagonals
			if (Tiles [0, 0].Player == player && Tiles [1, 1].Player == player
				&& Tiles [2, 2].Player == player && Tiles [3, 3].Player == player)

				return GameEnd.Win;
			if (Tiles [0, 3].Player == player && Tiles [1, 2].Player == player
				&& Tiles [2, 1].Player == player && Tiles [3, 0].Player == player)
				return GameEnd.Win;

			// check squares
			if (Tiles [0, 0].Player == player && Tiles [0, 1].Player == player
				&& Tiles [1, 0].Player == player && Tiles [1, 1].Player == player)
				return GameEnd.Win;
			if (Tiles [1, 0].Player == player && Tiles [1, 1].Player == player
				&& Tiles [2, 0].Player == player && Tiles [2, 1].Player == player)
				return GameEnd.Win;
			if (Tiles [2, 0].Player == player && Tiles [2, 1].Player == player
				&& Tiles [3, 0].Player == player && Tiles [3, 1].Player == player)
				return GameEnd.Win;
			
			if (Tiles [0, 1].Player == player && Tiles [0, 2].Player == player
				&& Tiles [1, 1].Player == player && Tiles [1, 2].Player == player)
				return GameEnd.Win;
			if (Tiles [1, 1].Player == player && Tiles [1, 2].Player == player
				&& Tiles [2, 1].Player == player && Tiles [2, 2].Player == player)
				return GameEnd.Win;
			if (Tiles [2, 1].Player == player && Tiles [2, 2].Player == player
				&& Tiles [3, 1].Player == player && Tiles [3, 2].Player == player)
				return GameEnd.Win;

			if (Tiles [0, 2].Player == player && Tiles [0, 3].Player == player
				&& Tiles [1, 2].Player == player && Tiles [1, 3].Player == player)
				return GameEnd.Win;
			if (Tiles [1, 2].Player == player && Tiles [1, 3].Player == player
				&& Tiles [2, 2].Player == player && Tiles [2, 3].Player == player)
				return GameEnd.Win;
			if (Tiles [2, 2].Player == player && Tiles [2, 3].Player == player
				&& Tiles [3, 2].Player == player && Tiles [3, 3].Player == player)
				return GameEnd.Win;

			if (!GetLegalMoves ().Any ())
				if (Moves.Count == 16)
					return GameEnd.Tie;
				else

				return LastMove.Player == player ? GameEnd.Win : GameEnd.Loss;

			return GameEnd.NotDone;
		}

		private Board Clone()
		{
			var newTiles = new Tile [4, 4];
			for (int i = 0; i < 4; i++)
				for (int j = 0; j < 4; j++)
					newTiles [i, j] = Tiles [i, j].Clone ();

			return new Board (newTiles) { Moves = Moves.ToList() };
		}

		private static IEnumerable<Tile> MakeTiles()
		{
			return new List<Tile> 
			{
				new Tile(TileType.Bird | TileType.Tree),
				new Tile(TileType.Leaves | TileType.Wind),
				new Tile(TileType.Bird | TileType.Flower),
				new Tile(TileType.Sun | TileType.Leaves),
				new Tile(TileType.Sun | TileType.Tree),
				new Tile(TileType.Wind | TileType.Flower),
				new Tile(TileType.Coupon | TileType.Flower),
				new Tile(TileType.Leaves | TileType.Bird),
				new Tile(TileType.Sun | TileType.Flower),
				new Tile(TileType.Wind | TileType.Cactus),
				new Tile(TileType.Coupon | TileType.Tree),
				new Tile(TileType.Cactus | TileType.Bird),
				new Tile(TileType.Coupon | TileType.Leaves),
				new Tile(TileType.Coupon | TileType.Cactus),
				new Tile(TileType.Wind | TileType.Tree),
				new Tile(TileType.Sun | TileType.Cactus),
			};
		}

		private static IEnumerable<Tile> ShuffleTiles(IEnumerable<Tile> tiles)
		{
			var list = new List<Tile> ();

			var t = tiles.ToList ();


			var rand = new Random ();
			while (t.Any ()) 
			{
				var i = rand.Next (t.Count);
				list.Add(t[i]);
				t.RemoveAt (i);
			}

			return list;
		}

		public class Move
		{
			public RowCol RowCol { get; set; }
			public IPlayer Player { get; set; }

			public Move(IPlayer player, RowCol rc)
			{
				RowCol = rc;
				Player = player;
			}
		}
	}
}

