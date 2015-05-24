using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GemBox.Document;
using GemBox.Document.Tables;
using MyDiary.Desktop.ViewModels;
using System.IO;

namespace MyDiary.Desktop.Common
{
    public class FileStore
    {
        //http://www.gemboxsoftware.com/SampleExplorer/Document/Formatting/Styles?tab=cs
        public static bool SaveNotesForDay(List<NoteViewModel> notes, DateTime date, string path)
        {
            try
            {
                ComponentInfo.SetLicense("FREE-LIMITED-KEY");

                DocumentModel document = new DocumentModel()
                {

                };

                var blockItems = new List<Block>();
                ParagraphStyle title = (ParagraphStyle)Style.CreateStyle(StyleTemplateType.Title, document);
                title.CharacterFormat.Bold = true;
                title.CharacterFormat.Italic = true;
                //title.CharacterFormat.FontColor = new Color(0, 190, 33);

                CharacterFormat italic = new CharacterFormat();
                italic.Italic = true;

                CharacterFormat bold = new CharacterFormat();
                bold.Bold = true;
                bold.Italic = true;

                // First add style to the document, then use it.
                document.Styles.Add(title);
                blockItems.Add(new Paragraph(document, new Run(document, "My diary notes - "),
                            new Run(document, date.ToString("dd.MM.yyy")))
                {
                    ParagraphFormat = new ParagraphFormat()
                    {
                        Style = title
                    }
                });
                var inlineItems = new List<Inline>();
                foreach (var note in notes)
                {
                    var dateStyle = bold.Clone();
                    inlineItems.Add(new Run(document, note.Time + "  ")
                    {
                        CharacterFormat = dateStyle
                    });
                    var noteStyle = italic.Clone();
                    inlineItems.Add(new Run(document, note.NoteText)
                    {
                        CharacterFormat = noteStyle
                    });
                    inlineItems.Add(new SpecialCharacter(document, SpecialCharacterType.LineBreak));
                }

                blockItems.Add(new Paragraph(document, inlineItems));
                document.Sections.Add(new Section(document, blockItems));
                document.Save(path);
                if (File.Exists(path))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
