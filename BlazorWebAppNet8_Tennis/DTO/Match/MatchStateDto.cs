namespace BlazorWebAppNet8_Tennis.DTO.Match
{
    public class MatchStateDto
    {
        public string PlayerOneName { get; set; } = "";
        public string PlayerTwoName { get; set; } = "";

        public List<int> PlayerOneSetScores { get; set; } = new();
        public List<int> PlayerTwoSetScores { get; set; } = new();

        public int PlayerOnePoints { get; set; }
        public int PlayerTwoPoints { get; set; }

        //public bool IsTiebreak { get; set; }
        public int SetsToWin { get; set; }
    }
}
