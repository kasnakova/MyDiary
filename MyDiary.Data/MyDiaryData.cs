namespace MyDiary.Data
{
    using System.Data.Entity;
    using System.Collections.Generic;
    using System;

    using MyDiary.Data.Repositories;
    using MyDiary.Models;

    public class MyDiaryData : IMyDiaryData
    {
        private DbContext context;

        private IDictionary<Type, object> repositories;

        public MyDiaryData(DbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public IRepository<ApplicationUser> Users
        {
            get { return this.GetRepository<ApplicationUser>(); }
        }

        public IRepository<Note> Notes
        {
            get { return this.GetRepository<Note>(); }
        }

        public IRepository<NoteTag> Tags
        {
            get { return this.GetRepository<NoteTag>(); }
        }

        public IRepository<DoNotForgetNote> DoNotForgetNotes
        {
            get { return this.GetRepository<DoNotForgetNote>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfRepository = typeof(T);
            if (!this.repositories.ContainsKey(typeOfRepository))
            {
                var newRepository = Activator.CreateInstance(typeof(EFRepository<T>), context);
                this.repositories.Add(typeOfRepository, newRepository);
            }

            return (IRepository<T>)this.repositories[typeOfRepository];
        }
    }
}
