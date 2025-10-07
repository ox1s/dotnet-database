namespace Options
{
    public interface IMenuOption
    {
        string Name { get; }
        void Execute();
    }
}