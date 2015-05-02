namespace MyDiary.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Note
    {
        public int Id { get; set; }

        [Required]
        public NoteType Type { get; set; }

        //TODO: note can have multiple tags
        public int? NoteTagId { get; set; }

        public virtual NoteTag NoteTag { get; set; }

        public string NoteText { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateModified { get; set; }

        [MinLength(6)]
        public string PasswordHash { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
