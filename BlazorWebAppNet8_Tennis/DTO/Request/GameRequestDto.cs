namespace BlazorWebAppNet8_Tennis.DTO.Request
{
    public class GameRequestDto
    {
        public bool IsTiebreak { get; set; }
        public int PlayerOnePoints { get; set; }
        public int PlayerTwoPoints { get; set; }

        public string PlayerOneName { get; set; }
        public string PlayerTwoName { get; set; }
    }
}
