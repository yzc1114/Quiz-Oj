using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace quiz_oj.Entities.Quiz
{
    [Table("QuizHighScore")]
    public class QuizHighScore
    {
        [Column("userId")]
        [Key]
        public string UserId { get; set; }
        
        [Column("highScore")]
        public int HighScore { get; set; }
    }
}