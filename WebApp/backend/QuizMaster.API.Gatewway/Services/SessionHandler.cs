using QuizMaster.API.Gateway.Hubs;

namespace QuizMaster.API.Gateway.Services
{
    public class SessionHandler
    {
        private IDictionary<string, string> connectionGroupPair;

        public SessionHandler()
        {
            connectionGroupPair = new Dictionary<string, string>();
        }

        public async Task AddToGroup(SessionHub hub, string group, string connectionId)
        {
            connectionGroupPair[connectionId] = group;
            await hub.Groups.AddToGroupAsync(connectionId, group);
        }

        public async Task RemoveFromGroup(SessionHub hub, string group, string connectionId)
        {
            await hub.Groups.RemoveFromGroupAsync(connectionId, group);
            connectionGroupPair.Remove(connectionId);
        }

        public async Task RemoveGroup(SessionHub hub, string group)
        {
            var connectionIds = connectionGroupPair.Where(kv => kv.Value.Equals(group)).Select(kv => kv.Key).ToList();
            foreach(var connectionId in connectionIds)
            {
                await hub.Groups.RemoveFromGroupAsync(connectionId, group);
                connectionGroupPair.Remove(connectionId);
            }
        }
    }
}
