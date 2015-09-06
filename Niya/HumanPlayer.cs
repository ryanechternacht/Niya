using System;

namespace Niya
{
	public class HumanPlayer : IPlayer
	{
		public string Name { get; private set; }

		public HumanPlayer (string name)
		{
			Name = name;
		}

		public RowCol GetNextMove(Board board)
		{
			Console.WriteLine ("{0}, What's your next move? (ex: \"2 3\" to play in the 2nd row and 3rd column)", Name);
			var move = Console.ReadLine ();
			var tokens = move.Split (' ');
			return new RowCol (int.Parse (tokens [0]) - 1, int.Parse (tokens [1]) - 1);
		}

		public override string ToString ()
		{
			return Name;
		}
	}
}

