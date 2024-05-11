using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.PuzzleManagement.CrosswordPuzzles
{
	[Serializable]
	public class CrosswordTileInfo
	{
	    private int _x;
	    private int _y;
	
	    public int X { get => _x; set => _x = value; }
	    public int Y { get => _y; set => _y = value; }
	
	    public CrosswordTileInfo(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
		
		public override bool Equals(Object obj)
		{
			if(obj == null) return false;
			
			CrosswordTileInfo otherTile = obj as CrosswordTileInfo;
			if(otherTile == null)
			{
				return false;	
			}
			else
			{
				return this.X == otherTile.X && this.Y == otherTile.Y;
			}
		}
		
		public override int GetHashCode() {
	      return this.X ^ this.Y;
	   	}
		
		public bool Equals(CrosswordTileInfo t)
		{
			if(t == null)
			{
				return false;	
			}
			else
			{
				return this.X == t.X && this.Y == t.Y;
			}
		}
	}
}
