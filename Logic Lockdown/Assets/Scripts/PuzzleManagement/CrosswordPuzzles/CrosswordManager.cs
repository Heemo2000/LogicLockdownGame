using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Game.Controls;
using Cinemachine;
using Leguar.TotalJSON;


namespace Game.PuzzleManagement.CrosswordPuzzles
{
    public class CrosswordManager : MonoBehaviour
    {
        [Header("Showing hint Settings:")]
        [SerializeField]private Camera lookCamera;
        [SerializeField]private TMP_Text clueText;

        [Header("Generator Settings:")]
        [SerializeField]private TextAsset wordsFile;
	    [SerializeField]private CrosswordTile tile;
		[SerializeField]private Vector2 blockSize;
		[SerializeField]private CinemachineTargetGroup targetGroup;
        [SerializeField]private bool showBackground;

        [Min(0.5f)]
        [SerializeField]private float generateTime = 1.0f;
        private CrosswordManagerControls _controls;
    
        private Coroutine _generateCrosswordCoroutine;
        private List<CrosswordInfo> _crosswordToShow = new List<CrosswordInfo>();

	    private List<CrosswordTile> _allTiles = new List<CrosswordTile>();

        public bool AllTilesFilledCorrect()
        {
			if(_allTiles.Count == 0)
			{
				return false;
			}

            foreach(CrosswordTile tile in _allTiles)
            {
				char current = tile.InputField.text[0];
				//Debug.Log("Current: " + current + ", Clue Part: " + tile.CluePart);
                if(current != tile.CluePart)
                {
                    return false;
                }
            }

			//Debug.Log("All tiles are filled correctly");

            return true;
        }
	    private void FindCrosswordInfoPositions()
	    {
	        //Get words
	        List<CrosswordInfo> fixedWords = new List<CrosswordInfo>();

			try
              {
                JSON json = JSON.ParseString(wordsFile.text);
                var crosswordDatas = json.GetJArray("crosswordDatas");

                foreach(JValue value in crosswordDatas.Values)
                {
                    string dataInString = value.CreateString();
                    JSON dataJSON = JSON.ParseString(dataInString);
					fixedWords.Add(new CrosswordInfo(dataJSON.GetString("word").ToUpper().Trim(), 
													 dataJSON.GetString("clue").ToUpper().Trim()));
                    //Debug.Log(dataJSON.GetString("word") + ": " + dataJSON.GetString("clue"));
                }
              }
              catch(JSONKeyNotFoundException exception)
              {
                Debug.Log(exception.ToString());
              }

			if(fixedWords.Count == 0)
			{
				Debug.Log("Count is zero");
				return;
			}

	        // The very final crosswords

			List<CrosswordInfo> CrossWordsToKeep = new List<CrosswordInfo>();

			float crosswordLength = float.MaxValue;
			int WordsPlaced = 0;
			int crosswordMinX = 0, crosswordMinY = 0;

			// Looping to choose the best grid (looping arbitrary "the number of words" time) 

			for(int gen = 0 ;  gen < fixedWords.Count ; gen++)
			{
				// Not Touching the initial List
				List <CrosswordInfo> allWords = new List<CrosswordInfo>(fixedWords);

				if(gen % 2 == 1)
				{
					// Shuffling the words half the tries because sometimes not starting with the longer word can be a good option too
					for(int j = allWords.Count - 1 ; j > 0; j--)
					{
						int r = UnityEngine.Random.Range(0, j+1);
						CrosswordInfo tmp = allWords[r];
						allWords[r] = allWords[j];
						allWords[j] = tmp;
					}
				}
				else
				{
					allWords.Sort();
					allWords.Reverse();
				}

				// The final crosswords for this loop only
				List<CrosswordInfo> finalWords = new List<CrosswordInfo>();

				// Adding the first word we found
				finalWords.Add(new CrosswordInfo(allWords[0]));
				// Removing this word from the list
				allWords.RemoveAt(0);

				// Initial size knowing the fact that the first word is Horizontal
				int minX = 0, maxX = finalWords[0].Size - 1, minY = 0, maxY = 0;

				// Loop on all the words we want in the crossword
				int maxLoop = Mathf.FloorToInt(allWords.Count*allWords.Count );

				int z = 0;
				int i = 0;
				for(; 0 != allWords.Count && z < maxLoop ;z++)
				{

					// The current word we want to place 
					CrosswordInfo currentWordToPlace = new CrosswordInfo(allWords[i]);


					// Will tell us if we succeed placing it
					bool isPlaced = false;

					// Will always be the best position we find, initialise here with arbitrary values
					CrosswordTileInfo BestStartingPosition = new CrosswordTileInfo(0,0);
					CrosswordInfo.Direction BestDirection = CrosswordInfo.Direction.Horizontal;

					// Will be a score to tell us which position is "conceptually" the best
					float score = float.MaxValue;

					// Loop on all the existing words in the crossword
					for(int j = 0; j< finalWords.Count; j++)
					{
						// The current already placed word that we will used to try to place the new word
						CrosswordInfo currentWordPlaced = finalWords[j];

						// If we must placed the new one according to the existing one, the new one will be the other direction
						currentWordToPlace.WordDirection = currentWordPlaced.WordDirection == CrosswordInfo.Direction.Horizontal ? CrosswordInfo.Direction.Vertical : CrosswordInfo.Direction.Horizontal;

						// An array wich gave us for each letter (e.g. array[0] for the first letter) the tiles on which the current placed word has the same letter 
						List<CrosswordTileInfo>[] intersectionForEachLetter = currentWordPlaced.SimilarLetterTiles(currentWordToPlace);

						// Loop on all the letters
						for(int k = 0; k < intersectionForEachLetter.Length;k++)
						{
							// Looking for each given tile for one letter
							for(int l = 0; l < intersectionForEachLetter[k].Count; l++)
							{
								// Getting the tile
								CrosswordTileInfo currentCommonCrosswordTile = intersectionForEachLetter[k][l];

								// Given the direction of the placed word and the intersection tile we calculate the new word potential starting position
								if( currentWordPlaced.WordDirection == CrosswordInfo.Direction.Horizontal )
								{
									currentWordToPlace.StartingPosition = new CrosswordTileInfo(currentCommonCrosswordTile.X, currentCommonCrosswordTile.Y - k);
								}
								else
								{
									currentWordToPlace.StartingPosition = new CrosswordTileInfo(currentCommonCrosswordTile.X -k , currentCommonCrosswordTile.Y);
								}

								// Loop on all the words in the crossword to check if the place we want the new word isn't in conflict with the existings words
								// the int to tell us how many correct intersection we have
								int canBePlacedCount = 0;
								// the boolean to tell us a conflict
								bool canBePlaced = true;
								for(int m = 0; m < finalWords.Count && canBePlaced; m++)
								{
									// ca = 0 means no conflict, -1 means a conflict, 1 means a good intersection
									int ca = finalWords[m].CanAccept(currentWordToPlace);
									if(ca > 0)
										canBePlacedCount += ca;
									if(ca == -1)
										canBePlaced = false;
								}

								// The place is OK and have minimum one good intersection
								if(canBePlaced && canBePlacedCount > 0)
								{
									// Calculate a score to find the best place

									// how much intersection but the opposite value
									int crossedNumber = (0 - canBePlacedCount);

									// a conceptual score that should be the less the better
									float tmpScore =  UnityEngine.Random.Range(0,10) + crossedNumber *100; 

									// if this score si better than a previous one we keep this position and tell that we succeed placing at least one time this word
									if( tmpScore < score)
									{
										isPlaced = true;

										// Updating the new best score
										score = tmpScore;
										BestStartingPosition = currentWordToPlace.StartingPosition;
										BestDirection = currentWordToPlace.WordDirection;
									}
								}
							}
						}
					}

					// We have at least one position to place this new word
					if(isPlaced)
					{
						// getting this saved position
						currentWordToPlace.StartingPosition = BestStartingPosition;
						currentWordToPlace.WordDirection = BestDirection;
						// adding this word to the crossword
						finalWords.Add(currentWordToPlace);

						// Shuffling the crossword array to have more random factor on the grid creation (doesn't really matters but linear operation so it's ok)
						for(int j = finalWords.Count - 1 ; j > 0; j--)
						{
							int r = UnityEngine.Random.Range(0, j+1);
							CrosswordInfo tmp = finalWords[r];
							finalWords[r] = finalWords[j];
							finalWords[j] = tmp;
						}

						// Updating the grid Rectangle if necessary
						minX = Mathf.Min(minX, currentWordToPlace.StartingPosition.X);
						minY = Mathf.Min(minY, currentWordToPlace.StartingPosition.Y);

						maxX = Mathf.Max(maxX, currentWordToPlace.WordDirection == CrosswordInfo.Direction.Horizontal ? currentWordToPlace.StartingPosition.X + currentWordToPlace.Size - 1 : currentWordToPlace.StartingPosition.X);
						maxY = Mathf.Max(maxY, currentWordToPlace.WordDirection == CrosswordInfo.Direction.Vertical ? currentWordToPlace.StartingPosition.Y + currentWordToPlace.Size - 1 : currentWordToPlace.StartingPosition.Y);

						allWords.RemoveAt(i);
						if(allWords.Count > 0)
							i = i % allWords.Count;
					}
					else
					{
					 	i = (i+1) % allWords.Count;	
					}
				}

				// Final new length of the grid
				float newLength = Mathf.Sqrt((maxX - minX)*(maxX - minX) + (maxY - minY)*(maxY - minY));
				// Final new number of word we succeed to put on the grid
				int currentWordsPlaced = finalWords.Count;

				// if it's a better grid (smaller and more words). Indeed, we allow a bigger crossword proportionally to how much more words it contains
				if(newLength - (currentWordsPlaced - WordsPlaced)*4 < crosswordLength  && WordsPlaced < currentWordsPlaced)
				{
					// Keeping this grid in memory
					CrossWordsToKeep = finalWords;
					// Updating best grid values
					crosswordLength = newLength;
					WordsPlaced = currentWordsPlaced;

					crosswordMinX = minX;
					crosswordMinY = minY;
				}

			}

			Debug.Log(CrossWordsToKeep.Count+"/"+fixedWords.Count+" size: "+crosswordLength);

			for(int r = 0; r < CrossWordsToKeep.Count; r++)
			{
				CrossWordsToKeep[r].StartingPosition.X -= crosswordMinX;
				CrossWordsToKeep[r].StartingPosition.Y = -CrossWordsToKeep[r].StartingPosition.Y + crosswordMinY;
			}

			_crosswordToShow = CrossWordsToKeep;


	    }

	    private void ShowCrossword()
	    {
	        for(int i = 0; i < _allTiles.Count; i++)
	        {
				targetGroup.RemoveMember(_allTiles[i].transform);
	            Destroy(_allTiles[i].gameObject);
	        }

	        _allTiles.Clear();

			Random.InitState((int)System.DateTime.Now.Ticks);

			int tileNumber = 1;
	        foreach(CrosswordInfo crossword in _crosswordToShow)
	        {
				List<CrosswordTile> tiles = new List<CrosswordTile>();

	            for(int i = 0; i < crossword.Size; i++)
	            {
	                Vector3 pos = Vector3.zero;
					CrosswordTileInfo tilePos = new CrosswordTileInfo(0,0);
	                if(crossword.WordDirection == CrosswordInfo.Direction.Horizontal)
	                {
	                    pos = transform.position + transform.right *  (crossword.StartingPosition.X + i) * blockSize.x + transform.up * crossword.StartingPosition.Y * blockSize.y;
	                    tilePos.X = crossword.StartingPosition.X + i;
						tilePos.Y = crossword.StartingPosition.Y;
	                }
	                else
	                {
	                    pos = transform.position + transform.right * crossword.StartingPosition.X * blockSize.x + transform.up * (crossword.StartingPosition.Y - i) * blockSize.y;
						tilePos.X = crossword.StartingPosition.X;
						tilePos.Y = crossword.StartingPosition.Y - i;
	                }

					CrosswordTile tile = _allTiles.Find((current)=> current.Position.Equals(tilePos));

	                if(tile == null)
	                {
	                    tile = Instantiate(this.tile, pos, transform.rotation);
						tile.CluePart = crossword.Word[i];
	                    tile.ShowCluePart = Random.Range(1, 10+1) >= 8;
						_allTiles.Add(tile);
						tile.gameObject.transform.parent = transform;
						tile.name = tileNumber.ToString();
						tileNumber++;
						tile.Position = tilePos;
                        tile.ShowBackgroundGraphic = showBackground;
                    }

					tile.AddCrosswordInfo(crossword);
					targetGroup.AddMember(tile.transform, 1.0f, 1.0f);
					tiles.Add(tile);
	            }

				for(int i = 0; i < tiles.Count; i++)
				{
					for(int j = 0; j < tiles.Count; j++)
					{
						if(j != i)
						{
							tiles[i].OtherTiles.Add(tiles[j]);
						}
					}
				}

			}
	    }

        private void OnPosOnScreen(InputAction.CallbackContext context)
        {
            Vector3 posOnScreen = context.ReadValue<Vector2>();
            Ray ray = lookCamera.ScreenPointToRay(posOnScreen);
    
            if(Physics.Raycast(ray,out RaycastHit hit,1000f))
            {
                CrosswordTile tile = hit.transform.GetComponent<CrosswordTile>();
                if(tile != null)
                {
                    string clues = "";
                    int clueNumber = 1;
                    foreach(var crossword in tile.Crosswords)
                    {
                        clues += crossword.WordDirection.ToString() +": " + crossword.Clue +"\n";
                        clueNumber++;
                    }
    
                    clueText.text = clues;
                    
                }
                else
                {
                    clueText.text = "";
                }
            }
            else
            {
                clueText.text = "";
            }
        }

        private IEnumerator GenerateCrossword()
        {
            yield return new WaitForSeconds(generateTime);
            FindCrosswordInfoPositions();
            ShowCrossword();
        }
        private void Awake() {
            _controls = new CrosswordManagerControls();
        }
        private void Start() 
        {
            _controls.CrosswordActionMap.PosOnScreen.performed += OnPosOnScreen;
            if(_generateCrosswordCoroutine == null)
            {
                _generateCrosswordCoroutine = StartCoroutine(GenerateCrossword());
            }
        }

        private void OnEnable() 
        {
            _controls.CrosswordActionMap.PosOnScreen.Enable();
        }
        private void OnDisable() 
        {
            _controls.CrosswordActionMap.PosOnScreen.Disable();
        }

        private void OnValidate() 
        {
            foreach(CrosswordTile tile in _allTiles)
            {
                tile.ShowBackgroundGraphic = showBackground;
            }    
        }
    }

}
