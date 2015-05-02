namespace MyDiary.Services.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SaveNoteBindingModel
    {
        [Required]
        [Display(Name = "NoteText")]
        public string NoteText { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
    }
}