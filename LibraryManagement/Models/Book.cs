using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace LibraryManagement.Models;

public class Book
{
    public int Id { get; set; }
    // public int UserId { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public DateTime PublishedDate { get; set; }
}
