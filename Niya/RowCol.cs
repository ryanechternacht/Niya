using System;

namespace Niya
{
	public class RowCol : IEquatable<RowCol>
	{
		public int Row { get; set; }
		public int Col { get; set; }

		public RowCol ()
		{
		}

		public RowCol(int row, int col)
		{
			Row = row;
			Col = col;
		}

		#region IEquatable implementation
		public bool Equals (RowCol other)
		{
			return other.Row == Row && other.Col == Col;
		}

		public override bool Equals (object obj)
		{
			return obj is RowCol ? Equals ((RowCol)obj) : false;
		}

		public override int GetHashCode ()
		{
			return (Row << 16).GetHashCode () ^ Col.GetHashCode ();
		}

		public override string ToString ()
		{
			return string.Format ("[Row={0}, Col={1}]", Row, Col);
		}
		#endregion
	}
}

