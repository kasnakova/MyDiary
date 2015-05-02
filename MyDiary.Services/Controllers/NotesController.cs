namespace MyDiary.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using Microsoft.AspNet.Identity;

    using MyDiary.Data;
    using MyDiary.Models;
    using MyDiary.Services.Models;
    using MyDiary.Services.Infrastructure;
    using System.Data.Entity.Core.Objects;

    [Authorize]
    public class NotesController : BaseApiController
    {
        private IUserIdProvider userIdProvider;

        public NotesController()
            :this(new MyDiaryData(new MyDiaryDbContext()), new AspNetUserIdProvider())
        {

        }

        public NotesController(IMyDiaryData data,
                                IUserIdProvider userIdProvider)
            : base(data)
        {
            this.userIdProvider = userIdProvider;
        }

        [HttpPost]
        public IHttpActionResult SaveNote(SaveNoteBindingModel info)
        {
            if (!ModelState.IsValid)
            {
                var dfg = BadRequest(ModelState);
                return BadRequest(ModelState);
            }

            var currentUserId = this.userIdProvider.GetUserId();
            var note = new Note()
            {
                Date = info.Date,
                DateCreated = DateTime.Now,
                DateModified = DateTime.Now,
                NoteText = info.NoteText,
                Type = NoteType.Normal,
                UserId = currentUserId
            };

            this.data.Notes.Add(note);
            this.data.SaveChanges();

            return Ok(note.Id);
        }

        [HttpGet]
        public IHttpActionResult GetNotes(DateTime date)
        {
            var currentUserId = this.userIdProvider.GetUserId();
            var notes = this.data.Notes.All().Where(n => (currentUserId == n.UserId) && (EntityFunctions.TruncateTime(n.Date) == date.Date)).Select(NoteViewModel.FromNote);
            return Ok(notes);
        }

        [HttpDelete]
        public IHttpActionResult DeleteNote([FromUri] int id)
        {
            var deleted = this.data.Notes.Delete(id);
            this.data.SaveChanges();
            if(deleted != null)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetDatesWithNotes(int month, int year)
        {
            if(month > 12)
            {
                return BadRequest();
            }

            var currentUserId = this.userIdProvider.GetUserId();
            var dates = this.data.Notes.All()
                                       .Where(n => (n.UserId == currentUserId) && (n.Date.Month == month) && (n.Date.Year == year))
                                       .Select(n => n.Date);
                
            return Ok(dates);
        }
    }
}