namespace Domain.Security
{
    public interface IHasher
    {
        string Hash(string input);
        bool Verify(string input, string hashed);
    }
}
