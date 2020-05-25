using System.Collections.Generic;

namespace quiz_oj.Entities
{
    public class QuizQuestion
    {
        public int Id { get; set; }
        public string description { get; set; }
        public List<QuizOption> optionDTOList { get; set; }
    }

    public class QuizOption
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public string Description { get; set; }
    }
}