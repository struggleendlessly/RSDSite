using shared.Models;

namespace shared.Helpers
{
    public static class VisibilityHelpers
    {
        public static bool IsVisible(Dictionary<string, string> dictionary, string key)
        {
            return !dictionary.TryGetValue(key + StaticStrings.IsVisibleKeyEnding, out var value) || bool.Parse(value);
        }

        public static List<ServiceItem> GetVisibleServiceItems(List<ServiceItem> serviceItems, int maxVisibleItemsCount)
        {
            var visibleServiceItems = new List<ServiceItem>();

            foreach (var serviceItem in serviceItems)
            {
                var serviceKey = serviceItem.ShortDesc.FirstOrDefault().Key;
                var isVisible = IsVisible(serviceItem.ShortDesc, serviceKey);
                if (isVisible)
                {
                    visibleServiceItems.Add(serviceItem);
                }

                if (visibleServiceItems.Count >= maxVisibleItemsCount)
                {
                    break;
                }
            }

            return visibleServiceItems;
        }
    }
}
