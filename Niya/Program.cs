using System;

namespace Niya
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var human = new HumanPlayer ("P1");
			var computer = new ComputerPlayer () { Opponent = human };

			var game = new Game (computer, human);

			game.Play ();
		}
	}
}
