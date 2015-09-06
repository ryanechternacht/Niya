This is a project a quickly wrote to write an AI to play the game Niya (a modified tic-tac-toe). The AI should be able to win when it goes first. However, it plays very poorly when not going first. 

The AI performs a complete assessment of the move space and chooses a route that leads to the most possible wins. It's very first run (especially when going first) tends to take awhile. I multi-threaded the search for different moves, but its still quite slow (about 5 minutes on my mac book air). Subsequent searches tend to be rather quick. 

The logic for running the game is pretty bare bones, but it gets the job done. 

Overall, this could use a lot of work (and probably won't get it). 
