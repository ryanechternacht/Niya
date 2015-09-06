using System;

namespace Niya
{
	public interface IPlayer
	{
		string Name { get; }

		RowCol GetNextMove(Board board);
	}
}

