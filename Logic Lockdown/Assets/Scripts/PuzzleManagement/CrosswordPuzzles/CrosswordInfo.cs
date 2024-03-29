using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.PuzzleManagement.CrosswordPuzzles
{
	public class CrosswordInfo : IComparable
	{
	    public enum Direction
	    {
	        Horizontal,
	        Vertical
	    }
	
	    private string _word;
	    private string _clue;
	    private CrosswordTileInfo _startingPosition;
	    private Direction _wordDirection;
	
	    public string Word { get => _word; set => _word = value; }
	    public string Clue { get => _clue; set => _clue = value; }
	    public CrosswordTileInfo StartingPosition { get => _startingPosition; set => _startingPosition = value; }
	    public Direction WordDirection { get => _wordDirection; set => _wordDirection = value; }
	    public int Size
	    {
	        get => _word.Length;
	    }
	
	    public CrosswordInfo(string word, string clue){
			this.Word = word;
			this.Clue = clue;
			this.WordDirection = Direction.Horizontal;
			this.StartingPosition = new CrosswordTileInfo(0,0);
		}
		
		public CrosswordInfo(CrosswordInfo previous){
			this.Word = previous.Word;
			this.Clue = previous.Clue;
			this.WordDirection = previous.WordDirection;
			this.StartingPosition = new CrosswordTileInfo(previous.StartingPosition.X, previous.StartingPosition.Y);
		}
	
	    public int CompareTo(object obj)
	    {
	        if(obj == null) return 1;
			
			CrosswordInfo otherCrossWord = obj as CrosswordInfo;
			if(otherCrossWord != null)
			{
				return this.Size.CompareTo(otherCrossWord.Size);	
			}
			else
			{
				throw new ArgumentException("Object is not a CrossedWord");	
			}
	    }
	
	    // Give the letter at given Tile position, '0' if this word is not over this tile (used for a more simple utilisation of this class)
		public char LetterOnTile(CrosswordTileInfo t)
		{
			if(isWordOverTile(t))
			{
				switch(this.WordDirection)
				{
					case Direction.Horizontal : return this.Word[t.X - this.StartingPosition.X];
					case Direction.Vertical : return this.Word[t.Y - this.StartingPosition.Y];
					default : return '0';
				}
			}
			else
			{
				return '0';	
			}
		}
		
		// Give the tile under the "pos" position of the word string
		public CrosswordTileInfo TileAtWordPosition(int pos)
		{
			if(pos >= 0 && pos < this.Size)
			{
				switch(this.WordDirection)
				{
					case Direction.Horizontal : return new CrosswordTileInfo(this.StartingPosition.X + pos, this.StartingPosition.Y);
					case Direction.Vertical : return new CrosswordTileInfo(this.StartingPosition.X, this.StartingPosition.Y + pos);
					default : throw new MissingMemberException("This Word has no direction");
				}
			}
			else
			{
				throw new ArgumentOutOfRangeException();	
			}
		}
		
		// Tells us if one of the words letter is on the given Tile
		public bool isWordOverTile(CrosswordTileInfo t)
		{
			return (this.WordDirection == Direction.Horizontal && t.Y == this.StartingPosition.Y && t.X >= this.StartingPosition.X && t.X < this.StartingPosition.X + this.Size) 
				|| (this.WordDirection == Direction.Vertical && t.X == this.StartingPosition.X && t.Y >= this.StartingPosition.Y && t.Y < this.StartingPosition.Y + this.Size) ;
		}
		
		// An array wich gave us for each letter of c (e.g. array[0] for the first letter) the tiles on which the current instance has the same letter
		public List<CrosswordTileInfo>[] SimilarLetterTiles(CrosswordInfo c)
		{
			List<CrosswordTileInfo>[] tilesForEachLetter = new List<CrosswordTileInfo>[c.Size];
			for(int i = 0; i< c.Size ; i++)
			{
				List<CrosswordTileInfo> TilesForCurrentLetter = new List<CrosswordTileInfo>();
				for(int j = 0; j<this.Size; j++)
				{
					if(c.Word[i] == this.Word[j])
						TilesForCurrentLetter.Add(this.TileAtWordPosition(j));
				}
				tilesForEachLetter[i] = TilesForCurrentLetter;
			}
			return tilesForEachLetter;
		}
	
	    // Tells if instance can accept the crossword (no superposition...)
		public int CanAccept(CrosswordInfo c)
		{
			// BOTH HORIZONTAL 
			if(this.WordDirection == Direction.Horizontal && c.WordDirection == Direction.Horizontal )
			
				// Having more than 1 line between them
				if( Math.Abs(c.StartingPosition.Y - this.StartingPosition.Y) > 1 )
					return 0;
			
			// Having less than 1 line between them but not touching nor supersposing
			if( Math.Abs(c.StartingPosition.Y - this.StartingPosition.Y) <= 1 && (this.StartingPosition.X > c.StartingPosition.X + c.Size || this.StartingPosition.X + this.Size < c.StartingPosition.X)) 
				return 2;
							
			// BOTH VERTICAL 
			if(this.WordDirection == Direction.Vertical && c.WordDirection == Direction.Vertical )
			
				// Having more than 1 row between them
				if( Math.Abs(c.StartingPosition.X - this.StartingPosition.X) > 1  ) 
					return 0;
			
			// Having less than 1 row between them but not touching nor supersposing
			if ( Math.Abs(c.StartingPosition.X - this.StartingPosition.X)  <= 1  && (this.StartingPosition.Y > c.StartingPosition.Y + c.Size || this.StartingPosition.Y + this.Size < c.StartingPosition.Y))
				return 2;
							
			// INSTANCE HORIZONTAL AND OTHER VERTICAL
			if(this.WordDirection == Direction.Horizontal && c.WordDirection == Direction.Vertical)
			{
				CrosswordTileInfo potentialIntersection = new CrosswordTileInfo(c.StartingPosition.X, this.StartingPosition.Y);
				
				char instanceChar = this.LetterOnTile(potentialIntersection);
				char otherChar = c.LetterOnTile(potentialIntersection);
				// IF CROSSING ON THE SAME LETTER --> TRUE
				if(this.isWordOverTile(potentialIntersection) && c.isWordOverTile(potentialIntersection) && instanceChar == otherChar)
				{
					if(instanceChar != '0')
						return 1;
					else
						return 0;
				}
				else if(instanceChar == '0' && (potentialIntersection.X < this.StartingPosition.X - 1 || potentialIntersection.X > this.StartingPosition.X + this.Size))
				{
					return 0;
				}
			}
			
			// INSTANCE VERTICAL AND OTHER HORIZONTAL
			if(this.WordDirection == Direction.Vertical && c.WordDirection == Direction.Horizontal)
			{
				CrosswordTileInfo potentialIntersection = new CrosswordTileInfo(this.StartingPosition.X, c.StartingPosition.Y);
				
				char instanceChar = this.LetterOnTile(potentialIntersection);
				char otherChar = c.LetterOnTile(potentialIntersection);
				// IF CROSSING ON THE SAME LETTER --> TRUE
				if(this.isWordOverTile(potentialIntersection) && c.isWordOverTile(potentialIntersection) && instanceChar == otherChar)
				{
					if(instanceChar != '0')
						return 1;
					else
						return 0;
				}
				else if(instanceChar == '0' && (potentialIntersection.Y < this.StartingPosition.Y - 1 || potentialIntersection.Y > this.StartingPosition.Y + this.Size))
				{
					return 0;
				}
			}
			
					
			return -1;
		}
	}

}
