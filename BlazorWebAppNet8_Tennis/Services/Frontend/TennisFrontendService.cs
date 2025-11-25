using BlazorWebAppNet8_Tennis.DTO.Match;
using BlazorWebAppNet8_Tennis.DTO.Request;
using BlazorWebAppNet8_Tennis.DTO.Response;
using System.Net.Http.Json;

namespace BlazorWebAppNet8_Tennis.Services.Frontend
{
    
    public class TennisFrontendService
    {
        private readonly HttpClient _http;

        public string PointsText { get; private set; } = "";
        public string SetsTotalTextP1 { get; private set; } = "";
        public string SetsTotalTextP2 { get; private set; } = "";
        public string SetsListP1 { get; private set; } = "";
        public string SetsListP2 { get; private set; } = "";
        //public string GameResultText { get; private set; } = "";
        public string SetResultText { get; private set; } = "";
        public string MatchResultText { get; private set; } = "";

        //public TennisFrontendService(HttpClient http)
        //{
        //    _http = http;
        //}
        public TennisFrontendService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("backend");
        }


        //public async Task<MatchResponseDto> PlayPoint(
        public async Task<MatchStateDto> PlayPoint(
            int player,
            //MatchResponseDto currentState
            MatchStateDto currentState
        )
        {
            //var req = ToRequest(currentState);

            var addPointResp =
                await _http.PostAsJsonAsync(
                    $"api/match/add-point/{player}", 
                    currentState
                );

            Console.WriteLine("AddPoint status: " + addPointResp.StatusCode);
            Console.WriteLine("AddPoint content: " + await addPointResp.Content.ReadAsStringAsync());


            if (!addPointResp.IsSuccessStatusCode)
                throw new Exception("Error while adding point");

            var updatedState =
                await addPointResp.Content.ReadFromJsonAsync<MatchStateDto>();


            var updateResp =
                await _http.PostAsJsonAsync(
                    "api/match/update", 
                    updatedState);

            updatedState =
                await updateResp.Content.ReadFromJsonAsync<MatchStateDto>();


            var scoreResp =
                await _http.PostAsJsonAsync(
                    "api/match/scoreboard", 
                    updatedState
            );

            var scoreBoard =
                await scoreResp.Content.ReadFromJsonAsync<ScoreBoardResponseDto>();

            PointsText = scoreBoard.PointsText;
            //GameResultText = scoreBoard.SetsListP1 + scoreBoard.SetsListP2;
            //GameResultText = scoreBoard.GameTextP2;
            SetsTotalTextP1 = scoreBoard.SetsTotalTextP1;
            SetsTotalTextP2 = scoreBoard.SetsTotalTextP2;
            SetsListP1 = scoreBoard.SetsListP1;
            SetsListP2 = scoreBoard.SetsListP2;
            MatchResultText = scoreBoard.MatchWonText;

            return updatedState!;
        }

        //private MatchRequestDto ToRequest(MatchResponseDto state)
        //{
        //    return new MatchRequestDto
        //    {
        //        SetsToWin = 3,
        //        PlayerOneName = state.PlayerOneName,
        //        PlayerTwoName = state.PlayerTwoName,

        //        PlayerOneSetScores = state.PlayerOneSetScores.ToList(),
        //        PlayerTwoSetScores = state.PlayerTwoSetScores.ToList(),

        //        PlayerOnePoints = state.PlayerOnePoints,
        //        PlayerTwoPoints = state.PlayerTwoPoints
        //    };
        //}
    }

}
