using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Models.QuizSession
{
    public class CreateRoomDTO
    {
        [Required]
        public string RoomName { get; set; } = string.Empty;

        /// <summary>
        /// List of question set Id's
        /// </summary>
        [Required]
        public IEnumerable<int> QuestionSets { get; set; } = new List<int>();

        /// <summary>
        /// Room Options:<br/>
        ///     - mode: elimination or normal (default normal | on elimination round, top 50% will proceed to the next round)<br/>
        ///     - show_leader_board_each_round: true|false (default true)<br/>
        ///     - display_top_10_only: true|false (default true, on false, everyone is showned based on ranking)<br/>
        ///     - allow_reconnect: true|false (default true)<br/>
        ///     - allow_join_on_game_started: true|false (default false)<br/>
        /// </summary>
        [Required] 
        public IEnumerable<string> RoomOptions { get; set; } = new List<string>();
    }
}
