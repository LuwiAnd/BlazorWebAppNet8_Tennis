namespace BlazorWebAppNet8_Tennis.DTO.Request
{
    public class MatchRequestDto
    {
        public int SetsToWin { get; set; }


        public string PlayerOneName { get; set; } = "";
        //public List<int> PlayerOneTotalSetWins { get; set; } = new List<int>();
        public List<int> PlayerOneSetScores { get; set; } = new List<int>();
        public int PlayerOnePoints { get; set; } = 0;



        public string PlayerTwoName { get; set; } = "";
        //public List<int> PlayerTwoTotalSetWins { get; set; } = new List<int>();
        public List<int> PlayerTwoSetScores { get; set; } = new List<int>();
        public int PlayerTwoPoints { get; set; } = 0;
        
    }
}
