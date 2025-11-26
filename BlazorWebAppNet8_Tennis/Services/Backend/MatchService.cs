using BlazorWebAppNet8_Tennis.DTO.Match;
using BlazorWebAppNet8_Tennis.DTO.Request;
using BlazorWebAppNet8_Tennis.DTO.Response;
using BlazorWebAppNet8_Tennis.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebAppNet8_Tennis.Services.Backend
{
    
    public class MatchService
    {
        public MatchStateDto AddPoint(int player, MatchStateDto match)
        {
            // Kontrollera att matchen inte är slut:
            int p1SetWins = 0;
            int p2SetWins = 0;

            for (int i = 0; i < match.PlayerOneSetScores.Count; i++)
            {
                if (
                    IsSetWon(match.PlayerOneSetScores[i], match.PlayerTwoSetScores[i])
                    &&
                    match.PlayerOneSetScores[i] > match.PlayerTwoSetScores[i])
                {
                    p1SetWins++;
                }
                else if(IsSetWon(match.PlayerOneSetScores[i], match.PlayerTwoSetScores[i]))
                {
                    p2SetWins++;
                }
            }

            if (Math.Max(p1SetWins, p2SetWins) >= match.SetsToWin)
                return match;

            // ---- Slut på kontroll ---



            int diff = Math.Abs( match.PlayerOnePoints - match.PlayerTwoPoints );
            int max = Math.Max(match.PlayerOnePoints, match.PlayerTwoPoints);
            bool isTiebreak = IsTiebreak(match);

            bool okToAddPoint = true;
            if(!isTiebreak && max >= 4 && diff >= 2)
            {
                okToAddPoint = false;
            }else if(isTiebreak && max >= 7 && diff >= 2)
            {
                okToAddPoint = false;
            }



            if (okToAddPoint)
            {
                if (player == 1)
                {
                    match.PlayerOnePoints++;
                }
                else if (player == 2)
                {
                    match.PlayerTwoPoints++;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(player), "You must choose either player one or two");
                }
            }

            return match;
        }



        public MatchStateDto UpdateMatch(MatchStateDto match)
        {
            match = UpdateGames(match);
            match = UpdateSets(match);
            return match;
        }



        public MatchStateDto UpdateGames(MatchStateDto match)
        {
            int diff = Math.Abs(match.PlayerOnePoints - match.PlayerTwoPoints);
            int max = Math.Max(match.PlayerOnePoints, match.PlayerTwoPoints);
            bool isTiebreak = IsTiebreak(match);



            bool isGameWon = false;
            
            
            
            if(!isTiebreak && max >= 4 && diff >= 2)
            {
                isGameWon = true;
            }else if(isTiebreak && max >= 7 && diff >= 2)
            {
                isGameWon = true;
            }


            if (!isGameWon)
                return match;


            if(match.PlayerOnePoints > match.PlayerTwoPoints)
            {
                match.PlayerOneSetScores[match.PlayerOneSetScores.Count - 1]++;
                match.PlayerOnePoints = 0;
                match.PlayerTwoPoints = 0;
            }else if(match.PlayerTwoPoints > match.PlayerOnePoints)
            {
                match.PlayerTwoSetScores[match.PlayerTwoSetScores.Count - 1]++;
                match.PlayerOnePoints = 0;
                match.PlayerTwoPoints = 0;
            }

            return match;
        }

        public MatchStateDto UpdateSets(MatchStateDto match)
        {
            int currentPlayerOneGames = match.PlayerOneSetScores[match.PlayerOneSetScores.Count - 1];
            int currentPlayerTwoGames = match.PlayerTwoSetScores[match.PlayerTwoSetScores.Count - 1];

            int diff = Math.Abs(currentPlayerOneGames - currentPlayerTwoGames);
            int max = Math.Max(currentPlayerOneGames, currentPlayerTwoGames);

            //bool isSetWon = false;
            //if(
            //    (diff >= 2 && max >= 6)
            //    ||
            //    max == 7
            //)
            //{
            //    isSetWon = true;
            //}
            bool isSetWon = IsSetWon(currentPlayerOneGames, currentPlayerTwoGames);


            if (isSetWon)
            {
                int playerOneSetWins = 0;
                int playerTwoSetWins = 0;

                for (int i = 0; i < match.PlayerOneSetScores.Count; i++)
                {
                    if (IsSetWon(match.PlayerOneSetScores[i], match.PlayerTwoSetScores[i]) &&
                        match.PlayerOneSetScores[i] > match.PlayerTwoSetScores[i]
                    )
                    {
                        playerOneSetWins++;
                    }
                    else if(IsSetWon(match.PlayerOneSetScores[i], match.PlayerTwoSetScores[i]))
                    {
                        playerTwoSetWins++;
                    }
                }

                if(Math.Max(playerOneSetWins, playerTwoSetWins) < match.SetsToWin)
                {
                    match.PlayerOneSetScores.Add(0);
                    match.PlayerTwoSetScores.Add(0);
                }
            }


            //return ConvertToResponse(match);
            return match;
        }


        private bool IsSetWon(int p1Score, int p2Score)
        {
            int diff = Math.Abs(p1Score - p2Score);
            int max = Math.Max(p1Score, p2Score);
            if(
                max >= 6 && diff >= 2
                ||
                max >= 7
            )
            {
                return true;
            }

            return false;
        }















        public bool IsTiebreak(MatchStateDto match)
        {
            int currentPlayerOneGameScore = match.PlayerOneSetScores[match.PlayerOneSetScores.Count - 1];
            int currentPlayerTwoGameScore = match.PlayerTwoSetScores[match.PlayerTwoSetScores.Count - 1];
            bool isTiebreak = (currentPlayerOneGameScore == 6 && currentPlayerTwoGameScore == 6);

            return isTiebreak;
        }

        
    }
}
