

# Table Tennis Generator

A program to generate matches for table tennis & process game outcomes (scores) to show the tournament results.

### Generating Matches

Users supply a list of players, the number of rounds to play, and the number of simultaneous matches that can occur within one round (to allow for the possibility of multiple tables available). 

|     Inputs             |
|------------------------|
| Players   |
| Number of matches that can be played simultaneously within a single round |
| Number of rounds to be played  |

|     Output            |
|------------------------|
| An enumeration of teams for each match within each round that fulfills the listed requirements for the solution. |

```
Assumptions: 
	1. Each team consists of 2 players.
	2. Each match consists of 2 teams (i.e., each team plays one other team in a match).
```

#### Solution Requirements
1. The given number of rounds must be played. 
2. If the given number of players is sufficient to play the given number of simultaneous matches, the given number of simultaneous matches must be played each round. If not, as many simultaneous rounds as possible (up to but not exceeding the user-supplied number) must be played. (e.g., if the user says 3 simultaneous matches and gives 16 players, 3 simultaneous rounds must be played per round. If instead the user says 3 simultaneous matches but only gives 8 players, 2 simultaneous matches must be played per round.)
3. Maximize the number of unique teams that play (i.e., maximize the number of unique partners each player plays with)
4. Minimize the variance in the number of games played by each player (i.e., as much as possible, each player should play the same number of games).

#### Example
Given the list of players ```Sally, John, Bill, Bob, Mary, Hank, Sue, Tom```, 2 simultaneous matches, and 4 rounds, the program could produce the following:

|  Round | Match | Team1, Player1 | Team1, Player2 | Team2, Player1 | Team2, Player1 |
|--------|-------|----------------|----------------|----------------|----------------|
| 1 | 1 | Bill | Tom | Bob | John |
| 1 | 2 | Sue | Mary | Sally | Hank |
| 2 | 1 | Hank | Tom | Sally | Sue |
| 2 | 2 | John | Bill | Mary | Bob |
|3 | 1 | Bob | Bill | Mary | John |
|3 | 2 | Tom | Sue | Sally | Hank |
| 4 | 1 | Hank | John | Bob | Sally |
| 4 | 2 | Sue | Mary | Bill | Tom | 

### Processing Tournament Results

TODO





