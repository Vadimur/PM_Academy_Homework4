using System;

namespace Task_3
{
    public interface INote
    {
        int Id { get; } 
        string Title { get; } 
        string Text { get; } 
        DateTime CreatedOn { get; }

        string ToShortString();
    }
}