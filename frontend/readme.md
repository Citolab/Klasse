# Klasse

Cito launches Klasse, a cooperative game for secondary education classes. Your students play this 'escape room-like' game in a race against the clock. Because the solution must be entered within 30 minutes. In the meantime, the teacher can observe how their students work together.

This source codes only contains the original source code for the theme site [https://klasse.cito.nl/](https://klasse.cito.nl/)

## Frontend

The frontend is build with Vue2

To run the project, clone this repository

In the frontend folder install all dependencies
```
npm i
```
start the frontend with
```
npm run serve
```

We added bootstrap and themes for the different subjects
There are 2 themes included
- basic theme
- hacker theme

## Backend

The backend is a dotnet core web API project (netcoreapp2.1).

Swashbuckle is included, so the api can be explored when running locally at:
https://localhost:5001/swagger/index.html

At the moment, the backend uses two in-memory repos for `Games` and `GameSessions`. 
`TestGameRepository` is initialized with:
```c#
_games = new List<Game>
                {new Game(1, "WACHTWOORD", 5, 30), new Game(2, "PASSWORD", 4, 20)}; 

```
 * Game 1, Id 1, solution "WACHTWOORD", 5 minutes max time, 30 seconds penalty time.
 * Game 2, Id 2, solution "PASSWORD", 4 minutes max time, 20 seconds penalty time.

### Endpoints



#### `POST /api/game/start/{gameId}`
This call doesn't have a body payload.

Produces the following response when called with a valid game id:
```javascript
StartGameResponse {
    startTime	        string($date-time)  // start time of the game session
    solutionLength	integer($int32)     // length of the solution (number of letters)
    maxTimeInMinutes	integer($int32)     // Time to solve the puzzles (usually 30)
    penaltySeconds	integer($int32)     // penalty that is applied when checking a wrong answer
    remainingSeconds	integer($int32)     // remaining seconds to finish the game
    state               integer($int32) // integer representing the game state
}
```

```c#
 public enum GameState
    {
        Started = 0,
        Passed = 1,
        Continued = 2,
        Stopped = 3
    }
```

#### `POST /api/game/stop`

Clears the session cookie, effectively stopping the game session. Returns 200 OK.

#### `POST /api/game/continue`

Sets the game state to `Continued`. Returns 200 OK.

#### `POST /api/game/checkanswer`
Body: 
`string`

Checks the supplied answer and produces the following `CheckAnswerResponse` response:
```javascript
CheckAnswerResponse {
    code	                integer($int32)
    message	                string
    totalPenaltySecondsApplied	integer($int32)
    remainingSeconds	        integer($int32)
}
```

The code and message fields have the following possible values:
 * 42 - Correct.  
 * 2 -  Incorrect answer, penalty time ([PenaltySeconds]s) applied.

### Errors
When an error occurs (client or server), an `ErrorResponse`  is given:
```javascript
ErrorResponse {
    code	integer($int32)
    message	string
}
```

The code field has the following possible values:
 * 3 -  Already passed or stopped this game session.
 * 13 - No session cookie found.
 * 39 - Unknown game id.
 * 144 - Exceeding check rate limit. Checks must be 5 seconds apart.


