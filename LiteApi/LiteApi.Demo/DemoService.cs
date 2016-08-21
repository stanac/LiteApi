namespace LiteApi.Demo
{
    public interface IDemoService
    {
        string Add(string a, string b);
        int Add(int a, int b);
    }

    public class DemoService : IDemoService
    {
        public int Add(int a, int b)
            => a + b;

        public string Add(string a, string b)
            => $"{a} {b}";
    }
}
