using shared.Models;

namespace shared.Interfaces
{
    public interface IEditable
    {
        PageModel Model { get; set; }
        void Save();
    }
}
