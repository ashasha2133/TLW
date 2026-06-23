using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using ExitGames.Client.Photon;
using Photon;
using UnityEngine;
using Utility;

public class InRoomChat : Photon.MonoBehaviour
{
	public enum LANGUAGE
	{
		en,
		es,
		pt,
		ru,
		tl,
		th,
		fr,
		it,
		tr
	}

	private string chatname = "";

	private string chatcolor = "";

	private string prefix = "";

	private string suffix = "";

	private static bool isShowCommandList;

	private Rect rectCommands = new Rect(0f, 0f, 0f, 0f);

	private Vector2 commandsScroll = new Vector2(0f, myYScroll);

	private static int myYScroll;

	private bool AlignBottom = true;

	public static Rect GuiRect;

	public static Rect GuiRect2;

	private string inputLine = string.Empty;

	public static List<string> messages;

	private Vector2 scrollPos = Vector2.zero;

	private string key = string.Empty;

	private static GUIStyle currentStyle;

	public static InRoomChat ChatInstanse;

	private static readonly Dictionary<string, MethodInfo> CommandsCache;

	public bool IsVisible = true;

	public Stopwatch timeOfFade = new Stopwatch();

	private static bool MessageHistoryUpdatecache;

	private static string MessageHistoryCache;

	private static Dictionary<string, string> CommandList;

	public AudioSource Typing;

	public static string GetCompiledMessages
	{
		get
		{
			if (MessageHistoryUpdatecache)
			{
				MessageHistoryUpdatecache = false;
				MessageHistoryCache = string.Empty;
				if (messages.Count < FengGameManagerMKII.instance.ChatMessages)
				{
					for (int i = 0; i < messages.Count; i++)
					{
						MessageHistoryCache = MessageHistoryCache + messages[i] + Environment.NewLine;
					}
				}
				else
				{
					for (int j = messages.Count - FengGameManagerMKII.instance.ChatMessages; j < messages.Count; j++)
					{
						MessageHistoryCache = MessageHistoryCache + messages[j] + Environment.NewLine;
					}
				}
			}
			return MessageHistoryCache;
		}
	}

	private void Style()
	{
		if (currentStyle == null)
		{
			currentStyle = new GUIStyle(GUI.skin.box);
			currentStyle.alignment = TextAnchor.MiddleLeft;
			currentStyle.wordWrap = true;
			currentStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.2f));
		}
	}

	private Texture2D MakeTex(int width, int height, Color col)
	{
		Color[] array = new Color[width * height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = col;
		}
		Texture2D texture2D = new Texture2D(width, height);
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}

	static InRoomChat()
	{
		isShowCommandList = false;
		myYScroll = 0;
		GuiRect = new Rect(0f, 100f, 300f, 470f);
		GuiRect2 = new Rect(30f, 575f, 300f, 25f);
		messages = new List<string>();
		CommandsCache = new Dictionary<string, MethodInfo>();
		MessageHistoryUpdatecache = true;
		MessageHistoryCache = string.Empty;
		CommandList = new Dictionary<string, string>
		{
			{ "<color=orange><b>MC Commands</b></color>", "" },
			{ "<color=red>/r</color>; <color=red>/restart</color>", " (restarts the game) [MC]" },
			{ "<color=red>/room max n</color>", " (changes room max players) [MC]" },
			{ "<color=red>/rev ID</color>; <color=red>/revive ID</color>", " (revives the person) [MC]" },
			{ "<color=red>/revall</color>; <color=red>/reviveall</color>", " (revives everyone in server) [MC]" },
			{ "<color=red>/resetteam</color>", " (resets team round wins) [MC]" },
			{ "<color=red>/resetkdall</color>", " (puts on 0 score of everyone) [MC]" },
			{ "<color=red>/kick ID</color>", " (kicks person from room) [MC]" },
			{ "<color=red>/ban ID</color>", " (bans the person and adds him to banlist) [MC]" },
			{ "<color=red>/unban ID</color>", " (unbans the person and removes him from banlist) [MC]" },
			{ "<color=red>/move ID 0/1/2</color>", " (moves ID into the team, convenient to set teams for team duels) [MC]" },
			{ "<color=orange><b>ID Commands</b></color>", "" },
			{ "<color=yellow>/mute ID</color>", " (mutes player, but he can see your msg, AND you can see his movements, kills etc)" },
			{ "<color=yellow>/unmute ID</color>", " (unmutes player)" },
			{ "<color=yellow>/ign ID</color>", " (ignores player, not just mute but also wont see his movements or kills, the ban contains it)" },
			{ "<color=yellow>/uni ID</color>", " (unignores player, useful in case of false positive in local ignore anti)" },
			{ "<color=yellow>/name ID</color>", " (gives color you want to the person to see them better, choose color in Video panel" },
			{ "<color=yellow>/unname ID</color>", " (removes the set color from the player's name" },
			{ "<color=yellow>/mark ID</color>", " (replaces person's guild with red [X] so you can target them in FFA or make them more visible" },
			{ "<color=yellow>/unmark ID</color>", " (removes target from marked players" },
			{ "<color=yellow>/pm ID message</color>", " (private message someone)" },
			{ "<color=orange><b>Utility Commands</b> (No MC)</color>", "" },
			{ "<color=yellow>/resetkd</color>", " (puts on 0 all your scores)" },
			{ "<color=yellow>/specmode</color>", " (goes into specmode, swap players with keys 1-2, do command again to spawn again)" },
			{ "<color=yellow>/ignorelist</color>", " (see ignorest list to see everyone you ignored locally in case of an anti)" },
			{ "<color=yellow>/team 0/1/2</color>", " (in team game, changes your team)" },
			{ "<color=yellow>/clear</color>", " (clears your chat and console)" },
			{ "<color=yellow>/j</color>", " (autorejoins the room, will also randomize your name as it fakes being disconnected)" },
			{ "<color=yellow>/menu</color>", " (to open menu, for people who unbound their menu on 'None' key or put it on key like Esc which autocloses the mod)" }
		};
		MethodInfo[] methods = typeof(InRoomChat).GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		Type typeFromHandle = typeof(CommandAttribute);
		MethodInfo[] array = methods;
		foreach (MethodInfo methodInfo in array)
		{
			object[] customAttributes = methodInfo.GetCustomAttributes(typeFromHandle, inherit: false);
			if (customAttributes == null || customAttributes.Length == 0)
			{
				continue;
			}
			object[] array2 = customAttributes;
			for (int j = 0; j < array2.Length; j++)
			{
				if (array2[j] is CommandAttribute commandAttribute)
				{
					if (commandAttribute.Name == null)
					{
						CommandsCache.Add(methodInfo.Name.ToLower(), methodInfo);
					}
					else
					{
						CommandsCache.Add(commandAttribute.Name.ToLower(), methodInfo);
					}
				}
			}
		}
	}

	private void Awake()
	{
		ChatInstanse = this;
	}

	public void addLINE(string newLine)
	{
		Write(newLine);
	}

	public static int Write(string newLine)
	{
		if (messages.Count >= 100)
		{
			messages.RemoveAt(0);
		}
		MessageHistoryUpdatecache = true;
		messages.Add(newLine.FilterSizeTag());
		if (!ChatInstanse.IsVisible)
		{
			ChatInstanse.IsVisible = true;
		}
		ChatInstanse.timeOfFade.Reset();
		ChatInstanse.timeOfFade.Start();
		return messages.Count - 1;
	}

	public static void TryHighlightFeed(List<FeedEntry> feedEntries, List<FeedEntry> previousFeedEntries)
	{
		MessageHistoryUpdatecache = true;
		int num = Math.Max(messages.Count - FengGameManagerMKII.instance.ChatMessages, 0);
		foreach (FeedEntry previousFeedEntry in previousFeedEntries)
		{
			if (previousFeedEntry.Index >= num && previousFeedEntry.Index < messages.Count)
			{
				string text = messages[previousFeedEntry.Index].Replace("<b>", string.Empty).Replace("</b>", string.Empty);
				if (text == previousFeedEntry.ChatElement)
				{
					messages[previousFeedEntry.Index] = text;
				}
			}
		}
		foreach (FeedEntry feedEntry in feedEntries)
		{
			if (feedEntry.Index >= num && feedEntry.Index < messages.Count && messages[feedEntry.Index] == feedEntry.ChatElement)
			{
				messages[feedEntry.Index] = "<b>" + messages[feedEntry.Index] + "</b>";
			}
		}
		if (!ChatInstanse.IsVisible)
		{
			ChatInstanse.IsVisible = true;
		}
		ChatInstanse.timeOfFade.Reset();
		ChatInstanse.timeOfFade.Start();
	}

	public static void Chat(string msg, string sender = "")
	{
		FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, msg, sender);
		ChatInstanse.timeOfFade.Reset();
		ChatInstanse.timeOfFade.Start();
	}

	private void ShowMessageWindow()
	{
		if (IsVisible)
		{
			GUI.SetNextControlName(string.Empty);
			if (GameSettings.ChatBackground > 0)
			{
				Style();
			}
			GUILayout.BeginArea(GuiRect);
			GUILayout.FlexibleSpace();
			GUILayout.Label(GetCompiledMessages, (GameSettings.ChatBackground > 0) ? currentStyle : ((GUIStyle)""));
			GUILayout.EndArea();
		}
	}

	private void TranslateOutgoingMessages(string text, bool PM, int ID, string str3)
	{
		int num = (int)FengGameManagerMKII.settings[329];
		LANGUAGE lANGUAGE = (LANGUAGE)num;
		Enum.GetName(typeof(LANGUAGE), lANGUAGE);
		Action<string, string> callback = delegate(string translation, string errors)
		{
			if (!errors.IsNullOrEmpty())
			{
				Write(errors);
			}
			if (!PM)
			{
				object[] parameters = new object[2]
				{
					string.Concat(" <color=#", chatcolor, ">" + prefix, translation, suffix, "</color>"),
					chatname
				};
				FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
				timeOfFade.Reset();
				timeOfFade.Start();
			}
			else
			{
				PhotonPlayer photonPlayer = PhotonPlayer.Find(ID);
				if (photonPlayer != null)
				{
					object[] parameters2 = new object[2]
					{
						RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]).hexColor(),
						translation
					};
					FengGameManagerMKII.instance.photonView.RPC("ChatPM", photonPlayer, parameters2);
					timeOfFade.Reset();
					timeOfFade.Start();
					Write("<color=#FFC000>TO [" + photonPlayer.ID + "]</color> " + str3 + ":" + translation);
				}
			}
		};
		StartCoroutine(ThymesUtils.TranslateText(text, num, callback));
	}

	public void OnGUI()
	{
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE && (int)FengGameManagerMKII.settings[244] == 1)
		{
			ShowMessageWindow();
		}
		if (PhotonNetwork.connectionStateDetailed != PeerStates.Joined)
		{
			return;
		}
		switch (Event.current.type)
		{
		case EventType.KeyUp:
			if (Event.current.keyCode != KeyCode.None && Event.current.keyCode == FengGameManagerMKII.inputRC.humanKeys[InputCodeRC.chat] && GUI.GetNameOfFocusedControl() != "ChatInput")
			{
				inputLine = string.Empty;
				GUI.FocusControl("ChatInput");
			}
			break;
		case EventType.KeyDown:
		{
			KeyCode keyCode = Event.current.keyCode;
			if (keyCode == KeyCode.Return || keyCode == KeyCode.KeypadEnter)
			{
				if (!string.IsNullOrEmpty(inputLine))
				{
					if (inputLine == "\t")
					{
						inputLine = string.Empty;
						GUI.FocusControl(string.Empty);
						return;
					}
					if (FengGameManagerMKII.RCEvents.ContainsKey("OnChatInput"))
					{
						string text = (string)FengGameManagerMKII.RCVariableNames["OnChatInput"];
						if (FengGameManagerMKII.stringVariables.ContainsKey(text))
						{
							FengGameManagerMKII.stringVariables[text] = inputLine;
						}
						else
						{
							FengGameManagerMKII.stringVariables.Add(text, inputLine);
						}
						((RCEvent)FengGameManagerMKII.RCEvents["OnChatInput"]).checkEvent();
					}
					if (!inputLine.StartsWith("/"))
					{
						string text2 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]).hexColor();
						if (text2 == string.Empty)
						{
							text2 = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]);
							if (PhotonNetwork.player.customProperties["RCteam"] != null)
							{
								if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]) == 1)
								{
									text2 = "<color=#00FFFF>" + text2 + "</color>";
								}
								else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]) == 2)
								{
									text2 = "<color=#FA8072>" + text2 + "</color>";
								}
							}
						}
						if ((int)FengGameManagerMKII.settings[324] == 0)
						{
							object[] parameters = new object[2]
							{
								string.Concat(" <color=#", chatcolor, ">" + prefix, (GameSettings.chatStyle == 0) ? inputLine : ((GameSettings.chatStyle == 1) ? ("<b>" + inputLine + "</b>") : ((GameSettings.chatStyle == 2) ? ("<i>" + inputLine + "</i>") : ("<b><i>" + inputLine + "</i></b>"))), suffix, "</color>"),
								(GameSettings.chatNameStyle == 0) ? chatname : ((GameSettings.chatNameStyle == 1) ? ("<b>" + chatname + "</b>") : ((GameSettings.chatNameStyle == 2) ? ("<i>" + chatname + "</i>") : ("<b><i>" + chatname + "</i></b>")))
							};
							FengGameManagerMKII.instance.photonView.RPC("Chat", PhotonTargets.All, parameters);
							timeOfFade.Reset();
							timeOfFade.Start();
						}
						else
						{
							TranslateOutgoingMessages(inputLine, PM: false, 0, "");
						}
					}
					else
					{
						string[] array = inputLine.Substring(1).Split(' ');
						if (CommandsCache.TryGetValue(array[0], out var value))
						{
							if (value.IsStatic)
							{
								value.Invoke(null, new object[1] { array });
							}
							else
							{
								value.Invoke(this, new object[1] { array });
							}
						}
						else
						{
							Write("No such command as " + array[0]);
						}
					}
					inputLine = string.Empty;
					GUI.FocusControl(string.Empty);
					return;
				}
				inputLine = "\t";
				GUI.FocusControl("ChatInput");
			}
			else if ((Event.current.keyCode == KeyCode.Tab || Event.current.character == '\t') && !IN_GAME_MAIN_CAMERA.isPausing && FengGameManagerMKII.inputRC.humanKeys[InputCodeRC.chat] != KeyCode.Tab)
			{
				Event.current.Use();
			}
			break;
		}
		}
		if (!IsVisible)
		{
			return;
		}
		GUI.SetNextControlName(string.Empty);
		if (GameSettings.ChatBackground > 0)
		{
			Style();
		}
		GUILayout.BeginArea(GuiRect);
		GUILayout.FlexibleSpace();
		GUILayout.Label(GetCompiledMessages, (GameSettings.ChatBackground > 0) ? currentStyle : ((GUIStyle)"Label"));
		GUILayout.EndArea();
		GUILayout.BeginArea(GuiRect2);
		GUILayout.BeginHorizontal();
		GUI.SetNextControlName("ChatInput");
		inputLine = GUILayout.TextField(inputLine, GUILayout.MaxWidth(280f));
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
		if (!isShowCommandList || GameSettings.CMDHelper <= 0)
		{
			return;
		}
		Screen.lockCursor = false;
		GUILayout.BeginArea(rectCommands);
		GUILayout.FlexibleSpace();
		GUILayout.Label(string.Concat("<color=#" + FengGameManagerMKII.alternativefinalhex() + ">[Available commands]</color>"), (GameSettings.ChatBackground > 0) ? currentStyle : ((GUIStyle)"Label"));
		commandsScroll = GUILayout.BeginScrollView(commandsScroll);
		foreach (KeyValuePair<string, string> command in CommandList)
		{
			if (command.Key.ToLower().Contains(inputLine))
			{
				GUILayout.Label(command.Key + command.Value, (GameSettings.ChatBackground > 0) ? currentStyle : ((GUIStyle)"Label"));
			}
		}
		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	public void setPosition()
	{
		if (AlignBottom)
		{
			GuiRect = new Rect(5f, Screen.height - 500, 330f, 470f);
			GuiRect2 = new Rect(30f, Screen.height - 300 + 275, 300f, 25f);
			rectCommands = new Rect(330f, (float)Screen.height - 270f, 384f, 200f);
		}
	}

	public void Start()
	{
		if (ChatInstanse == null)
		{
			ChatInstanse = this;
		}
		timeOfFade.Start();
		setPosition();
		key = inputLine;
		Typing = Camera.main.gameObject.AddComponent<AudioSource>();
		Typing.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("type");
		Typing.volume = FengGameManagerMKII.InterfaceVolume;
		chatname = (((int)FengGameManagerMKII.settings[359] == 0) ? FengGameManagerMKII.Chatname : FengGameManagerMKII.Chatname2);
		chatcolor = (((int)FengGameManagerMKII.settings[359] == 0) ? FengGameManagerMKII.Chatcolor : FengGameManagerMKII.Chatcolor2);
		prefix = (((int)FengGameManagerMKII.settings[359] == 0) ? FengGameManagerMKII.Prefix : FengGameManagerMKII.Prefix2);
		suffix = (((int)FengGameManagerMKII.settings[359] == 0) ? FengGameManagerMKII.Suffix : FengGameManagerMKII.Suffix2);
	}

	private void Update()
	{
		if ((Input.GetKeyDown(KeyCode.Return) || FengGameManagerMKII.inputRC.isInputExtras(InputCodeRC.showconsole)) && !IsVisible)
		{
			IsVisible = true;
			timeOfFade.Reset();
			timeOfFade.Start();
		}
		if (timeOfFade.IsRunning)
		{
			if (timeOfFade.ElapsedMilliseconds >= 1000 * FengGameManagerMKII.instance.ChatFadeTime && IsVisible)
			{
				IsVisible = false;
			}
			if (timeOfFade.ElapsedMilliseconds < 1000 * FengGameManagerMKII.instance.ChatFadeTime && !IsVisible)
			{
				IsVisible = true;
			}
		}
		if (key != inputLine)
		{
			timeOfFade.Reset();
			timeOfFade.Start();
			if (!Typing.isPlaying && GameSettings.Typing > 0 && AudioListener.volume > 0f && FengGameManagerMKII.InterfaceVolume > 0f)
			{
				Typing.Play();
			}
			key = inputLine;
		}
		if (GameSettings.CMDHelper <= 0)
		{
			return;
		}
		if (isShowCommandList && Input.GetAxis("Mouse ScrollWheel") > 0f)
		{
			timeOfFade.Reset();
			timeOfFade.Start();
			commandsScroll = new Vector2(0f, myYScroll -= 50);
		}
		else if (isShowCommandList && Input.GetAxis("Mouse ScrollWheel") < 0f)
		{
			timeOfFade.Reset();
			timeOfFade.Start();
			commandsScroll = new Vector2(0f, myYScroll += 50);
		}
		if (inputLine.StartsWith("/"))
		{
			if (!isShowCommandList)
			{
				isShowCommandList = true;
			}
			if (timeOfFade.IsRunning)
			{
				timeOfFade.Stop();
			}
		}
		else if (isShowCommandList)
		{
			isShowCommandList = false;
			if (!timeOfFade.IsRunning)
			{
				timeOfFade.Start();
			}
		}
	}

	[Command("stop")]
	private void Stop(string[] args)
	{
		StopAllCoroutines();
		Write("<color=#FFCC00>Stopped</color>");
	}

	[Command("move")]
	private void Move(string[] args)
	{
		char[] separator = new char[1] { ' ' };
		PhotonPlayer targetPlayer = PhotonPlayer.Find(Convert.ToInt32(inputLine.Split(separator)[1]));
		if (PhotonNetwork.isMasterClient)
		{
			if (GameSettings.teamMode > 0)
			{
				FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", targetPlayer, Convert.ToInt32(inputLine.Split(separator)[2]));
			}
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[Command("white")]
	[Command("name")]
	private void White(string[] args)
	{
		if (args.Length > 1)
		{
			if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
			{
				FengGameManagerMKII.instance.whiteNameList.Add(value.ID);
				FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
				Write("<color=#FFCC00>Used /name ID </color>" + value.ID);
			}
		}
		else
		{
			Write("<color=#FFCC00>Usage: /name <ID> </color>");
		}
	}

	[Command("Unname")]
	private void UnWhite(string[] args)
	{
		if (args.Length > 1)
		{
			if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
			{
				FengGameManagerMKII.instance.whiteNameList.Remove(value.ID);
				FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
				Write("<color=#FFCC00>Used /unname ID </color>" + value.ID);
			}
		}
		else
		{
			Write("<color=#FFCC00>Usage: /unname <ID> </color>");
		}
	}

	[Command("mark")]
	private void Mark(string[] args)
	{
		if (args.Length > 1)
		{
			if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
			{
				FengGameManagerMKII.instance.markList.Add(value.ID);
				FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
				Write("<color=#FFCC00>Used /mark ID </color>" + value.ID);
			}
		}
		else
		{
			Write("<color=#FFCC00>Usage: /mark <ID> </color>");
		}
	}

	[Command("unmark")]
	private void UnMark(string[] args)
	{
		if (args.Length > 1)
		{
			if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
			{
				FengGameManagerMKII.instance.markList.Remove(value.ID);
				FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
				Write("<color=#FFCC00>Used /unmark ID </color>" + value.ID);
			}
		}
		else
		{
			Write("<color=#FFCC00>Usage: /unmark <ID> </color>");
		}
	}

	[Command("resetteam")]
	[Command("resetteams")]
	private void ResetTeams(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			Chat("<color=#FFCC00>MasterClient has reset teams count!</color>");
			FengGameManagerMKII.instance.cyanWins = 0;
			FengGameManagerMKII.instance.magentaWins = 0;
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[Command("r")]
	[Command("restart")]
	private void GameRestart(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			Chat("<color=#FFCC00>MasterClient has restarted the game!</color>");
			FengGameManagerMKII.instance.restartRC();
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[Command("menu")]
	private void MenuOn(string[] args)
	{
		if (!FengCustomInputs.Inputs.menuOn)
		{
			FengCustomInputs.Inputs.menuOn = true;
		}
		else if (FengCustomInputs.Inputs.menuOn)
		{
			FengCustomInputs.Inputs.menuOn = false;
		}
	}

	[Command("aso")]
	private void ASORules(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			if (args.Length != 2)
			{
				Write("<color=#FFCC00>Usage: /aso <kdr|racing> </color>");
				return;
			}
			string text = args[1];
			if (!(text == "kdr"))
			{
				if (text == "racing")
				{
					if (GameSettings.racingStatic == 0)
					{
						GameSettings.racingStatic = 1;
						Write("<color=#FFCC00>Racing will not end on finish.</color>");
					}
					else
					{
						GameSettings.racingStatic = 0;
						Write("<color=#FFCC00>Racing will end on finish.</color>");
					}
				}
				else
				{
					Write("<color=#FFCC00>Usage: /aso <kdr|racing> </color>");
				}
			}
			else if (GameSettings.asoPreservekdr == 0)
			{
				GameSettings.asoPreservekdr = 1;
				Write("<color=#FFCC00>KDRs will be preserved from disconnects.</color>");
			}
			else
			{
				GameSettings.asoPreservekdr = 0;
				Write("<color=#FFCC00>KDRs will not be preserved from disconnects.</color>");
			}
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[Command("pause")]
	private void GamePause(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			Chat("<color=#FFCC00>MasterClient has paused the game.</color>");
			FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, true);
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[Command("unpause")]
	private void GameUnPause(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			Chat("<color=#FFCC00>MasterClient has unpaused the game.</color>");
			FengGameManagerMKII.instance.photonView.RPC("pauseRPC", PhotonTargets.All, false);
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[Command("clear")]
	private void ChatClear(string[] args)
	{
		messages.Clear();
		FleurConsole.messages.Clear();
		Write("Cleared");
	}

	[Command("ignorelist")]
	private void IgnoreList(string[] args)
	{
		string text = string.Empty;
		foreach (int ignore in FengGameManagerMKII.ignoreList)
		{
			text += $"{ignore}{Environment.NewLine}";
		}
		Write(text);
	}

	[Command("mute")]
	private void PlayerMute(string[] args)
	{
		if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
		{
			FengGameManagerMKII.instance.muteList.Add(value.ID);
			FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
		}
	}

	[Command("unmute")]
	private void PlayerUnMute(string[] args)
	{
		if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
		{
			FengGameManagerMKII.instance.muteList.Remove(value.ID);
			FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
		}
	}

	[Command("room")]
	private void Room(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			string text = args[1];
			if (!(text == "max"))
			{
				if (text == "time")
				{
					float num = Convert.ToSingle(args[2]);
					FengGameManagerMKII.instance.addTime(num);
					Chat($"<color=#FFCC00> {num} seconds added to the clock.</color>");
				}
				else
				{
					Write("<color=#FFCC00>Usage: /room <max|time> <number> </color>");
				}
			}
			else
			{
				int num2 = Convert.ToInt32(args[2]);
				FengGameManagerMKII.instance.maxPlayers = num2;
				PhotonNetwork.room.maxPlayers = num2;
				Chat($"<color=#FFCC00>Max players changed to {num2}!</color>");
			}
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[Command("ign")]
	private void PlayerIgnore(string[] args)
	{
		if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
		{
			FengGameManagerMKII.ignoreList.Add(value.ID);
			FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
		}
	}

	[Command("uni")]
	private void PlayerUnIgnore(string[] args)
	{
		if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
		{
			if (FengGameManagerMKII.ignoreList.Contains(value.ID))
			{
				FengGameManagerMKII.ignoreList.Remove(value.ID);
			}
			if (FengGameManagerMKII.instance.DCPeopleList.Contains(RCextensions.returnStringFromObject(value.customProperties["name"])))
			{
				FengGameManagerMKII.instance.DCPeopleList.Remove(RCextensions.returnStringFromObject(value.customProperties["name"]));
			}
			if (NetworkingPeer.instantiateCounter.ContainsKey(value))
			{
				NetworkingPeer.instantiateCounter[value].Clear();
			}
			FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
		}
	}

	[Command("resetkd")]
	private void ResetKD(string[] args)
	{
		PhotonNetwork.player.SetCustomProperties(new Hashtable
		{
			{ "kills", 0 },
			{ "deaths", 0 },
			{ "max_dmg", 0 },
			{ "total_dmg", 0 }
		});
		Write("<color=#FFCC00>Your stats have been reset. </color>");
	}

	[Command("resetkdall")]
	private void ResetKDAll(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			Hashtable customProperties = new Hashtable
			{
				{ "kills", 0 },
				{ "deaths", 0 },
				{ "max_dmg", 0 },
				{ "total_dmg", 0 }
			};
			PhotonPlayer[] playerList = PhotonNetwork.playerList;
			for (int i = 0; i < playerList.Length; i++)
			{
				playerList[i].SetCustomProperties(customProperties);
			}
			Chat("<color=#FFCC00>All stats have been reset.</color>");
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[Command("pm")]
	private void PlayerPrivateMessage(string[] args)
	{
		if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
		{
			string text = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]).hexColor();
			if (text == string.Empty)
			{
				text = RCextensions.returnStringFromObject(PhotonNetwork.player.customProperties["name"]);
				if (PhotonNetwork.player.customProperties["RCteam"] != null)
				{
					if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]) == 1)
					{
						text = "<color=#00FFFF>" + text + "</color>";
					}
					else if (RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]) == 2)
					{
						text = "<color=#FA8072>" + text + "</color>";
					}
				}
			}
			string text2 = RCextensions.returnStringFromObject(value.customProperties["name"]).hexColor();
			if (text2 == string.Empty)
			{
				text2 = RCextensions.returnStringFromObject(value.customProperties["name"]);
				if (value.customProperties["RCteam"] != null)
				{
					if (RCextensions.returnIntFromObject(value.customProperties["RCteam"]) == 1)
					{
						text2 = "<color=#00FFFF>" + text2 + "</color>";
					}
					else if (RCextensions.returnIntFromObject(value.customProperties["RCteam"]) == 2)
					{
						text2 = "<color=#FA8072>" + text2 + "</color>";
					}
				}
			}
			string text3 = string.Join(" ", args, 2, args.Length - 2);
			if ((int)FengGameManagerMKII.settings[324] == 0)
			{
				FengGameManagerMKII.instance.photonView.RPC("ChatPM", value, text, text3);
				Write("<color=#FFC000>TO [" + value.ID + "]</color> " + text2 + ":" + text3);
			}
			else
			{
				TranslateOutgoingMessages(text3, PM: true, value.ID, text2);
			}
		}
		else
		{
			Write("<color=#FFCC00>No player with id " + args[1] + "</color>");
		}
	}

	[Command("specmode")]
	private void SpectateMode(string[] args)
	{
		if ((int)FengGameManagerMKII.settings[245] == 0)
		{
			FengGameManagerMKII.settings[245] = 1;
			FengGameManagerMKII.instance.EnterSpecMode(enter: true);
			Write("<color=#FFCC00>You have entered spectator mode.</color>");
		}
		else
		{
			FengGameManagerMKII.settings[245] = 0;
			FengGameManagerMKII.instance.EnterSpecMode(enter: false);
			Write("<color=#FFCC00>You have exited spectator mode.</color>");
		}
	}

	[Command("spec")]
	[Command("spectate")]
	private void PlayerSpectate(string[] args)
	{
		int num = Convert.ToInt32(args[1]);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject gameObject in array)
		{
			if (gameObject.GetPhotonView().owner.ID == num)
			{
				IN_GAME_MAIN_CAMERA.mainCamera.setMainObject(gameObject);
				IN_GAME_MAIN_CAMERA.mainCamera.setSpectorMode(val: false);
			}
		}
	}

	[Command("rev")]
	[Command("revive")]
	private void PlayerRevive(string[] args)
	{
		if (!PhotonNetwork.isMasterClient)
		{
			return;
		}
		int num = Convert.ToInt32(args[1]);
		if (NetworkingPeer.mActors.TryGetValue(num, out var value))
		{
			Write($"<color=#FFCC00>Player {num} has been revived.</color>");
			if (value.customProperties["dead"] != null && RCextensions.returnBoolFromObject(value.customProperties["dead"]) && RCextensions.returnIntFromObject(value.customProperties["isTitan"]) != 2)
			{
				FengGameManagerMKII.instance.photonView.RPC("Chat", value, "<color=#FFCC00>You have been revived by the master client.</color>", string.Empty);
				FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", value);
			}
		}
	}

	[Command("revall")]
	[Command("reviveall")]
	private void PlayerReviveAll(string[] args)
	{
		if (!PhotonNetwork.isMasterClient)
		{
			return;
		}
		Chat("<color=#FFCC00>All players have been revived.</color>");
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in playerList)
		{
			if (photonPlayer.customProperties["dead"] != null && RCextensions.returnBoolFromObject(photonPlayer.customProperties["dead"]) && RCextensions.returnIntFromObject(photonPlayer.customProperties["isTitan"]) != 2)
			{
				FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", photonPlayer);
			}
		}
	}

	[Command("kick")]
	private void PlayerKick(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			if (args.Length <= 1)
			{
				return;
			}
			for (int i = 1; i < args.Length; i++)
			{
				int num = Convert.ToInt32(args[i]);
				PhotonPlayer value;
				if (num == PhotonNetwork.player.ID)
				{
					Write("error: can't kick yourself.");
				}
				else if (NetworkingPeer.mActors.TryGetValue(num, out value))
				{
					FengGameManagerMKII.instance.kickPlayerRC(value, ban: false, "");
					Chat("<color=#FFCC00>" + RCextensions.returnStringFromObject(value.customProperties["name"]) + " has been kicked from the server!</color>");
				}
				else
				{
					Write("error: no such player.");
				}
			}
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[Command("ban")]
	private void PlayerBan(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			if (args.Length <= 1)
			{
				return;
			}
			for (int i = 1; i < args.Length; i++)
			{
				int num = Convert.ToInt32(args[i]);
				PhotonPlayer value;
				if (num == PhotonNetwork.player.ID)
				{
					Write("error: can't ban yourself.");
				}
				else if (NetworkingPeer.mActors.TryGetValue(num, out value))
				{
					FengGameManagerMKII.instance.kickPlayerRC(value, ban: true, "");
					Chat("<color=#FFCC00>" + RCextensions.returnStringFromObject(value.customProperties["name"]) + " has been banned from the server!</color>");
				}
				else
				{
					Write("error: no such player.");
				}
			}
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[Command("banlist")]
	private void PlayerBanlist(string[] args)
	{
		Write("<color=#FFCC00>List of banned players:</color>");
		string text = string.Empty;
		foreach (int key in FengGameManagerMKII.banHash.Keys)
		{
			text += $"<color=#FFCC00>{key} : {FengGameManagerMKII.banHash[key]} </color>{Environment.NewLine}";
		}
		Write(text);
	}

	[Command("unban")]
	private void PlayerUnBan(string[] args)
	{
		int num = Convert.ToInt32(args[1]);
		if (FengGameManagerMKII.banHash.ContainsKey(num))
		{
			string text = (string)FengGameManagerMKII.banHash[num];
			if (FengGameManagerMKII.instance.DCPeopleList.Contains(text))
			{
				FengGameManagerMKII.instance.DCPeopleList.Remove(text);
			}
			FengGameManagerMKII.banHash.Remove(num);
			FengGameManagerMKII.banHash.Remove(text);
			if (NetworkingPeer.mActors.TryGetValue(num, out var value))
			{
				FengGameManagerMKII.ignoreList.Remove(value.ID);
			}
			FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
			if (PhotonNetwork.isMasterClient)
			{
				Chat("<color=#FFCC00>" + text + " has been unbanned from the server. </color>");
			}
			else
			{
				Write("<color=#FFCC00>" + text + " has been unbanned from the local banlist. </color>");
			}
		}
		else
		{
			Write("error: no such player");
		}
	}

	[Command("team")]
	private void ChangeTeam(string[] args)
	{
		if (GameSettings.teamMode == 1)
		{
			switch (args[1])
			{
			case "0":
			case "individual":
			{
				FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 0);
				Write("<color=#00FFFF>You have joined individuals.</color>");
				GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
				foreach (GameObject gameObject2 in array)
				{
					if (gameObject2.GetPhotonView().isMine)
					{
						HERO component2 = gameObject2.GetComponent<HERO>();
						component2.markDie();
						component2.photonView.RPC("netDie2", PhotonTargets.All, -1, "Team Switch");
					}
				}
				break;
			}
			case "1":
			case "cyan":
			{
				FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 1);
				Write("You have joined team 1.");
				GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
				foreach (GameObject gameObject3 in array)
				{
					if (gameObject3.GetPhotonView().isMine)
					{
						HERO component3 = gameObject3.GetComponent<HERO>();
						component3.markDie();
						component3.photonView.RPC("netDie2", PhotonTargets.All, -1, "Team Switch");
					}
				}
				break;
			}
			case "2":
			case "magenta":
			{
				FengGameManagerMKII.instance.photonView.RPC("setTeamRPC", PhotonNetwork.player, 2);
				Write("You have joined team 2.");
				GameObject[] array = GameObject.FindGameObjectsWithTag("Player");
				foreach (GameObject gameObject in array)
				{
					if (gameObject.GetPhotonView().isMine)
					{
						HERO component = gameObject.GetComponent<HERO>();
						component.markDie();
						component.photonView.RPC("netDie2", PhotonTargets.All, -1, "Team Switch");
					}
				}
				break;
			}
			default:
				Write("<color=#FFCC00>Usage: /team <0|1|2> OR /team <cyan|magenta|individual>.</color>");
				break;
			}
		}
		else
		{
			Write("<color=#FFCC00>error: teams are locked or disabled. </color>");
		}
	}

	[Command("j")]
	private void Rejoin(string[] args)
	{
		PhotonNetwork.networkingPeer.OnStatusChanged(StatusCode.DisconnectByServerLogic);
	}

	[Command("rules")]
	private void Rules(string[] args)
	{
		Write("<color=#FFCC00>Currently activated gamemodes:</color>");
		if (GameSettings.bombMode > 0)
		{
			Write("<color=#FFCC00>Bomb mode is on.</color>");
		}
		if (GameSettings.teamMode > 0 && GameSettings.teamMode == 1)
		{
			Write("<color=#FFCC00>Team mode is on (no sort).</color>");
		}
		if (GameSettings.pointMode > 0)
		{
			Write("<color=#FFCC00>Point mode is on (" + Convert.ToString(GameSettings.pointMode) + ").</color>");
		}
		if (GameSettings.disableRock > 0)
		{
			Write("<color=#FFCC00>Punk Rock-Throwing is disabled.</color>");
		}
		if (GameSettings.spawnMode > 0)
		{
			Write("<color=#FFCC00>Custom spawn rate is on (" + GameSettings.nRate.ToString("F2") + "% Normal, " + GameSettings.aRate.ToString("F2") + "% Abnormal, " + GameSettings.jRate.ToString("F2") + "% Jumper, " + GameSettings.cRate.ToString("F2") + "% Crawler, " + GameSettings.pRate.ToString("F2") + "% Punk </color>");
		}
		if (GameSettings.explodeMode > 0)
		{
			Write("<color=#FFCC00>Titan explode mode is on (" + Convert.ToString(GameSettings.explodeMode) + ").</color>");
		}
		if (GameSettings.healthMode > 0)
		{
			Write("<color=#FFCC00>Titan health mode is on (" + Convert.ToString(GameSettings.healthLower) + "-" + Convert.ToString(GameSettings.healthUpper) + ").</color>");
		}
		if (GameSettings.damageMode > 0)
		{
			Write("<color=#FFCC00>Minimum nape damage is on (" + Convert.ToString(GameSettings.damageMode) + ").</color>");
		}
		if (GameSettings.moreTitans > 0)
		{
			Write("<color=#FFCC00>Custom titan is on (" + Convert.ToString(GameSettings.moreTitans) + ").</color>");
		}
		if (GameSettings.sizeMode > 0)
		{
			Write("<color=#FFCC00>Custom titan size is on (" + GameSettings.sizeLower.ToString("F2") + "," + GameSettings.sizeUpper.ToString("F2") + ").</color>");
		}
		if (GameSettings.banEren > 0)
		{
			Write("<color=#FFCC00>Anti-Eren is on. Using Titan eren will get you kicked.</color>");
		}
		if (GameSettings.waveModeOn == 1)
		{
			Write("<color=#FFCC00>Custom wave mode is on (" + Convert.ToString(GameSettings.waveModeNum) + ").</color>");
		}
		if (GameSettings.friendlyMode > 0)
		{
			Write("<color=#FFCC00>Friendly-Fire disabled. PVP is prohibited.</color>");
		}
		if (GameSettings.pvpMode > 0)
		{
			if (GameSettings.pvpMode == 1)
			{
				Write("<color=#FFCC00>AHSS/Blade PVP is on (team-based).</color>");
			}
			else if (GameSettings.pvpMode == 2)
			{
				Write("<color=#FFCC00>AHSS/Blade PVP is on (FFA).</color>");
			}
		}
		if (GameSettings.maxWave > 0)
		{
			Write("<color=#FFCC00>Max Wave set to " + GameSettings.maxWave + "</color>");
		}
		if (GameSettings.horseMode > 0)
		{
			Write("<color=#FFCC00>Horses are enabled.</color>");
		}
		if (GameSettings.ahssReload > 0)
		{
			Write("<color=#FFCC00>AHSS Air-Reload disabled.</color>");
		}
		if (GameSettings.punkWaves > 0)
		{
			Write("<color=#FFCC00>Punk override every 5 waves enabled.</color>");
		}
		if (GameSettings.endlessMode > 0)
		{
			Write("<color=#FFCC00>Endless Respawn is enabled (" + GameSettings.endlessMode + " seconds).</color>");
		}
		if (GameSettings.globalDisableMinimap > 0)
		{
			Write("<color=#FFCC00>Minimaps are disabled.</color>");
		}
		if (GameSettings.motd != string.Empty)
		{
			Write("<color=#FFCC00>MOTD:" + GameSettings.motd + "</color>");
		}
		if (GameSettings.deadlyCannons > 0)
		{
			Write("<color=#FFCC00>Cannons will kill humans.</color>");
		}
		if (GameSettings.DrunkTitans > 0)
		{
			Write("<color=#FFCC00>Drunk Titans enabled.</color>");
		}
	}
}
