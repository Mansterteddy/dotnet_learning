
namespace PluginBase
{
    public interface ICommand
    {
        string Name { get; }
        string Description { get; }

        int InitEngine();
    }
}