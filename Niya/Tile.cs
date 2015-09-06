using System;

namespace Niya
{
	public class Tile
	{
		public IPlayer Player { get; private set; }
		public bool Occupied { get { return Player != null; } }
		public TileType TileType { get; set; }

		public Tile (TileType tileType)
		{
			TileType = tileType;
		}

		private Tile(TileType tileType, IPlayer player)
		{
			TileType = tileType;
			Player = player;
		}

		public void Play(IPlayer player)
		{
			if (Occupied)
				throw new Exception ("I'm Occupied");
			Player = player;
		}

		public Tile Clone() 
		{
			return new Tile (TileType, Player);
		}
	}
}

