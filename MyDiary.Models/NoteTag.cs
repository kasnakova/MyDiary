namespace MyDiary.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NoteTag
    {
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Tag { get; set; }
    }
}
