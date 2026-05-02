using BRM;
using ExitGames.Client.Photon;
using Microsoft.SqlServer.Server;
using Photon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
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

	public static InRoomChat Instance;

	public static Rect GuiRect;

	public static Rect GuiRect2;

	private string inputLine = string.Empty;

	public static List<string> messages;

	private Vector2 scrollPos = Vector2.zero;

	private string key = string.Empty;

	private static GUIStyle currentStyle;

	public static InRoomChat ChatInstanse;

	private static readonly Dictionary<string, MethodInfo> MyCommandsCache;

	private static readonly Dictionary<string, MethodInfo> BotCommandsCache;

	private static bool botOn;

	public static bool god;

	public static bool kdEnabled;

	private static UrbanDictionary ud;

    public bool IsVisible = true;

	public Stopwatch timeOfFade = new Stopwatch();

	private static bool MessageHistoryUpdatecache;

	private static string MessageHistoryCache;

	private static Dictionary<string, string> CommandList;

	//private AudioSource Typing;

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
		MyCommandsCache = new Dictionary<string, MethodInfo>();
		BotCommandsCache = new Dictionary<string, MethodInfo>();
		botOn = true;
		kdEnabled = true;
		god = true;
		ud = new UrbanDictionary();
        MessageHistoryUpdatecache = true;
		MessageHistoryCache = string.Empty;
		CommandList = new Dictionary<string, string>
		{
			// kill id
			// spellcheck
			// TODO infinite hooks & boost
			// hook skin
			// change stats / unlimited stats
			// auto msgs
			// horse speed yeeeeeee
			// hook and burst skin
			// constant chat log
			// steal skins
			{ "<color=red>/kickall #</color>","" },
			{ "<color=red>/penis #</color>","" },
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

        Type botCommandAttrType = typeof(BotCommandAttribute);
        Type myCommandAttrType = typeof(MyCommandAttribute);

        foreach (MethodInfo methodInfo in methods)
        {
            // Check for BotCommandAttribute
            var botCommandAttrs = methodInfo.GetCustomAttributes(botCommandAttrType, inherit: false);
            foreach (BotCommandAttribute attr in botCommandAttrs)
            {
                string cmdName = attr.Name ?? methodInfo.Name.ToLower();
                BotCommandsCache[cmdName.ToLower()] = methodInfo;
            }

            // Check for MyCommandAttribute
            var myCommandAttrs = methodInfo.GetCustomAttributes(myCommandAttrType, inherit: false);
            foreach (MyCommandAttribute attr in myCommandAttrs)
            {
                string cmdName = attr.Name ?? methodInfo.Name.ToLower();
                MyCommandsCache[cmdName.ToLower()] = methodInfo;
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

	public static void BotCommandInvoke(string txt)
	{
        string[] array = txt.Split(' ');
        int[] indices = array
            .Select((s, i) => new { s, i })
            .Where(x => x.s.StartsWith("-"))
            .Select(x => x.i)
            .ToArray();

        foreach (int i in indices)
        {
            if (array[i] != "-")
            {
                if (BotCommandsCache.TryGetValue(array[i].Substring(1), out var value))
                {
                    string[] array2 = array.Skip(i).ToArray();
                    array2[0] = array2[0].Substring(1);

                    if (value.IsStatic)
                    {
                        value.Invoke(null, new object[1] { array2 });
                    }
                    else
                    {
                        value.Invoke(Instance, new object[1] { array2 });
                    }
                    break;
                }
            }
        }
    }

	public static int Write(string newLine)
	{
		if (messages.Count > 150)
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
			if (Event.current.keyCode != 0 && Event.current.keyCode == FengGameManagerMKII.inputRC.humanKeys[InputCodeRC.chat] && GUI.GetNameOfFocusedControl() != "ChatInput")
			{
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
					if (GUI.GetNameOfFocusedControl() == "ChatInput")
					{
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
                            Instance = this;
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
                            if (MyCommandsCache.TryGetValue(array[0], out var value))
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
                }
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
		//Typing = Camera.main.gameObject.AddComponent<AudioSource>();
		//Typing.clip = (AudioClip)FengGameManagerMKII.someBundle.Load("type");
		//Typing.volume = FengGameManagerMKII.InterfaceVolume;
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
			//if (!Typing.isPlaying && GameSettings.Typing > 0)
			//{
			//	Typing.Play();
			//}
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

    [MyCommand("on")]
    [MyCommand("ON")]
    private void On(string[] args)
    {
		botOn = true;
        Write("<color=#FFCC00>Internal Bot is ERECT (Turned ON)</color>");
    }

    [MyCommand("off")]
    [MyCommand("OFF")]
    private void Off(string[] args)
    {
        botOn = false;
        Write("<color=#FFCC00>Internal Bot is FLACID (Turned OFF)</color>");
    }

	[BotCommand("pls")]
	private void PlsPenis(string[] args)
	{
		if (botOn)
		{
			if (args.Length > 1)
			{
				if (args[1] == "penis")
				{
					string name = "";

					if (args.Length > 2)
					{
						if (int.TryParse(args[2], out int n))
						{
							if (NetworkingPeer.mActors.TryGetValue(n, out PhotonPlayer value))
							{
								name = RCextensions.StripHex(value.uiname);
							}
						}
					}
					
					if (name == "")
                        name = string.Join(" ", args.Skip(2).ToArray());

                    int size = UnityEngine.Random.Range(0, 33); // Adjust the range as needed
                    string text = ": ";
                    if (name != null && name.Trim() != "") text += $"<color=#2CB255>{name}'s dick size is</color>\n";

                    if (size == 0)
                        text += "<color=#FFDA94>() you don't have a dick you have pussy</color>";
                    else if (size < 10 && size != 0)
                        text += "<color=#FFDA94>8" + new string('=', size) + "</color>" + "<color=#710071>D</color> " + size +
                                " cm micro penis";
                    else
                        text += "<color=#FFDA94>8" + new string('=', size) + "</color>" + "<color=#710071>D</color> " + size +
                                " cm";

                    Chat(text);
                }
			}
		}
    }

    [BotCommand("istrue")]
    private void IsTrue(string[] args)
    {
		if (botOn)
		{
			int i = UnityEngine.Random.Range(0, 2);
			string response;
			if (i == 1)
				response = ": Its true!";
			else
				response = ": Its not True";

			Chat(response);
		}
    }

    [BotCommand("roll")]
    private void Roll(string[] args)
    {
		if (args.Length > 1)
		{
			if (int.TryParse(args[1], out int num))
			{
				Chat(": You Rolled: " + UnityEngine.Random.Range(1, num + 1));
			}
		}
    }

    [BotCommand("coin")]
    private void Coin(string[] args)
    {
        Chat(": " + (UnityEngine.Random.Range(0, 2) == 1 ? "Heads" : "Tails"));
    }

    [BotCommand("rev")]
	[BotCommand("revive")]
	private void SelfRev(string[] args)
	{
		if (GameSettings.bombMode == 1) return;
		int.TryParse(args[args.Length - 1], out int num);
		if (NetworkingPeer.mActors.TryGetValue(num, out var value))
		{
			if (value.customProperties["dead"] != null && RCextensions.returnBoolFromObject(value.customProperties["dead"]) && RCextensions.returnIntFromObject(value.customProperties["isTitan"]) != 2)
			{
				FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", value);
			}
		}
	}

	[MyCommand("clone")]
    private void Clone(string[] args)
    {
        if (PlayerPrefs.HasKey("characterUsed"))
		{
			for (int i = 0; i < ((args.Length > 1 && int.TryParse(args[1], out var n)) ? n : 1) ; i++)
			{
				CharacterSelection.ChoosePlayer(PlayerPrefs.GetString("characterUsed"));
            }
		}
    }

    [BotCommand("ud")]
    private void Ud(string[] args)
	{
		if (botOn)
		{
			if (args.Length > 1)
			{
				string term = string.Join(" ", args.Skip(1).ToArray());
				try
				{
					new Thread(() =>
					{
						string response = ud.DefineTerm(term);
						if (response != "")
						{
							Chat(": " + response);
						}
					}).Start();
				}
				catch (Exception ex)
				{
					Write("InRoomChat UD Error: " + ex.Message);
				}
			}
		}
    }

    [BotCommand("next")]
    private void Next(string[] args)
    {
        string response = ud.Next();
        if (response != "")
        {
            Chat(": " + response);
        }
    }

    [BotCommand("back")]
    private void Back(string[] args)
    {
        string response = ud.Back();
        if (response != "")
        {
            Chat(": " + response);
        }
    }

    [BotCommand("hey")]
    [BotCommand("yo")]
    [BotCommand("hi")]
    private void Ai(string[] args)
    {
        if (args.Length > 1)
        {
            if (args[1] == "slave" || args[1] == "slave,")
            {
                string term = string.Join(" ", args.Skip(2).ToArray());
                try
                {
                    new Thread(() =>
                    {
                        string response = AI.AskAI(term.Replace(" ", "") == "" ? args[0].Replace("-", "") : term);
                        if (response != "")
                        {
                            Chat(" " + response, "<color=#000000>Slave</color>");
                        }
                    }).Start();
                }
                catch (Exception ex)
                {
                    Write("InRoomChat AI Error: " + ex.Message);
                }
            }
        }
    }

    [BotCommand("avg")]
    [BotCommand("average")]
    private void Avg(string[] args)
    {
		if (botOn || args[0] == "avg")
		{
			if (args.Length > 1)
			{
				int id;


                if (int.TryParse(args[1], out id)) { }
				else
				{
					string name = string.Join(" ", args.Skip(1).Take(args.Length - 2).ToArray());

                    foreach (KeyValuePair<int, PhotonPlayer> value in NetworkingPeer.mActors)
                    {
                        if (RCextensions.StripHex(value.Value.uiname).ToLower() == name.ToLower())
                        {
                            id = value.Key;
							break;
                        }
                    }
                    if (id == 0 ? int.TryParse(args[args.Length - 1], out id) : true) { }
					else return;
                }

                if (NetworkingPeer.mActors.TryGetValue(id, out PhotonPlayer player)) { }
				else return;

				int avg;
				player.customProperties.TryGetValue("total_dmg", out var n);
				avg = (int)n;
				player.customProperties.TryGetValue("kills", out n);
				if ((int)n == 0)
					n = 1;
				
				avg = avg / (int)n;

				Chat(": " + RCextensions.hexColor(player.uiname) + "'s Average DMG: " + avg.ToString());
				return;
			}
		}
    }

    [BotCommand("kd")]
    private void Kd(string[] args)
    {
        if (args.Length > 1)
        {
            int id;


            if (int.TryParse(args[1], out id)) { }
            else
            {
                string name = string.Join(" ", args.Skip(1).Take(args.Length - 2).ToArray());

                foreach (KeyValuePair<int, PhotonPlayer> value in NetworkingPeer.mActors)
                {
                    if (RCextensions.StripHex(value.Value.uiname).ToLower() == name.ToLower())
                    {
                        id = value.Key;
                        break;
                    }
                }
                if (id == 0 ? int.TryParse(args[args.Length - 1], out id) : true) { }
                else return;
            }

            if (NetworkingPeer.mActors.TryGetValue(id, out PhotonPlayer player)) { }
            else return;

            float kd;
            player.customProperties.TryGetValue("kills", out var n);
            kd = (int)n;
			player.customProperties.TryGetValue("deaths", out n);
			if ((int)n == 0)
				n = 1;

			kd = kd / (int)n;

            Chat(": " + RCextensions.hexColor(player.uiname) + "'s KD: " + kd.ToString("0.##"));
            return;
        }
    }

    [BotCommand("shrug")]
    private void BotShrug(string[] args)
    {
        Chat(": ¯\\_(ツ)_/¯");
    }

    [MyCommand("am")]
    private void Am(string[] args)
    {
        if (args.Length > 1)
		{
			if (args[1] == "to")
			{
				if (args.Length > 3)
				{
					string[] names = args[2].Split(',');
                    string msg = string.Join(" ", args.Skip(3).ToArray());
                    foreach (string name in names)
					{
						if (FengGameManagerMKII.amMsgs.ContainsKey(name.ToLower()))
						{
							FengGameManagerMKII.amMsgs[name.ToLower()].Add(msg);
						}
						else
						{
                            FengGameManagerMKII.amMsgs.Add(name.ToLower(), new List<string> { msg });
                        }
                    }
					FengGameManagerMKII.am = true;
                    Write("<color=#FFCC00>Auto msg set to: " + msg + "</color>");
                }
			}
			else if (args.Length == 2 &&  args[1] == "clear")
			{
				FengGameManagerMKII.am = false;
				FengGameManagerMKII.amMsgs.Clear();
				FengGameManagerMKII.amBlackList.Clear();
                Write("<color=#FFCC00>am cleared</color>");
            }
			else
			{
                string msg = string.Join(" ", args.Skip(1).ToArray());
                if (FengGameManagerMKII.amMsgs.ContainsKey("all"))
                {
                    FengGameManagerMKII.amMsgs["all"].Add(msg);
                }
                else
                {
                    FengGameManagerMKII.amMsgs.Add("all", new List<string> { msg });
                }
                FengGameManagerMKII.am = true;
                Write("<color=#FFCC00>Auto msg set to: " + msg + "</color>");
            }
		}
    }

    [MyCommand("kd")]
    private void KdToggle(string[] args)
    {
        if (args.Length == 2)
		{
			if (args[1] == "off")
			{
				//ResetKD(args);
				kdEnabled = false;
                Write("<color=#FFCC00>KD Disabled</color>");
            }
			else if (args[1] == "on")
            {
                kdEnabled = true;
                Write("<color=#FFCC00>KD Enabled</color>");
            }
			FengGameManagerMKII.instance.updatePlayerList();
        }
    }

    [MyCommand("god")]
    private void GodToggle(string[] args)
    {
		god = !god;
		if (god)
		{
            Write("<color=#FFCC00>GOD Enabled</color>");
        }
		else
		{
            Write("<color=#FFCC00>GOD Disabled</color>");
        }
    }
    IEnumerator WaitAndQuit()
    {
        yield return new WaitForSeconds(0.5f);

        FengGameManagerMKII.alltitans.Clear();
        if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
        {
            Time.timeScale = 1f;
        }
        else
        {
            PhotonNetwork.Disconnect();
        }
        Screen.lockCursor = false;
        Screen.showCursor = true;
        IN_GAME_MAIN_CAMERA.gametype = GAMETYPE.STOP;
        FengGameManagerMKII.mapScript = string.Empty;
        FengGameManagerMKII.script_text_field = string.Empty;
        FengGameManagerMKII.customMapScripts = false;
        FengGameManagerMKII.is_skin_gui = false;
        FengGameManagerMKII.gameStart = false;
        CacheGameObject.Find<FengCustomInputs>("InputManagerController").menuOn = false;
        FengGameManagerMKII.instance.DestroyAllExistingCloths();
        UnityEngine.Object.Destroy(CacheGameObject.Find("MultiplayerManager"));
        Application.LoadLevel("menu");
    }

    [MyCommand("quit")]
    private void Quit(string[] args)
    {
		KickAll(new string[] { "kickall" });
        StartCoroutine(WaitAndQuit());
    }

    [MyCommand("shrug")]
    private void MyShrug(string[] args)
    {
		Chat(" <color=#FFFFFF>¯\\_(ツ)_/¯</color>", RCextensions.hexColor(PhotonNetwork.player.uiname));
    }

    [MyCommand("close")]
	private void Close(string[] args)
	{
		PhotonNetwork.room.open = false;
		Chat("<color=#FFCC00>Room Closed</color>");
	}

    [MyCommand("open")]
    private void Open(string[] args)
    {
        PhotonNetwork.room.open = true;
        Chat("<color=#FFCC00>Room Opened</color>");
    }

    [MyCommand("hide")]
    private void Hide(string[] args)
    {
		if (args.Length > 1)
		{
			Stay(args);
        }

		Close(args);
        PhotonNetwork.room.visible = false;
        Chat("<color=#FFCC00>Room Hidden</color>");
    }

    [MyCommand("show")]
    private void Unhide(string[] args)
    {
        PhotonNetwork.room.visible = true;
        Chat("<color=#FFCC00>Room Shown</color>");
    }

    [MyCommand("forest")]
    private void Forest(string[] args)
    {
		FengGameManagerMKII.level = "The Forest III";
        FengGameManagerMKII.levelinfo = LevelInfo.getInfo(FengGameManagerMKII.level);
        IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(FengGameManagerMKII.level).type;
        FengGameManagerMKII.PView.RPC("RPCLoadLevel", PhotonTargets.All);
    }

    [MyCommand("akina")]
    private void Akina(string[] args)
    {
		FengGameManagerMKII.level = "Racing - Akina";
        FengGameManagerMKII.levelinfo = LevelInfo.getInfo(FengGameManagerMKII.level);
        IN_GAME_MAIN_CAMERA.gamemode = LevelInfo.getInfo(FengGameManagerMKII.level).type;
        FengGameManagerMKII.PView.RPC("RPCLoadLevel", PhotonTargets.All);
    }

    [MyCommand("stop")]
    private void Stop(string[] args)
    {
        StopAllCoroutines();
        Write("<color=#FFCC00>Stopped</color>");
    }

    [MyCommand("move")]
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

	[MyCommand("white")]
	[MyCommand("name")]
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

	[MyCommand("Unname")]
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

	[MyCommand("mark")]
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

	[MyCommand("unmark")]
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

	[MyCommand("resetteam")]
	[MyCommand("resetteams")]
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

	[MyCommand("r")]
	[MyCommand("restart")]
	private void GameRestart(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			//Chat("<color=#FFCC00>MasterClient has restarted the game!</color>");
			FengGameManagerMKII.instance.restartRC();
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[MyCommand("menu")]
	private void MenuOn(string[] args)
	{
		FengCustomInputs.Inputs.menuOn = !FengCustomInputs.Inputs.menuOn;
    }

	[MyCommand("aso")]
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

	[MyCommand("pause")]
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

	[MyCommand("unpause")]
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

	[MyCommand("clear")]
	private void ChatClear(string[] args)
	{
		messages.Clear();
		FleurConsole.messages.Clear();
		Write("");
	}

	[MyCommand("clearl")]
	[MyCommand("clearlog")]
	private void logClear(string[] args)
	{
		FleurConsole.messages.Clear();
	}

	[MyCommand("ignorelist")]
	private void IgnoreList(string[] args)
	{
		string text = string.Empty;
		foreach (int ignore in FengGameManagerMKII.ignoreList)
		{
			text += $"{ignore}{Environment.NewLine}";
		}
		Write(text);
	}

	[MyCommand("mute")]
	private void PlayerMute(string[] args)
	{
		if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
		{
			FengGameManagerMKII.instance.muteList.Add(value.ID);
			FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
		}
	}

	[MyCommand("unmute")]
	private void PlayerUnMute(string[] args)
	{
		if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
		{
			FengGameManagerMKII.instance.muteList.Remove(value.ID);
			FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
		}
	}

	[MyCommand("room")]
	private void Room(string[] args)
	{
		if (args.Length > 2)
		{
			if (PhotonNetwork.isMasterClient || true)
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
	}

	[MyCommand("ign")]
	[MyCommand("ignore")]
	private void PlayerIgnore(string[] args)
	{
		if (NetworkingPeer.mActors.TryGetValue(Convert.ToInt32(args[1]), out var value))
		{
			FengGameManagerMKII.ignoreList.Add(value.ID);
			FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
		}
	}

	[MyCommand("uni")]
	[MyCommand("unignore")]
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
			FengGameManagerMKII.instance.RecompilePlayerList(0.1f);
		}
	}

	[MyCommand("resetkd")]
	private void ResetKD(string[] args)
	{
		PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
		{
			{ "kills", 0 },
			{ "deaths", 0 },
			{ "max_dmg", 0 },
			{ "total_dmg", 0 }
		});
		Write("<color=#FFCC00>Your stats have been reset. </color>");
	}

	[MyCommand("resetkdall")]
	private void ResetKDAll(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable
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
			//Chat("<color=#FFCC00>All stats have been reset.</color>");
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}

	[MyCommand("pm")]
	private void PlayerPrivateMessage(string[] args)
	{
        int id;


        if (int.TryParse(args[1], out id)) { }
        else
        {
            foreach (KeyValuePair<int, PhotonPlayer> val in NetworkingPeer.mActors)
            {
                if (RCextensions.StripHex(val.Value.uiname).ToLower() == args[1].ToLower())
                {
                    id = val.Key;
                    break;
                }
            }
        }

        if (NetworkingPeer.mActors.TryGetValue(id, out var value))
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
			string text3 = " <color=#FFFFFF>" + string.Join(" ", args, 2, args.Length - 2) + "</color>";
			if ((int)FengGameManagerMKII.settings[324] == 0)
			{
				FengGameManagerMKII.instance.photonView.RPC("ChatPM", value, text, text3);
                var source = Camera.main.gameObject.AddComponent<AudioSource>();
                source.name = "NotificationSound"; // give it a name for filtering
                source.PlayOneShot(FengGameManagerMKII.ToClick, 1);
                Destroy(source, FengGameManagerMKII.ToClick.length);
                string content = DateTime.Now.ToString("h:mm:ss") + " <color=#FFC000>[" + value.ID + "] TO</color> " + text2 + ":" + text3;
                Write(content);
                File.AppendAllText(Environment.CurrentDirectory + "\\Chats\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", "\n " + content.StripColor().StripHex() + Environment.NewLine);
                if (FengGameManagerMKII.lastCheckedDate == null && File.Exists(Environment.CurrentDirectory + "\\Chats\\.Chats.txt"))
                {
                    FengGameManagerMKII.lastCheckedDate = FengGameManagerMKII.lastChatDateLine();
                    FengGameManagerMKII.lastCheckedDate = FengGameManagerMKII.lastCheckedDate == null ? "1970-01-01" : FengGameManagerMKII.lastCheckedDate.Substring(60, FengGameManagerMKII.lastCheckedDate.Length - 2 * 60);
                }
                if (FengGameManagerMKII.lastCheckedDate != null && FengGameManagerMKII.lastCheckedDate != DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    File.AppendAllText(Environment.CurrentDirectory + "\\Chats\\.Chats.txt", "\n\n----------------------------------------------------------- " + DateTime.Now.ToString("yyyy-MM-dd") + " -----------------------------------------------------------\n\n");
                    FengGameManagerMKII.lastCheckedDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                File.AppendAllText(Environment.CurrentDirectory + "\\Chats\\.Chats.txt", "\n " + content.StripColor().StripHex() + Environment.NewLine);
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
	
	[MyCommand("specmode")]
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

	[MyCommand("spec")]
	[MyCommand("spectate")]
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

	[MyCommand("rev")]
	[MyCommand("revive")]
	private void PlayerRevive(string[] args)
	{
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

	[MyCommand("revall")]
	[MyCommand("reviveall")]
	private void PlayerReviveAll(string[] args)
	{
		//Chat("<color=#FFCC00>All players have been revived.</color>");
		PhotonPlayer[] playerList = PhotonNetwork.playerList;
		foreach (PhotonPlayer photonPlayer in playerList)
		{
			if (photonPlayer.customProperties["dead"] != null && RCextensions.returnBoolFromObject(photonPlayer.customProperties["dead"]) && RCextensions.returnIntFromObject(photonPlayer.customProperties["isTitan"]) != 2)
			{
				FengGameManagerMKII.instance.photonView.RPC("respawnHeroInNewRound", photonPlayer);
			}
		}
	}

	[MyCommand("kick")]
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
				int.TryParse(args[i], out int num);
				PhotonPlayer value;
				if (num == PhotonNetwork.player.ID)
				{
					Write("error: can't kick yourself.");
				}
				else if (NetworkingPeer.mActors.TryGetValue(num, out value))
				{
					FengGameManagerMKII.instance.kickPlayerRC(value, ban: false, "");
					//Chat("<color=#FFCC00>" + RCextensions.returnStringFromObject(value.customProperties["name"]) + " has been kicked from the server!</color>");
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

	[MyCommand("kickall")]
	private void KickAll(string[] args)
	{
		if (PhotonNetwork.isMasterClient)
		{
			List<int> num = new List<int>();

			for (int i = 1; i < args.Length; i++)
			{
				if (int.TryParse(args[i], out int n))
					num.Add(n);
			}

			foreach (KeyValuePair<int, PhotonPlayer> value in NetworkingPeer.mActors)
			{
				if (!num.Contains(value.Key) && value.Key != PhotonNetwork.player.ID)
				{
					FengGameManagerMKII.instance.kickPlayerRC(value.Value, ban: false, "");
					//Chat("<color=#FFCC00>" + RCextensions.returnStringFromObject(value.Value.customProperties["name"]) + " has been kicked from the server!</color>");
				}
			}
		}
		else
		{
			Write("<color=#FFCC00>error: not master client</color>");
		}
	}
	
	[MyCommand("kill")]
	private void Kill(string[] args)
	{
		if (args.Length > 1 &&  int.TryParse(args[1], out int id))
		{
			string msg = "f";
            if (args.Length > 2) msg = string.Join(" ", args.Skip(2).ToArray());
            HERO hero = HERO.FindMyHero(id);
			if (!hero.HasDied())
			{
				hero.photonView.RPC("netDie2", PhotonTargets.All, -1, msg);
			}
		}
		else
		{
            string msg = "f";
            if (args.Length > 1) msg = string.Join(" ", args.Skip(1).ToArray());
            foreach (PhotonPlayer player in PhotonNetwork.playerList)
			{
				HERO hero = HERO.FindMyHero(player.ID);
				if (hero == null) continue;
				if (!hero.HasDied())
				{
					hero.photonView.RPC("netDie2", PhotonTargets.All, -1, msg);
				}
            }
		}
    }

    [MyCommand("stay")]
    private void Stay(string[] args)
    {
		if (args.Length > 1)
		{
			KickAll(args);
			Room(new string[]{"room", "max", args.Length.ToString()});
        }
    }

    [MyCommand("ban")]
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
					//Chat("<color=#FFCC00>" + RCextensions.returnStringFromObject(value.customProperties["name"]) + " has been banned from the server!</color>");
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

    [MyCommand("banall")]
    private void banAll(string[] args)
    {
        if (PhotonNetwork.isMasterClient)
        {
            List<int> num = new List<int>();

            for (int i = 1; i < args.Length; i++)
            {
                num.Add(Convert.ToInt32(args[i]));
            }

            foreach (KeyValuePair<int, PhotonPlayer> value in NetworkingPeer.mActors)
            {
                if (!num.Contains(value.Key) && value.Key != PhotonNetwork.player.ID)
                {
                    FengGameManagerMKII.instance.kickPlayerRC(value.Value, ban: true, "");
                    //Chat("<color=#FFCC00>" + RCextensions.returnStringFromObject(value.Value.customProperties["name"]) + " has been banned from the server!</color>");
                }
            }
        }
        else
        {
            Write("<color=#FFCC00>error: not master client</color>");
        }
    }

    [MyCommand("banlist")]
	private void PlayerBanlist(string[] args)
	{
		Write("<color=#FFCC00>List of banned players:</color>");
		string text = string.Empty;
		foreach (int key2 in FengGameManagerMKII.banHash.Keys)
		{
			text += $"<color=#FFCC00>{key2} : {FengGameManagerMKII.banHash[key2]} </color>{Environment.NewLine}";
		}
		Write(text);
	}

	[MyCommand("unban")]
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
				//Chat("<color=#FFCC00>" + text + " has been unbanned from the server. </color>");
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

	[MyCommand("team")]
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

	[MyCommand("j")]
	private void Rejoin(string[] args)
	{
		PhotonNetwork.networkingPeer.OnStatusChanged(StatusCode.DisconnectByServerLogic);
	}

	[MyCommand("rules")]
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
