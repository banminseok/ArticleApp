using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetNote.Models
{
    public interface INoteCommentRepository
    {
        void AddNoteComment(NoteComment model);
        int DeleteNoteComment(int boardId, int id, string password);
        int GetCountBy(int boardId, int id, string password);
        List<NoteComment> GetNoteComments(int boardId);
        List<NoteComment> GetRecentComments();
        List<NoteComment> GetRecentCommentsNoCache();
    }
}
