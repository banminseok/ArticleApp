namespace DotNetNote.Models.Urls
{
    public interface IUrlRepository
    {
        bool IsExists(string email);
    }
}
