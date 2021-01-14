using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Task_3
{
    public class NotesRepository
    {
        private readonly FileWorker _fileWorker;
        private List<INote> _notes = new List<INote>();
        private bool _isNotesFetched;

        public NotesRepository()
        {
            _fileWorker = new FileWorker();
        }
        
        public Response<bool> CreateNote(string noteContent)
        {
            var response = FetchNotes();
            if (!response.Success)
            {
                return Response<bool>.Error(response.Message);
            }
            
            int noteId = 0;
            
            _notes ??= new List<INote>();
            
            if (_notes.Count != 0)
            {
                noteId = _notes.Max(n => n.Id) + 1;
            }
            
            string noteTitle = noteContent.Substring(0, noteContent.Length > 32 ? 32 : noteContent.Length);
            INote newNote = new Note(noteId, noteTitle, noteContent, DateTime.UtcNow);
            
            _notes.Add(newNote);
            
            return _fileWorker.SaveNotes(_notes);
        }

        public Response<INote> GetNote(int id)
        {
            var response = FetchNotes();
            if (!response.Success)
            {
                return Response<INote>.Error(response.Message);
            }

            if (_notes == null || _notes.Count == 0)
            {
                return Response<INote>.Error("No notes have been created yet");
            }

            var result = _notes.FirstOrDefault(n => n.Id == id);
            if (result == null)
            {
                return Response<INote>.Error($"Cannot find note #{id}"); 
            }
            return new Response<INote>(true, result);

        }
        
        public Response<List<INote>> GetAllNotes()
        {
            var response = FetchNotes();
            if (!response.Success)
            {
                return Response<List<INote>>.Error(response.Message);
            }

            if (_notes == null || _notes.Count == 0)
            {
                return Response<List<INote>>.Error("No notes have been created yet");
            }

            return new Response<List<INote>>(true, _notes.OrderBy(n => n.Id).ToList());
        }
        
        public Response<List<INote>> FindNotes(string query)
        {
            var response = FetchNotes();
            if (!response.Success)
            {
                return Response<List<INote>>.Error(response.Message);
            }

            if (_notes == null || _notes.Count == 0)
            {
                return Response<List<INote>>.Error("No notes have been created yet");
            }

            var idEntries = _notes.Where(note => note.Id.ToString().Contains(query));
            var textEntries = _notes.Where(note => note.Text.Contains(query));
            var titleEntries = _notes.Where(note => note.Title.Contains(query));
            var dateEntries = _notes.Where(note => note.CreatedOn.ToString(CultureInfo.CurrentCulture).Contains(query));

            var result = idEntries.Union(textEntries)
                .Union(dateEntries)
                .Union(titleEntries)
                .OrderBy(n => n.Id).ToList();
            
            return new Response<List<INote>>(true, result);
        }

        public Response<bool> DeleteNote(int id)
        {
            var response = FetchNotes();
            if (!response.Success)
            {
                return Response<bool>.Error(response.Message);
            }

            if (_notes == null || _notes.Count == 0)
            {
                return Response<bool>.Error("No notes have been created yet");
            }

            var noteForDeletion = _notes.FirstOrDefault(n => n.Id == id);
            if (noteForDeletion == null)
            {
                return Response<bool>.Error($"Note with id {id} doesn't exist");
            }
            _notes.Remove(noteForDeletion);
            
            var saveResponse = _fileWorker.SaveNotes(_notes);
            if (saveResponse.Success)
            {
                return new Response<bool>(true, true, $"Note with id {id} was deleted");
            }

            return saveResponse;
        }

        private Response<List<INote>> FetchNotes()
        {
            if (_isNotesFetched) 
                return new Response<List<INote>>(true, _notes, "Notes already fetched");
            
            var response = _fileWorker.GetAllNotes();
            if (!response.Success) return response;
            
            if (response.Result == null)
            {
                _notes = new List<INote>();
            }
            else
            {
                _notes = response.Result.OrderBy(n => n.Id).ToList();
            }
            
            _isNotesFetched = true;
            return response;
        }
        
    }
}