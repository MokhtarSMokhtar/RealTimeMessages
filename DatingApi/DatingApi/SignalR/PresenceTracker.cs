using System;
namespace DatingApi.SignalR
{
	public class PresenceTracker
	{
		private static Dictionary<string, List<string>> OnlineUser = new Dictionary<string, List<string>>();

		public PresenceTracker()
		{
		}

		public Task UserConnected(string userName,string connectionId)
		{
			lock(OnlineUser)
			{
				if(OnlineUser.ContainsKey(userName))
				{
					OnlineUser[userName].Add(connectionId);

                }
				else
				{
                    OnlineUser.Add(userName,new List<string> { connectionId });
                }
				return Task.CompletedTask;
			}
		}


		public Task DisconnectedUser(string userName, string connectionId)
		{
			lock (OnlineUser)
			{
                if (OnlineUser.ContainsKey(userName))
                    return Task.CompletedTask;

				OnlineUser[userName].Remove(connectionId);

				if (OnlineUser[userName].Count == 0)
				{
					OnlineUser.Remove(userName);
				}
            }

			return Task.CompletedTask;

		}

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;

            lock (OnlineUser)
            {
				onlineUsers = OnlineUser.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

			return Task.FromResult(onlineUsers);
        }
    }


}

