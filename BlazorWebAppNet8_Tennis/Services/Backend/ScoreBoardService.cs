using BlazorWebAppNet8_Tennis.DTO.Match;
using BlazorWebAppNet8_Tennis.DTO.Request;
using BlazorWebAppNet8_Tennis.DTO.Response;
using BlazorWebAppNet8_Tennis.Enums;
using System.Drawing;

namespace BlazorWebAppNet8_Tennis.Services.Backend
{
    public class ScoreBoardService
    {
        public ScoreBoardResponseDto BuildScoreBoard(MatchStateDto match)
        {
            string points = CalculatePointsText(match);

            //string SetsListP1 = match.PlayerOneSetScores[^1].ToString();
            //string SetsListP2 = match.PlayerTwoSetScores[^1].ToString();

            string SetsListP1 = SetPointsToString(match.PlayerOneSetScores);
            string SetsListP2 = SetPointsToString(match.PlayerTwoSetScores);

            //string setsTotalTextP1 = match.PlayerOneSetScores.Sum().ToString();
            //string setsTotalTextP2 = match.PlayerTwoSetScores.Sum().ToString();

            string setsTotalTextP1 = CalculateSetsWon(match, 1).ToString();
            string setsTotalTextP2 = CalculateSetsWon(match, 2).ToString();

            string matchWinner = CalculateMatchWinnerText(match);

            return new ScoreBoardResponseDto
            {
                PointsText = points,
                SetsListP1 = SetsListP1,
                SetsListP2 = SetsListP2,
                SetsTotalTextP1 = setsTotalTextP1,
                SetsTotalTextP2 = setsTotalTextP2,
                MatchWonText = matchWinner
            };
        }

        



        
        //public string CalculatePointsText(GameRequestDto game)
        public string CalculatePointsText(MatchStateDto game)
        {
            string pointsText = "";

            int diff = Math.Abs(game.PlayerOnePoints - game.PlayerTwoPoints);
            int max = Math.Max(game.PlayerOnePoints, game.PlayerTwoPoints);

            bool gameOver = false;
            bool isTiebreak = IsTiebreak(game);

            //if (!game.IsTiebreak)
            if (!isTiebreak)
            {
                if (game.PlayerOnePoints == game.PlayerTwoPoints)
                {
                    // Denna switch-sats kanske jag ska lägga i en egen funktion för lika poäng.
                    switch (game.PlayerOnePoints)
                    {
                        case 0:
                            pointsText = "Love - All";
                            break;
                        case 1:
                            pointsText = "Fifteen - All";
                            break;
                        case 2:
                            pointsText = "Thirty - All";
                            break;
                        case 3:
                            pointsText = "Deuce";
                            break;
                        default:
                            pointsText = "Deuce";
                            break;
                    }
                }
                else if (game.PlayerOnePoints > 3 && game.PlayerOnePoints > game.PlayerTwoPoints && diff >= 2)
                {
                    pointsText = $"{game.PlayerOneName} wins game.";
                    gameOver = true;
                }
                else if (game.PlayerTwoPoints > 3 && game.PlayerTwoPoints > game.PlayerOnePoints && diff >= 2)
                {
                    pointsText = $"{game.PlayerTwoName} wins game.";
                    gameOver = true;
                }
                else if (diff == 1 && (game.PlayerOnePoints > 3 || game.PlayerTwoPoints > 3))
                {
                    pointsText = game.PlayerOnePoints > game.PlayerTwoPoints ?
                        $"Advantage {game.PlayerOneName}" : $"Advantage {game.PlayerTwoName}";
                }
                else
                {
                    pointsText = $"{PointToWord(game.PlayerOnePoints)} - {PointToWord(game.PlayerTwoPoints)}";
                }

            }
            else
            {
                if (game.PlayerOnePoints >= 7 && diff >= 2)
                {
                    pointsText = $"{game.PlayerOneName} wins game.";
                    gameOver = true;
                }
                else if (game.PlayerTwoPoints >= 7 && diff >= 2)
                {
                    pointsText = $"{game.PlayerTwoName} wins game.";
                    gameOver = true;
                }
                else
                {
                    pointsText = $"{game.PlayerOnePoints} - {game.PlayerTwoPoints}";
                }
            }

            return pointsText;
        }

        private string PointToWord(int point)
        {
            // Känns som att det är lite onödigt att använda en enum för detta
            string word = "";
            switch (point)
            {
                case (int)GamePoint.Love:
                    word = "Love";
                    break;
                case (int)GamePoint.Fifteen:
                    word = "Fifteen";
                    break;
                case (int)GamePoint.Thirty:
                    word = "Thirty";
                    break;
                case (int)GamePoint.Forty:
                    word = "Forty";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("You can only translate points 0 - 3.");
                    break;
            }
            ;
            return word;
        }

        public bool IsTiebreak(MatchStateDto match)
        {
            int currentPlayerOneGameScore = match.PlayerOneSetScores[match.PlayerOneSetScores.Count - 1];
            int currentPlayerTwoGameScore = match.PlayerTwoSetScores[match.PlayerTwoSetScores.Count - 1];
            bool isTiebreak = (currentPlayerOneGameScore == 6 && currentPlayerTwoGameScore == 6);

            return isTiebreak;
        }


        private string CalculateMatchWinnerText(MatchStateDto match)
        {
            int p1Wins = 0;
            int p2Wins = 0;

            List<int> p1Scores = match.PlayerOneSetScores;
            List<int> p2Scores = match.PlayerTwoSetScores;

            for (int i = 0; i < match.PlayerOneSetScores.Count; i++)
            {
                if (
                    (p1Scores[i] - p2Scores[i] > 2 && p1Scores[i] >= 6)
                    ||
                    p1Scores[i] >= 7
                )
                {
                    p1Wins++;
                }
                else if(
                    (p2Scores[i] - p1Scores[i] > 2 && p2Scores[i] >= 6)
                    ||
                    p2Scores[i] >= 7
                )
                {
                    p2Wins++;
                }
            }


            if (p1Wins >= match.SetsToWin)
                return $"{match.PlayerOneName} wins the match!";

            if (p2Wins >= match.SetsToWin)
                return $"{match.PlayerTwoName} wins the match!";

            return "";
        }

        
        public string SetPointsToString(IEnumerable<int> points)
        {
            string result = "  | ";
            foreach(int point in points)
            {
                result += " " + point;
            }
            return result;
        }

        public string CalculateGamesText(MatchResponseDto match)
        {
            string playerOneGameText = match.PlayerOneGameScores.Sum().ToString();
            string playerTwoGameText = match.PlayerTwoGameScores.Sum().ToString();
            return playerOneGameText + "\n" + playerTwoGameText;
        }

        public string CalculateSetsText(MatchResponseDto match)
        {
            string playerOneSetText = match.PlayerOneSetScores.Sum().ToString();
            string playerTwoSetText = match.PlayerTwoSetScores.Sum().ToString();
            return playerOneSetText + "\n" + playerTwoSetText;
        }

        public int CalculateSetsWon(MatchStateDto match, int player)
        {
            List<int> currentPlayer;
            List<int> opponent;
            if (player == 1)
            {
                currentPlayer = match.PlayerOneSetScores;
                opponent = match.PlayerTwoSetScores;
            }
            else
            {
                currentPlayer = match.PlayerTwoSetScores;
                opponent = match.PlayerOneSetScores;
            }
            int score = 0;
            for (int i = 0; i < currentPlayer.Count; i++)
            {
                if (
                    (currentPlayer[i] - opponent[i] >= 2 && currentPlayer[i] >= 6)
                    ||
                    (currentPlayer[i] >= 7)
                ){
                    score++;
                }
            }
            return score;
        }
    }
}
