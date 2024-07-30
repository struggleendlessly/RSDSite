using shared.Models;

namespace shared.Helpers
{
    public static class VisibilityHelpers
    {
        public static bool IsVisible(PageModel model, string key)
        {
            return !model.Data.TryGetValue(key + StaticStrings.IsVisibleKeyEnding, out var value) || bool.Parse(value);
        }
    }
}
