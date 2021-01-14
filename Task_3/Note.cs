using System;
using System.Globalization;

namespace Task_3
{
    public class Note : INote
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime CreatedOn { get; set; }

        public Note()
        {
            
        }
        public Note(int id, string title, string text, DateTime createdOn)
        {
            Id = id;
            Title = title;
            Text = text;
            CreatedOn = createdOn;
        }

        public string ToShortString()
        {
            return $"Note #{Id}\n" +
                   $"Title: {Title}\n" +
                   $"Created: {CreatedOn.ToString(CultureInfo.CurrentCulture)}\n";
        }

        public override string ToString()
        {
            return $"Note #{Id}\n" +
                   $"Title: {Title}\n" +
                   $"Created: {CreatedOn.ToString(CultureInfo.CurrentCulture)}\n\n" +
                   $"{Text}\n";
        }
    }
}