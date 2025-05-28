# SportsRadarTechTest

Assumptions:
- Scores can only be incremented by 1 in any update. Furthermore only one team's score can be changed in a single update.
  - I would change the API to reflect this, with HomeTeamScored, AwayTeamScored type methods instead of the Update score that currently exists.
- I made UpdateScore the responsibility of the FootballMatch interface, as this felt like a better division of responsibilities.
- In this solution it is not currently possible to decrease the score. In normal operations I think this is a sensible restriction.
  - If I were to expand this funtionality further, I would add an endpoint specifically for setting scores in non-normal scenarios (a goal has been rescinded for example.) I think that division would reduce erroneous updates and allow for a split in permission levels.
- For now I've put all the testing in one project. For a more complex solution I would split these out on a per project basis but for the sake of reducing clutter here I decided to keep them all in the same place.

Work on next:
- This solution leaves the concept of a scoreboard as being quite tightly coupled to the concept of a football match. In the next revision I would split these apart, perhaps using generics to allow for a scoreboard that can support multiple game types.

Design choices:
- I have set this up as a public facing library. The intent is for this to be decoupled and configurable by the consumers.
- For example, a consumer wishing to use this library could set it up like so
  '''csharp services.Scoreboard(builder =>
     builder.WithInMemoryDataStore()
     builder.WithInMemoryTeamValidator([] {team1, team2, ...})
     )
  '''
  Should that consumer with to change to an SQLite backed data store, for example, they would simply need to change the registration.