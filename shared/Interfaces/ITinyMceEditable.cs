using shared.Models;

namespace shared.Interfaces
{
    public interface ITinyMceEditable
    {
        PageModel Model { get; set; }
        void Save();
    }
}
