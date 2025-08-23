using ExitGames.Client.Photon;

internal static class TeamExtensions
{
	public static PunTeams.Team GetTeam(this PhotonPlayer player)
	{
		if (player.customProperties.TryGetValue("team", out var value))
		{
			return (PunTeams.Team)(byte)value;
		}
		return PunTeams.Team.none;
	}

	public static void SetTeam(this PhotonPlayer player, PunTeams.Team team)
	{
		_ = PhotonNetwork.connectedAndReady;
		if (PhotonNetwork.player.GetTeam() != team)
		{
			Hashtable hashtable = new Hashtable();
			hashtable.Add("team", (byte)team);
			PhotonNetwork.player.SetCustomProperties(hashtable);
		}
	}
}
