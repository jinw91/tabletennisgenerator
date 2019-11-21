

# Table Tennis Generator

A program to generate matches for table tennis & process game outcomes (scores) to show the tournament results.

### Generating Matches

Users supply a list of players, the number of rounds to play, and the number of simultaneous matches that can occur within one round (to allow for the possibility of multiple tables available). 

|     Inputs             |
|------------------------|
| Players   |
| Number of matches that can be played simultaneously within a single round |
| Number of rounds to be played  |

```
Assumptions: 
	1. Each team consists of 2 players.
	2. Each match consists of 2 teams (i.e., each team plays one other team in a match).
```

|     Output            |
|------------------------|
| An enumeration of teams for each match within each round that fulfills the listed requirements for the solution. |

#### Solution Requirements
1. Maximize the number of unique teams that play (i.e., maximize the number of unique partners each player plays with)
2. Minimize the variance in the number of games played by each player (i.e., as much as possible, each player should play the same number of games).

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





