using System.ComponentModel.DataAnnotations;
public class Author
{
    public int AuthorId { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [StringLength(2000)]
    public string Biography { get; set; }
    public List<Book> Books { get; set; }
}