namespace BookStoreApp.Models.Author
{
    public class AuthorReadDto : BaseDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Bio { get; set; }
    }
}
