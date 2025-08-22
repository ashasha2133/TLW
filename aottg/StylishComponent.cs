using UnityEngine;

public class StylishComponent : MonoBehaviour
{
	public static StylishComponent stylebar;

	private Transform baseT;

	private UISprite UIBar;

	private UILabel UIChain;

	public UILabel UIHits;

	public UILabel UIS;

	public UILabel UIS1;

	public UILabel UIS2;

	public UILabel UIsub;

	public UILabel UITotal;

	public GameObject bar;

	private int chainKillRank;

	private float[] chainRankMultiplier;

	private float chainTime;

	private float duration;

	private Vector3 exitPosition;

	private bool flip;

	private bool hasLostRank;

	public GameObject labelChain;

	public GameObject labelHits;

	public GameObject labelS;

	public GameObject labelS1;

	public GameObject labelS2;

	public GameObject labelsub;

	public GameObject labelTotal;

	private Vector3 originalPosition;

	private float R;

	private int styleHits;

	private float stylePoints;

	private int styleRank;

	private int[] styleRankDepletions;

	private int[] styleRankPoints;

	private string[,] styleRankText;

	private int styleTotalDamage;

	private void OnAwake()
	{
		stylebar = this;
	}

	private void OnDestroy()
	{
		stylebar = null;
	}

	public StylishComponent()
	{
		string[,] array = (styleRankText = new string[11, 2]
		{
			{ "D", "eja Vu" },
			{ "C", "asual" },
			{ "B", "oppin!" },
			{ "A", "mazing!" },
			{ "S", "ensational!" },
			{ "S", "pectacular!!" },
			{ "S", "tylish!!!" },
			{ "X", "TREEME!!!" },
			{ "E", "thereal!!!" },
			{ "Z", "enith" },
			{ "G", "ET A LIFE!!!" }
		});
		chainRankMultiplier = new float[12]
		{
			1f, 1.1f, 1.2f, 1.3f, 1.5f, 1.7f, 2f, 2.3f, 2.5f, 2.5f,
			2.5f, 2.5f
		};
		styleRankPoints = new int[10] { 350, 950, 2450, 4550, 7000, 15000, 100000, 250000, 500000, 1000000 };
		styleRankDepletions = new int[11]
		{
			1, 2, 5, 10, 15, 20, 25, 25, 25, 25,
			25
		};
	}

	private int GetRankPercentage()
	{
		if (styleRank > 0 && styleRank < styleRankPoints.Length)
		{
			return (int)((stylePoints - (float)styleRankPoints[styleRank - 1]) * 100f / (float)(styleRankPoints[styleRank] - styleRankPoints[styleRank - 1]));
		}
		if (styleRank == 0)
		{
			return (int)(stylePoints * 100f) / styleRankPoints[styleRank];
		}
		return 100;
	}

	private int GetStyleDepletionRate()
	{
		return styleRankDepletions[styleRank];
	}

	public void reset()
	{
		styleTotalDamage = 0;
		chainKillRank = 0;
		chainTime = 0f;
		styleRank = 0;
		stylePoints = 0f;
		styleHits = 0;
	}

	private void setPosition()
	{
		originalPosition = new Vector3((int)((float)Screen.width * 0.5f - 2f), (int)((float)Screen.height * 0.5f - 150f), 0f);
		exitPosition = new Vector3(Screen.width, originalPosition.y, originalPosition.z);
	}

	private void SetRank()
	{
		int num = styleRank;
		int i;
		for (i = 0; i < styleRankPoints.Length && stylePoints > (float)styleRankPoints[i]; i++)
		{
		}
		if (i < styleRankPoints.Length)
		{
			styleRank = i;
		}
		else
		{
			styleRank = styleRankPoints.Length;
		}
		if (styleRank < num)
		{
			if (hasLostRank)
			{
				stylePoints = 0f;
				styleHits = 0;
				styleTotalDamage = 0;
				styleRank = 0;
			}
			else
			{
				hasLostRank = true;
			}
		}
		else if (styleRank > num)
		{
			hasLostRank = false;
		}
	}

	private void setRankText()
	{
		UIS.text = styleRankText[styleRank, 0];
		if (styleRank == 5)
		{
			UIS2.text = "[" + ColorSet.color_SS + "]S";
		}
		else
		{
			UIS2.text = string.Empty;
		}
		if (styleRank == 6)
		{
			UIS2.text = "[" + ColorSet.color_SSS + "]S";
			UIS1.text = "[" + ColorSet.color_SSS + "]S";
		}
		else
		{
			UIS1.text = string.Empty;
		}
		if (styleRank == 0)
		{
			UIS.text = "[" + ColorSet.color_D + "]D";
		}
		if (styleRank == 1)
		{
			UIS.text = "[" + ColorSet.color_C + "]C";
		}
		if (styleRank == 2)
		{
			UIS.text = "[" + ColorSet.color_B + "]B";
		}
		if (styleRank == 3)
		{
			UIS.text = "[" + ColorSet.color_A + "]A";
		}
		if (styleRank == 4)
		{
			UIS.text = "[" + ColorSet.color_S + "]S";
		}
		if (styleRank == 5)
		{
			UIS.text = "[" + ColorSet.color_SS + "]S";
		}
		if (styleRank == 6)
		{
			UIS.text = "[" + ColorSet.color_SSS + "]S";
		}
		if (styleRank == 7)
		{
			UIS.text = "[" + ColorSet.color_X + "]X";
		}
		if (styleRank == 8)
		{
			UIS.text = "[d4f9fc]E";
		}
		if (styleRank == 9)
		{
			UIS.text = "[ffd4d2]Z";
		}
		if (styleRank == 10)
		{
			UIS.text = "[" + ColorSet.color_X + "]G";
		}
		UIsub.text = styleRankText[styleRank, 1];
	}

	private void shakeUpdate()
	{
		if (duration > 0f)
		{
			duration -= Time.deltaTime;
			if (flip)
			{
				baseT.localPosition = originalPosition + Vector3.up * R;
			}
			else
			{
				baseT.localPosition = originalPosition - Vector3.up * R;
			}
			flip = !flip;
			if (duration <= 0f)
			{
				baseT.localPosition = originalPosition;
			}
		}
	}

	private void Start()
	{
		stylebar = this;
		UIBar = bar.GetComponent<UISprite>();
		UIChain = labelChain.GetComponent<UILabel>();
		UIHits = labelHits.GetComponent<UILabel>();
		UIS = labelS.GetComponent<UILabel>();
		UIS1 = labelS1.GetComponent<UILabel>();
		UIS2 = labelS2.GetComponent<UILabel>();
		UIsub = labelsub.GetComponent<UILabel>();
		UITotal = labelTotal.GetComponent<UILabel>();
		setPosition();
		(baseT = base.transform).localPosition = exitPosition;
	}

	public void startShake(int R, float duration)
	{
		if (this.duration < duration)
		{
			this.R = R;
			this.duration = duration;
		}
	}

	public void Style(int damage)
	{
		if (damage != -1)
		{
			stylePoints += (int)((float)(damage + 200) * chainRankMultiplier[chainKillRank]);
			styleTotalDamage += damage;
			chainKillRank = ((chainKillRank >= chainRankMultiplier.Length - 1) ? chainKillRank : (chainKillRank + 1));
			chainTime = 5f;
			styleHits++;
			SetRank();
		}
		else if (stylePoints == 0f)
		{
			stylePoints += 1f;
			SetRank();
		}
		startShake(5, 0.3f);
		setPosition();
		UITotal.text = ((int)stylePoints).ToString();
		UIHits.text = styleHits + ((styleHits <= 1) ? "Hit" : "Hits");
		if (chainKillRank == 0)
		{
			UIChain.text = string.Empty;
		}
		else
		{
			UIChain.text = "x" + chainRankMultiplier[chainKillRank] + "!";
		}
	}

	private void Update()
	{
		if (!IN_GAME_MAIN_CAMERA.isPausing)
		{
			if (stylePoints > 0f)
			{
				setRankText();
				UIBar.fillAmount = (float)GetRankPercentage() * 0.01f;
				stylePoints -= (float)GetStyleDepletionRate() * Time.deltaTime * 10f;
				SetRank();
			}
			else
			{
				base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, exitPosition, Time.deltaTime * 3f);
			}
			if (chainTime > 0f)
			{
				chainTime -= Time.deltaTime;
			}
			else
			{
				chainTime = 0f;
				chainKillRank = 0;
			}
			shakeUpdate();
		}
	}
}
