using System.Collections.Generic;

namespace BlazorWebAppNet8_Tennis.DTO.Response
{
    public class MatchResponseDto
    {
        //public bool IsTiebreak { get; set; } = false;
        public string MatchWonText { get; set; } = "";
        public string SetWonText { get; set; } = "";
        public string GameWonText { get; set; } = "";
        public string PointsText { get; set; } = "";


        public string PlayerOneName { get; set; } = "";
        public List<int> PlayerOneSetScores { get; set; } = new List<int>();
        public List<int> PlayerOneGameScores { get; set; } = new List<int>();
        public int PlayerOnePoints { get; set; } = 0;

        public string PlayerTwoName { get; set; } = "";
        public List<int> PlayerTwoSetScores { get; set; } = new List<int>();
        public List<int> PlayerTwoGameScores { get; set; } = new List<int>();
        public int PlayerTwoPoints { get; set; } = 0;
    }
}
