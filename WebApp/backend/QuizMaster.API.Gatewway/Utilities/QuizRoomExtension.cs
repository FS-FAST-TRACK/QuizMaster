using Newtonsoft.Json;
using QuizMaster.Library.Common.Entities.Rooms;

namespace QuizMaster.API.Gateway.Utilities
{
    /*
     * Created by: Jayharron Mar Abejar
     * Date: 12/13/2023
     */
    public static class QuizRoomExtension
    {

        public static IEnumerable<RoomOption> RoomOptionsList(this QuizRoom quizRoom)
        {
            List<RoomOption> roomOptions = new List<RoomOption>();

            /*
             * Room Options to check
             * 
             *          [type]            :           [value]
             * 
             * mode                       :  elimination or normal  (default normal | on elimination round, top 50% will proceed to the next round, others are kicked)
             * showLeaderboardEachRound   :  true or false  (default true)
             * displayTop10Only           :  true or false  (default true, on false, everyone is showned based on ranking)
             * allowReconnect             :  true or false  (default true)
             * allowJoinOnQuizStarted     :  true or false  (default false)
             */
            IEnumerable<string> quizRoomOptions = JsonConvert.DeserializeObject<IEnumerable<string>>(quizRoom.RoomOptions) ?? new List<string>();
            bool hasMode = false;
            bool hasLeaderboard = false;
            bool hasDisplayTop10Only = false;
            bool hasAllowReconnect = false;
            
            quizRoomOptions.ToList().ForEach(quizRoomOption =>
            {
                var option = quizRoomOption.ToLower().Replace(" ",string.Empty);
                // mode
                if (option.Contains("mode:"))
                {
                    hasMode = true;
                    roomOptions.Add(option.Split(":")[1].ToLower().Contains("elimination")?RoomOption.EliminationRound:RoomOption.NormalRound);
                }
                // show leaderboard
                if (option.Contains("showleaderboardeachround:"))
                {
                    hasLeaderboard = true;
                    if(option.Split(":")[1].ToLower().Contains("true"))
                        roomOptions.Add(RoomOption.ShowLeaderBoardEachRound);
                }
                // display top 10 only
                if (option.Contains("displaytop10only:"))
                {
                    hasDisplayTop10Only = true;
                    if (option.Split(":")[1].ToLower().Contains("true"))
                        roomOptions.Add(RoomOption.DisplayTop10Only);
                }
                // allow reconnect if disconnected
                if (option.Contains("allowreconnect:"))
                {
                    hasAllowReconnect = true;
                    if (option.Split(":")[1].ToLower().Contains("true"))
                        roomOptions.Add(RoomOption.AllowReconnect);
                }
                // allow join on quiz started
                if (option.Contains("allowjoinonquizstarted:"))
                {
                    if (option.Split(":")[1].ToLower().Contains("true"))
                        roomOptions.Add(RoomOption.AllowJoinOnQuizStarted);
                }
            });

            // Applying defaults
            if (!hasMode) roomOptions.Add(RoomOption.NormalRound);
            if (!hasLeaderboard) roomOptions.Add(RoomOption.ShowLeaderBoardEachRound);
            if (!hasDisplayTop10Only) roomOptions.Add(RoomOption.DisplayTop10Only);
            if (!hasAllowReconnect) roomOptions.Add(RoomOption.AllowReconnect);

            return roomOptions;
        }

        public static bool IsEliminationRound(this QuizRoom quizRoom)
        {
            return quizRoom.RoomOptionsList().Contains(RoomOption.EliminationRound);
        }

        public static bool IsNormalRound(this QuizRoom quizRoom)
        {
            return quizRoom.RoomOptionsList().Contains(RoomOption.NormalRound);
        }

        public static bool ShowLeaderboardEachRound(this QuizRoom quizRoom)
        {
            return quizRoom.RoomOptionsList().Contains(RoomOption.ShowLeaderBoardEachRound);
        }

        public static bool DisplayTop10Only(this QuizRoom quizRoom)
        {
            return quizRoom.RoomOptionsList().Contains(RoomOption.DisplayTop10Only);
        }

        public static bool AllowReconnect(this QuizRoom quizRoom)
        {
            return quizRoom.RoomOptionsList().Contains(RoomOption.AllowReconnect);
        }

        public static bool AllowJoinOnQuizStarted(this QuizRoom quizRoom)
        {
            return quizRoom.RoomOptionsList().Contains(RoomOption.AllowJoinOnQuizStarted);
        }
    }
}
