namespace quiz_oj.Entities.Quiz
{
    public class QuizStatus
    {
        public bool Ok { get; set; }
        // if ok is false then use
        public bool ContinueLast { get; set; }
        public int LastScore { get; set; }
        public int LastRemainingSecs { get; set; }
        public QuizQuestion LastQuizQuestion { get; set; }
        // if not continueLast then use
        public bool AutomaticSubmitted { get; set; }
    }
}