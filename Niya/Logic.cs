using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Niya
{
	public static class Logic
	{
		public static RowCol NextMove (Board board, IPlayer me, IPlayer opp)
		{
			// this is similar to of MyMove but with logging and multithreading

			var moves = new Dictionary<RowCol, int> ();
			var legalMoves = board.GetLegalMoves ();

			int count = 0;
			Console.WriteLine ("Starting move assessment ({0} to assess)", legalMoves.Count ());
			var semaphore = new SemaphoreSlim (4);
			var tasks = new Task[legalMoves.Count ()];
			foreach (var move in legalMoves) 
			{
				var count1 = ++count;
				tasks[count1-1] = Task.Run (() => 
				{
					semaphore.Wait();
					Console.WriteLine ("{0} starting (option {1}/{2})", move, count1, legalMoves.Count ());

					var b = board.Play (me, move);
					
					var isGameOver = b.IsGameOver (me);
					int value;
					if (isGameOver == GameEnd.Win)
						value = int.MaxValue; // if we can win, take it now!
					else if (isGameOver == GameEnd.Tie)
						value = 0;
					else
						value = OppMove (b, me, opp);
					
					moves [move] = value;

					Console.WriteLine ("{0} scores {1} (option {2}/{3})", move, value, count1, legalMoves.Count ());
					semaphore.Release();
				});
			}
			Task.WaitAll (tasks);

			var bestMove = null as RowCol;
			int best = -1;
			foreach (var kvp in moves) 
				if (kvp.Value > best) 
				{
					best = kvp.Value;
					bestMove = kvp.Key;
				}
			if (bestMove != null)
				Console.WriteLine ("The best move is {0}, with a score of {1}", bestMove, best);
			else 
			{
				Console.WriteLine ("All moves should result in losing, choosing randomly");
				bestMove = legalMoves [new Random ().Next (legalMoves.Count)];
			}
			return bestMove;
		}

		private static int MyMove(Board board, IPlayer me, IPlayer opp)
		{
			var moves = new Dictionary<RowCol, int> ();
			var legalMoves = board.GetLegalMoves ();

			foreach(var move in legalMoves)
			{
				var b = board.Play (me, move);

				var isGameOver = b.IsGameOver (me);
				if (isGameOver == GameEnd.Win)
					return 1;
				else if (isGameOver == GameEnd.Tie)
					return 0;
				

				moves [move] = OppMove (b, me, opp);

			}
			return moves.Any (kvp => kvp.Value == -1) ? -1 : moves.Sum (kvp => kvp.Value);
		}

		private static int OppMove(Board board, IPlayer me, IPlayer opp)
		{
			var moves = new Dictionary<RowCol, int> ();
			var legalMoves = board.GetLegalMoves ();

			foreach(var move in legalMoves)
			{
				var b = board.Play (opp, move);

				var isGameOver = b.IsGameOver (opp);
				if (isGameOver == GameEnd.Win)
					return -1;
				else if (isGameOver == GameEnd.Tie)
					return 0;

				moves [move] = MyMove (b, me, opp);
			}

			return moves.All (kvp => kvp.Value == -1) ? -1 : moves.Sum (kvp => kvp.Value);
		}
	}
}

