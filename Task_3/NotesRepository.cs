using System.Collections.Generic;
using System.Linq;

namespace Task_3
{
    public class NotesRepository
    {
        private readonly FileWorker _fileWorker;
        private List<INote> _notes;
        private bool _isNotesOutdated = true;

        public NotesRepository()
        {
            _fileWorker = new FileWorker();
        }
        
        public Response<bool> SaveNote(INote note)
        {
            return _fileWorker.SaveNote(note);
        }

        public Response<INote> GetNote(int id)
        {
            var response = UpdateNotes();
            if (!response.Success)
            {
                return Response<INote>.Error(response.Message);
            }

            if (_notes == null)
            {
                return Response<INote>.Error("No notes have been created yet");
            }

            var result = _notes.FirstOrDefault(n => n.Id == id);
            return new Response<INote>(true, result);
        }

        public Response<List<INote>> FindNotes(string query)
        {
            var response = UpdateNotes();
            if (!response.Success)
            {
                return Response<List<INote>>.Error(response.Message);
            }

            if (_notes == null)
            {
                return Response<List<INote>>.Error("No notes have been created yet");
            }

            var idEntries = _notes.Where(note => note.Id.ToString().Contains(query));
            var textEntries = _notes.Where(note => note.Text.Contains(query));
            var dateEntries = _notes.Where(note => note.CreatedOn.ToString().Contains(query));

            var result = idEntries.Union(textEntries).Union(dateEntries).ToList();
            return new Response<List<INote>>(true, result);
        }

        public Response<List<INote>> GetAllNotes()
        {
            var response = UpdateNotes();
            if (!response.Success)
            {
                return Response<List<INote>>.Error(response.Message);
            }

            if (_notes == null)
            {
                return Response<List<INote>>.Error("No notes have been created yet");
            }

            return new Response<List<INote>>(true, _notes);
        }

        private Response<List<INote>> UpdateNotes()
        {
            if (!_isNotesOutdated) 
                return new Response<List<INote>>(true, _notes, "Notes was not updated");
            
            var response = _fileWorker.GetAllNotes();
            if (response.Success)
            {
                _notes = response.Result;
            }
               
            return response;
        }
        
    }
}