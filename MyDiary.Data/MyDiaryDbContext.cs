namespace MyDiary.Data
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using MyDiary.Models;
    using MyDiary.Data.Migrations;

    public class MyDiaryDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyDiaryDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MyDiaryDbContext, Configuration>());
        }

        public static MyDiaryDbContext Create()
        {
            return new MyDiaryDbContext();
        }

        public IDbSet<Note> Notes { get; set; }

        public IDbSet<DoNotForgetNote> DoNotForgetNotes { get; set; }

        public IDbSet<NoteTag> Tags { get; set; }
    }
}
