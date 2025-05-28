namespace DotNetNote.Services
{
    public class CopyrightService : ICopyrightService
    {
        public string GetCopyrightString()
        {
            return $"Copyright © {DateTime.Now.Year}. All rights reserved."
                + $" from CopyrightService. Hash : {GetHashCode()}";
        }
    }
}
