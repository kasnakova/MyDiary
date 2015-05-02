﻿namespace MyDiary.Services.Models
{
    using System;
    using System.Linq.Expressions;

    using MyDiary.Models;

    public class NoteViewModel
    {
        public static Expression<Func<Note, NoteViewModel>> FromNote
        {
            get
            {
                return n => new NoteViewModel
                {
                    Id = n.Id,
                    NoteText = n.NoteText,
                    Date = n.Date
                };
            }
        }

        public int Id { get; set; }

        public string NoteText { get; set; }

        public DateTime Date { get; set; }
    }
}