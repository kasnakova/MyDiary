namespace MyDiary.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DoNotForgetNote
    {
        public DoNotForgetNote()
        {
            this.Checked = false;
        }

        public int Id { get; set; }

        [Required]
        public int NoteId { get; set; }

        public virtual Note Note { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(150)]
        public string ListItem { get; set; }

        [Required]
        public bool Checked { get; set; }
    }
}
