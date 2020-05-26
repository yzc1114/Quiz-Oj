using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quiz_oj.Entities.Quiz
{
    [Table("Quiz")]
    public class QuizQuestion
    {
        [Column("id")]
        [Key]
        public string Id { get; set; }
        [Column("description")]
        public string Description { get; set; }
        
        public List<QuizOption> OptionDTOList { get; set; }

        public QuizQuestion()
        {
            OptionDTOList = new List<QuizOption>();
        }
    }

    [Table("QuizOption")]
    public class QuizOption
    {
        [Column("quizId")]
        public string QuizId { get; set; }
        [Column("optionId")]
        public int Id { get; set; }
        [Column("description")]
        public string Description { get; set; }
        
        [Column("correct")]
        public bool? Correct { get; set; }

        // public QuizQuestion Question { get; set; }
    }
}