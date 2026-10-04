public class Post
{
    // Identificador del recurso en el servidor
    public int Id { get; set; }

    // Título del post
    public string Title { get; set; } = string.Empty;

    // Contenido del post
    public string Body { get; set; } = string.Empty;
}