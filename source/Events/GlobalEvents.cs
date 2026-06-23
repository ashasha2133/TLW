namespace Events;

public static class GlobalEvents
{
	public delegate void OnGameStateResetHandler();

	public static event OnGameStateResetHandler OnGameStateReset;

	public static void DispatchOnGameStateReset()
	{
		GlobalEvents.OnGameStateReset?.Invoke();
	}
}
