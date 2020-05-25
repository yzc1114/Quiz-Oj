using System;

namespace quiz_oj.Entities
{
    public class OjQuestion
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool Passed { get; set; }
        public int Difficulty { get; set; }
        public DateTime CreateTime { get; set; }
    }
}