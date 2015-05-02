namespace MyDiary.Data
{
    using MyDiary.Data.Repositories;
    using MyDiary.Models;

    public interface IMyDiaryData
    {
        IRepository<ApplicationUser> Users { get; }

        IRepository<Note> Notes { get; }

        IRepository<NoteTag> Tags { get; }

        IRepository<DoNotForgetNote> DoNotForgetNotes { get; }

        int SaveChanges();
    }
}
