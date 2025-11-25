using BlazorWebAppNet8_Tennis.DTO.Match;
using BlazorWebAppNet8_Tennis.DTO.Request;
using BlazorWebAppNet8_Tennis.DTO.Response;
using BlazorWebAppNet8_Tennis.Enums;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebAppNet8_Tennis.Services.Backend
{
    
    public class MatchService
    {
        //public MatchResponseDto AddPoint(int player, MatchRequestDto match)
        public MatchStateDto AddPoint(int player, MatchStateDto match)
        {
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

            //return ConvertToResponse(match);
            return match;
        }



        public MatchStateDto UpdateMatch(MatchStateDto match)
        {
            match = UpdateGames(match);
            match = UpdateSets(match);
            return match;
        }



        //public MatchResponseDto UpdateGames(MatchRequestDto match)
        public MatchStateDto UpdateGames(MatchStateDto match)
        {
            int diff = Math.Abs(match.PlayerOnePoints - match.PlayerTwoPoints);
            int max = Math.Max(match.PlayerOnePoints, match.PlayerTwoPoints);
            bool isTiebreak = IsTiebreak(match);



            //bool shouldUpdateSet = false;
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


            //if(isGameWon && match.PlayerOnePoints > match.PlayerTwoPoints)
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

            //if (isTiebreak)
            //    match.IsTiebreak = false;

            //return ConvertToResponse(match);
            return match;
        }

        //public MatchResponseDto UpdateSets(MatchRequestDto match)
        public MatchStateDto UpdateSets(MatchStateDto match)
        {
            int currentPlayerOneGames = match.PlayerOneSetScores[match.PlayerOneSetScores.Count - 1];
            int currentPlayerTwoGames = match.PlayerTwoSetScores[match.PlayerTwoSetScores.Count - 1];

            int diff = Math.Abs(currentPlayerOneGames - currentPlayerTwoGames);
            int max = Math.Max(currentPlayerOneGames, currentPlayerTwoGames);

            //bool shouldUpdateSets = false;
            bool isSetWon = false;

            if(
                (diff >= 2 && max >= 6)
                ||
                max == 7
            )
            {
                isSetWon = true;
            }

            if (isSetWon)
            {
                int playerOneSetWins = 0;
                int playerTwoSetWins = 0;

                for (int i = 0; i < match.PlayerOneSetScores.Count; i++)
                {
                    if (match.PlayerOneSetScores[i] > match.PlayerTwoSetScores[i])
                    {
                        playerOneSetWins++;
                    }
                    else
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


















        //public bool IsTiebreak(MatchRequestDto match)
        public bool IsTiebreak(MatchStateDto match)
        {
            int currentPlayerOneGameScore = match.PlayerOneSetScores[match.PlayerOneSetScores.Count - 1];
            int currentPlayerTwoGameScore = match.PlayerTwoSetScores[match.PlayerTwoSetScores.Count - 1];
            bool isTiebreak = (currentPlayerOneGameScore == 6 && currentPlayerTwoGameScore == 6);

            return isTiebreak;
        }

        //public MatchResponseDto ConvertToResponse(
        //    MatchRequestDto request,
        //    bool isTiebreak = false,
        //    string matchWonText = "",
        //    string setWonText = "",
        //    string gameWonText = "",
        //    string pointsText = ""
        //)
        //{
        //    return new MatchResponseDto
        //    {
        //        PlayerOneName = request.PlayerOneName,
        //        PlayerTwoName = request.PlayerTwoName,

        //        PlayerOnePoints = request.PlayerOnePoints,
        //        PlayerTwoPoints = request.PlayerTwoPoints,

        //        PlayerOneSetScores = request.PlayerOneSetScores?.ToList() ?? new List<int>(),
        //        PlayerTwoSetScores = request.PlayerTwoSetScores?.ToList() ?? new List<int>(),

                
        //        IsTiebreak = isTiebreak,
        //        MatchWonText = matchWonText,
        //        SetWonText = setWonText,
        //        GameWonText = gameWonText,
        //        PointsText = pointsText
        //    };
        //}
    }
}
