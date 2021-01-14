using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Task_3
{
    public class FileWorker
    {
        private const string Path = "notes.json";
        
        public Response<List<INote>> GetAllNotes()
        {
            string notesJson;
            try
            {
               // rawNotes = File.ReadLines(Path).ToList();
               notesJson = File.ReadAllText(Path);
            }
            catch (UnauthorizedAccessException)
            {
                return Response<List<INote>>.Error($"Unauthorized access to the file {Path}");
            }
            catch (FileNotFoundException)
            {
                using FileStream fs = File.Create(Path);
                
                return new Response<List<INote>>(true, null, "No notes have been created yet");
            }
            catch (SecurityException)
            {
                return Response<List<INote>>.Error("Cannot open file with notes because the caller does not have the required permission.");
            }
            catch (IOException)
            {
                return Response<List<INote>>.Error($"I/O error occured while reading {Path}");
            }

            try
            {
                if (string.IsNullOrWhiteSpace(notesJson))
                {
                    return new Response<List<INote>>(true, null);
                }
                // notes.AddRange(JsonSerializer.Deserialize<Note>(rawNotes));
                IEnumerable<INote> notes = JsonSerializer.Deserialize<List<Note>>(notesJson);
                //notes.AddRange(rawNotes);
                return new Response<List<INote>>(true, notes.ToList());
            }
            catch (ArgumentNullException)
            {
                return Response<List<INote>>.Error("Storage with notes has been damaged.");
            }
            catch (JsonException)
            {
                return Response<List<INote>>.Error("Storage with notes has been damaged.");
            }
            
        }

        public Response<bool> SaveNotes(List<INote> notes)
        {
            string noteJson = JsonSerializer.Serialize(notes);
            try
            {
                File.WriteAllText(Path, noteJson);
            }
            catch (UnauthorizedAccessException)
            {
                return Response<bool>.Error($"Unauthorized access to the {Path}");
            }
            catch (SecurityException)
            {
                return Response<bool>.Error("Cannot open/write file with notes because the caller does not have the required permission.");
            }
            catch (IOException)
            {
                return Response<bool>.Error($"I/O error occured while writing to the {Path}");
            }
            return new Response<bool>(true, true, "Note list was updated");
        }

    }
}