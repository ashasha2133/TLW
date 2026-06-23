using System.Collections.Generic;
using System.Linq;

namespace Utility;

public class FeedManager
{
	private List<FeedEntry> _feedEntries = new List<FeedEntry>();

	private List<FeedEntry> _lastMaxFeedEntries = new List<FeedEntry>();

	private int _lastMaxFeed;

	public void Reset()
	{
		_feedEntries.Clear();
		_lastMaxFeedEntries.Clear();
		_lastMaxFeed = 0;
	}

	public void AddEntry(float currentRoundTime, int damage, string chatEntry, int index)
	{
		_feedEntries.Add(new FeedEntry
		{
			RoundTime = currentRoundTime,
			ChatElement = chatEntry,
			Damage = damage,
			Index = index
		});
	}

	public KeyValuePair<int, int> Iterate(float currentRoundTime)
	{
		_feedEntries = _feedEntries.Where((FeedEntry e) => currentRoundTime - e.RoundTime >= 0f && currentRoundTime - e.RoundTime <= 8f).ToList();
		int num = 0;
		foreach (FeedEntry feedEntry in _feedEntries)
		{
			num += feedEntry.Damage;
		}
		if (num > _lastMaxFeed)
		{
			List<FeedEntry> feedEntries = _feedEntries.Select((FeedEntry e) => new FeedEntry
			{
				RoundTime = e.RoundTime,
				Damage = e.Damage,
				ChatElement = e.ChatElement,
				Index = e.Index
			}).ToList();
			if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE || (int)FengGameManagerMKII.settings[244] == 1)
			{
				InRoomChat.TryHighlightFeed(feedEntries, _lastMaxFeedEntries);
			}
			else if ((int)FengGameManagerMKII.settings[244] == 2)
			{
				FleurConsole.TryHighlightFeed(feedEntries, _lastMaxFeedEntries);
			}
			_lastMaxFeed = num;
			_lastMaxFeedEntries.Clear();
			_lastMaxFeedEntries = _feedEntries.Select((FeedEntry e) => new FeedEntry
			{
				RoundTime = e.RoundTime,
				Damage = e.Damage,
				ChatElement = e.ChatElement,
				Index = e.Index
			}).ToList();
		}
		return new KeyValuePair<int, int>(num, _feedEntries.Count());
	}
}
