using System;

namespace Task_3
{
    public class NotesWorker
    {
        private bool _keepProgramActive = true;
        private readonly NotesRepository _repository;
        public NotesWorker()
        {
            _repository = new NotesRepository();
        }
        public void Start()
        {
            do
            {
                PrintUserMenu();
                Console.Write("Enter command: ");
                string userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("Empty input. Try again\n");
                    continue;
                }

                if (int.TryParse(userInput, out int commandId) == false)
                {
                    Console.WriteLine("Unknown command id. Try again\n");
                    continue;
                }

                bool isCommandValid = ChooseCommand(commandId);
                if (isCommandValid == false)
                {
                    Console.WriteLine("Unknown command id. Try again\n");
                }
                
            } while (_keepProgramActive);
        }
        
        private bool ChooseCommand(int commandNumber)
        {
            bool correctCommand = true;
            switch (commandNumber)
            {
                case 1:
                    FindNoteByQuery();
                    break;
                case 2:
                    FindNoteById();
                    break;
                case 3: 
                    CreateNote();
                    break;
                case 4: 
                    DeleteNote(); 
                    break;
                case 5:
                    Exit();
                    break;
                default:
                    correctCommand = false;
                    break;
            }
            return correctCommand;
        }
        
        private void FindNoteByQuery()
        {
            Console.Write("Enter search query: ");
            string query = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(query))
            {
                ShowAllNotesShortForm();
            }
            else
            {
                var notesResponse = _repository.FindNotes(query);
                if (!notesResponse.Success)
                {
                    Console.WriteLine($"Error occured: {notesResponse.Message}");
                    return;
                }
                
                if (notesResponse.Message != null)
                    Console.WriteLine(notesResponse.Message);
                
                if (notesResponse.Result != null  && notesResponse.Result.Count != 0)
                {
                    foreach (var note in notesResponse.Result)
                    {
                        Console.WriteLine(note.ToShortString());
                    }
                }
                else
                {
                    Console.WriteLine("Result set is empty");
                }
            }
        }
        
        private void ShowAllNotesShortForm()
        {
            var allNotesResponse = _repository.GetAllNotes();
            if (!allNotesResponse.Success)
            {
                Console.WriteLine($"Error occured: {allNotesResponse.Message}");
                return;
            }
                
            if (allNotesResponse.Message != null)
                Console.WriteLine(allNotesResponse.Message);
                
            if (allNotesResponse.Result != null && allNotesResponse.Result.Count != 0)
            {
                foreach (var note in allNotesResponse.Result)
                {
                    Console.WriteLine(note.ToShortString());
                }
            }
            else
            {
                Console.WriteLine("Result set is empty");
            }
        }
        
        private void FindNoteById()
        {
            string rawId;
            int id;
            
            do
            {
                Console.Write("Enter note ID: ");
                rawId = Console.ReadLine();
            } while (int.TryParse(rawId, out id) == false);

            var response = _repository.GetNote(id);
            if (!response.Success)
            {
                Console.WriteLine($"Error occured: {response.Message}");
                return;
            }

            if (response.Message != null)
                Console.WriteLine(response.Message);

            Console.WriteLine(response.Result == null ? "Result set is empty" : response.Result.ToString());
        }
        
        private void CreateNote()
        {
            Console.WriteLine("\nEnter new note:");
            string newNoteContent = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(newNoteContent))
            {
                Console.WriteLine("Note wasn't saved");
                return;
            }

            var response = _repository.CreateNote(newNoteContent.Trim());
            if (!response.Success)
            {
                Console.WriteLine($"Error occured: {response.Message}");
                return;
            }
                
            if (response.Message != null)
                Console.WriteLine(response.Message);
        }
        
        private void DeleteNote()
        {
            string rawId;
            int id;
            
            do
            {
                Console.Write("Enter note ID: ");
                rawId = Console.ReadLine();
            } while (int.TryParse(rawId, out id) == false);

            var findResponse = _repository.GetNote(id);
            if (!findResponse.Success)
            {
                Console.WriteLine($"Error occured: {findResponse.Message}");
                return;
            }

            if (findResponse.Message != null)
                Console.WriteLine(findResponse.Message);

            if (findResponse.Result == null)
            {
                Console.WriteLine("Error occured");
                return;
            }
            
            Console.WriteLine(findResponse.Result.ToString());

            Console.WriteLine($"Are you sure you want delete note #{id}? ");
            Console.Write("y/[n]: ");
            string answer = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(answer) || !answer.ToLower().Equals("y")) return;
            
            var response = _repository.DeleteNote(id);
            if (!response.Success)
            {
                Console.WriteLine($"Error occured: {response.Message}");
                return;
            }

            if (response.Message != null)
                Console.WriteLine(response.Message);

        }
        
        private void Exit()
        {
            _keepProgramActive = false;
        }

        private void PrintUserMenu()
        {
            Console.WriteLine();
            Console.WriteLine("1. Find notes by search query");
            Console.WriteLine("2. Find note by ID");
            Console.WriteLine("3. Create new note");
            Console.WriteLine("4. Delete note by ID");
            Console.WriteLine("5. Exit");   
        }
        
    }
}