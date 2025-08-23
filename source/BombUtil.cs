using UnityEngine;

public class BombUtil
{
	public static Color GetBombColor(PhotonPlayer player, float minAlpha = 0.5f)
	{
		if (player.isLocal)
		{
			return GetBombColorIndividual(player, minAlpha);
		}
		if (GameSettings.teamMode > 0)
		{
			int num = RCextensions.returnIntFromObject(PhotonNetwork.player.customProperties["RCteam"]);
			int num2 = RCextensions.returnIntFromObject(player.customProperties["RCteam"]);
			if (num != 0)
			{
				if (num != num2)
				{
					return new Color(FengGameManagerMKII.ETeamColorFloats0, FengGameManagerMKII.ETeamColorFloats1, FengGameManagerMKII.ETeamColorFloats2);
				}
				return new Color(FengGameManagerMKII.TeamColorFloats0, FengGameManagerMKII.TeamColorFloats1, FengGameManagerMKII.TeamColorFloats2);
			}
			switch (num2)
			{
			case 1:
				return Color.cyan;
			case 2:
				return Color.magenta;
			}
		}
		return GetBombColorIndividual(player, minAlpha);
	}

	private static Color GetBombColorIndividual(PhotonPlayer player, float minAlpha)
	{
		float r = RCextensions.returnFloatFromObject(player.customProperties["RCBombR"]);
		float g = RCextensions.returnFloatFromObject(player.customProperties["RCBombG"]);
		float b = RCextensions.returnFloatFromObject(player.customProperties["RCBombB"]);
		float b2 = RCextensions.returnFloatFromObject(player.customProperties["RCBombA"]);
		b2 = Mathf.Max(minAlpha, b2);
		return new Color(r, g, b, b2);
	}

	public static float GetOldRadiusCost(float radius)
	{
		return (radius - 20f) / 4f;
	}

	public static float GetOldSpeedCost(float speed)
	{
		return (speed - 200f) / 60f;
	}

	public static float GetOldCooldownCost(float cooldown)
	{
		return (cooldown - 5f) / -0.4f;
	}

	public static float GetOldRangeCost(float range)
	{
		return (range - 200f) / 60f;
	}

	public static float GetOldBombRadius(float pointsSpent)
	{
		return pointsSpent * 4f + 20f;
	}

	public static float GetOldBombSpeed(float pointsSpent)
	{
		return pointsSpent * 60f + 200f;
	}

	public static float GetOldBombCooldown(float pointsSpent)
	{
		return pointsSpent * -0.4f + 5f;
	}

	public static float GetOldBombRange(float pointsSpent)
	{
		return pointsSpent * 60f + 200f;
	}

	public static float GetBombStat(float pointsSpent, float maxValue, float valuePerPoint, float cutoff)
	{
		if (cutoff > 10f)
		{
			return maxValue - (10f - pointsSpent) * valuePerPoint;
		}
		if (pointsSpent == 10f)
		{
			return maxValue;
		}
		if (pointsSpent > cutoff)
		{
			float num = 10f - pointsSpent;
			return maxValue - num * valuePerPoint * 0.5f;
		}
		return maxValue - ((10f - cutoff) * valuePerPoint * 0.5f + (cutoff - pointsSpent) * valuePerPoint);
	}

	public static float GetBombRadius(float pointsSpent, float oldMinCost, float oldMaxCost, float cutoff)
	{
		float valuePerPoint = (GetOldBombRadius(oldMaxCost) - GetOldBombRadius(oldMinCost)) / 10f;
		float oldBombRadius = GetOldBombRadius(oldMaxCost);
		return GetBombStat(pointsSpent, oldBombRadius, valuePerPoint, cutoff);
	}

	public static float GetBombRange(float pointsSpent, float oldMinCost, float oldMaxCost, float cutoff)
	{
		float valuePerPoint = (GetOldBombRange(oldMaxCost) - GetOldBombRange(oldMinCost)) / 10f;
		float oldBombRange = GetOldBombRange(oldMaxCost);
		return GetBombStat(pointsSpent, oldBombRange, valuePerPoint, cutoff);
	}

	public static float GetBombSpeed(float pointsSpent, float oldMinCost, float oldMaxCost, float cutoff)
	{
		float valuePerPoint = (GetOldBombSpeed(oldMaxCost) - GetOldBombSpeed(oldMinCost)) / 10f;
		float oldBombSpeed = GetOldBombSpeed(oldMaxCost);
		return GetBombStat(pointsSpent, oldBombSpeed, valuePerPoint, cutoff);
	}

	public static float GetBombCooldown(float pointsSpent, float oldMinCost, float oldMaxCost, float cutoff)
	{
		float valuePerPoint = (GetOldBombCooldown(oldMaxCost) - GetOldBombCooldown(oldMinCost)) / 10f;
		float oldBombCooldown = GetOldBombCooldown(oldMaxCost);
		return GetBombStat(pointsSpent, oldBombCooldown, valuePerPoint, cutoff);
	}
}
