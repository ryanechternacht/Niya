using System;

namespace Niya
{
	public class ComputerPlayer : IPlayer
	{
		public string Name { get; private set; }
		public IPlayer Opponent { get; set; }

		public ComputerPlayer ()
		{
			Name = "Computer";
		}

		public RowCol GetNextMove(Board board)
		{
			return Logic.NextMove (board, this, Opponent);
		}

		public override string ToString ()
		{
			return Name;
		}
	}
}

