using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizMaster.Library.Common.Models.QuizSession
{
    public class SetDTO
    {
        public string QSetDesc { get; set; }
        public string QSetName { get; set; }
        public IEnumerable<int>  questions { get; set;}
    }
}
