using System.Collections.Generic;
using UnityEngine;

public class CharacterCreateAnimationControl : MonoBehaviour
{
	private static Dictionary<string, int> f__switchmap0;

	private string currentAnimation;

	private float interval = 10f;

	private HERO_SETUP setup;

	private float timeElapsed;

	private void play(string id)
	{
		currentAnimation = id;
		base.animation.Play(id);
	}

	public void playAttack(string id)
	{
		if (id != null)
		{
			if (f__switchmap0 == null)
			{
				f__switchmap0 = new Dictionary<string, int>(7)
				{
					{ "mikasa", 0 },
					{ "levi", 1 },
					{ "sasha", 2 },
					{ "jean", 3 },
					{ "marco", 4 },
					{ "armin", 5 },
					{ "petra", 6 },
					{ "sit", 7 },
					{ "stand", 8 },
					{ "ride", 9 },
					{ "jump", 10 },
					{ "supply", 11 },
					{ "salute", 12 },
					{ "dodge", 13 }
				};
			}
			if (f__switchmap0.TryGetValue(id, out var value))
			{
				switch (value)
				{
				case 0:
					currentAnimation = "attack3_1";
					break;
				case 1:
					currentAnimation = "attack5";
					break;
				case 2:
					currentAnimation = "special_sasha";
					break;
				case 3:
					currentAnimation = "grabbed_jean";
					break;
				case 4:
					currentAnimation = "special_marco_0";
					break;
				case 5:
					currentAnimation = "special_armin";
					break;
				case 6:
					currentAnimation = "special_petra";
					break;
				case 7:
					currentAnimation = "horse_idle";
					break;
				case 8:
					currentAnimation = "idle";
					break;
				case 9:
					currentAnimation = "horse_run";
					break;
				case 10:
					currentAnimation = "jump";
					break;
				case 11:
					currentAnimation = "supply";
					break;
				case 12:
					currentAnimation = "salute";
					break;
				case 13:
					currentAnimation = "dodge";
					break;
				}
			}
		}
		base.animation.Play(currentAnimation);
	}

	private void Start()
	{
		setup = base.gameObject.GetComponent<HERO_SETUP>();
		currentAnimation = "stand_levi";
		play(currentAnimation);
	}

	public void toStand()
	{
		if (setup.myCostume.sex == SEX.FEMALE)
		{
			currentAnimation = "stand";
		}
		else
		{
			currentAnimation = "stand_levi";
		}
		base.animation.CrossFade(currentAnimation, 0.1f);
		timeElapsed = 0f;
	}

	private void Update()
	{
		if (currentAnimation == "stand" || currentAnimation == "stand_levi")
		{
			timeElapsed += Time.deltaTime;
			if (timeElapsed > interval)
			{
				timeElapsed = 0f;
				if (Random.Range(1, 1000) < 350)
				{
					play("salute");
				}
				else if (Random.Range(1, 1000) < 350)
				{
					play("supply");
				}
				else
				{
					play("dodge");
				}
			}
		}
		else if (base.animation[currentAnimation].normalizedTime >= 1f)
		{
			if (currentAnimation == "attack3_1")
			{
				play("attack3_2");
			}
			else if (currentAnimation == "special_sasha")
			{
				play("run_sasha");
			}
			else
			{
				toStand();
			}
		}
	}
}
