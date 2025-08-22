using ExitGames.Client.Photon;
using UnityEngine;

public class CostumeConeveter
{
	private static int DivisionToInt(DIVISION id)
	{
		return id switch
		{
			DIVISION.TheGarrison => 0, 
			DIVISION.TheMilitaryPolice => 1, 
			DIVISION.TraineesSquad => 3, 
			_ => 2, 
		};
	}

	public static void HeroCostumeToLocalData(HeroCostume costume, string slot)
	{
		slot = slot.ToUpper();
		PlayerPrefs.SetInt(slot + "sex", SexToInt(costume.sex));
		PlayerPrefs.SetInt(slot + "costumeId", costume.costumeId);
		PlayerPrefs.SetInt(slot + "heroCostumeId", costume.id);
		PlayerPrefs.SetInt(slot + "cape", costume.cape ? 1 : 0);
		PlayerPrefs.SetInt(slot + "hairInfo", costume.hairInfo.id);
		PlayerPrefs.SetInt(slot + "eye_texture_id", costume.eye_texture_id);
		PlayerPrefs.SetInt(slot + "beard_texture_id", costume.beard_texture_id);
		PlayerPrefs.SetInt(slot + "glass_texture_id", costume.glass_texture_id);
		PlayerPrefs.SetInt(slot + "skin_color", costume.skin_color);
		PlayerPrefs.SetFloat(slot + "hair_color1", costume.hair_color.r);
		PlayerPrefs.SetFloat(slot + "hair_color2", costume.hair_color.g);
		PlayerPrefs.SetFloat(slot + "hair_color3", costume.hair_color.b);
		PlayerPrefs.SetInt(slot + "division", DivisionToInt(costume.division));
		PlayerPrefs.SetInt(slot + "statSPD", costume.stat.SPD);
		PlayerPrefs.SetInt(slot + "statGAS", costume.stat.GAS);
		PlayerPrefs.SetInt(slot + "statBLA", costume.stat.BLA);
		PlayerPrefs.SetInt(slot + "statACL", costume.stat.ACL);
		PlayerPrefs.SetString(slot + "statSKILL", costume.stat.skillId);
	}

	public static void HeroCostumeToPhotonData2(HeroCostume costume, PhotonPlayer player)
	{
		Hashtable hashtable = new Hashtable();
		hashtable.Add("sex", SexToInt(costume.sex));
		int num = costume.costumeId;
		if (num == 26)
		{
			num = 25;
		}
		hashtable.Add("costumeId", num);
		hashtable.Add("heroCostumeId", costume.id);
		hashtable.Add("cape", costume.cape);
		hashtable.Add("hairInfo", costume.hairInfo.id);
		hashtable.Add("eye_texture_id", costume.eye_texture_id);
		hashtable.Add("beard_texture_id", costume.beard_texture_id);
		hashtable.Add("glass_texture_id", costume.glass_texture_id);
		hashtable.Add("skin_color", costume.skin_color);
		hashtable.Add("hair_color1", costume.hair_color.r);
		hashtable.Add("hair_color2", costume.hair_color.g);
		hashtable.Add("hair_color3", costume.hair_color.b);
		hashtable.Add("division", DivisionToInt(costume.division));
		hashtable.Add("statSPD", costume.stat.SPD);
		hashtable.Add("statGAS", costume.stat.GAS);
		hashtable.Add("statBLA", costume.stat.BLA);
		hashtable.Add("statACL", costume.stat.ACL);
		hashtable.Add("statSKILL", costume.stat.skillId);
		player.SetCustomProperties(hashtable);
	}

	private static DIVISION IntToDivision(int id)
	{
		return id switch
		{
			0 => DIVISION.TheGarrison, 
			1 => DIVISION.TheMilitaryPolice, 
			3 => DIVISION.TraineesSquad, 
			_ => DIVISION.TheSurveryCorps, 
		};
	}

	private static SEX IntToSex(int id)
	{
		if (id == 0)
		{
			return SEX.FEMALE;
		}
		_ = 1;
		return SEX.MALE;
	}

	private static UNIFORM_TYPE IntToUniformType(int id)
	{
		return id switch
		{
			0 => UNIFORM_TYPE.CasualA, 
			1 => UNIFORM_TYPE.CasualB, 
			3 => UNIFORM_TYPE.UniformB, 
			4 => UNIFORM_TYPE.CasualAHSS, 
			_ => UNIFORM_TYPE.UniformA, 
		};
	}

	public static HeroCostume LocalDataToHeroCostume(string slot)
	{
		slot = slot.ToUpper();
		if (!PlayerPrefs.HasKey(slot + "sex"))
		{
			return HeroCostume.costume[0];
		}
		new HeroCostume();
		HeroCostume heroCostume = new HeroCostume();
		heroCostume.sex = IntToSex(PlayerPrefs.GetInt(slot + "sex"));
		heroCostume.id = PlayerPrefs.GetInt(slot + "heroCostumeId");
		heroCostume.costumeId = PlayerPrefs.GetInt(slot + "costumeId");
		heroCostume.cape = ((PlayerPrefs.GetInt(slot + "cape") == 1) ? true : false);
		heroCostume.hairInfo = ((IntToSex(PlayerPrefs.GetInt(slot + "sex")) == SEX.FEMALE) ? CostumeHair.hairsF[PlayerPrefs.GetInt(slot + "hairInfo")] : CostumeHair.hairsM[PlayerPrefs.GetInt(slot + "hairInfo")]);
		heroCostume.eye_texture_id = PlayerPrefs.GetInt(slot + "eye_texture_id");
		heroCostume.beard_texture_id = PlayerPrefs.GetInt(slot + "beard_texture_id");
		heroCostume.glass_texture_id = PlayerPrefs.GetInt(slot + "glass_texture_id");
		heroCostume.skin_color = PlayerPrefs.GetInt(slot + "skin_color");
		heroCostume.hair_color = new Color(PlayerPrefs.GetFloat(slot + "hair_color1"), PlayerPrefs.GetFloat(slot + "hair_color2"), PlayerPrefs.GetFloat(slot + "hair_color3"));
		heroCostume.division = IntToDivision(PlayerPrefs.GetInt(slot + "division"));
		heroCostume.stat = new HeroStat();
		heroCostume.stat.SPD = PlayerPrefs.GetInt(slot + "statSPD");
		heroCostume.stat.GAS = PlayerPrefs.GetInt(slot + "statGAS");
		heroCostume.stat.BLA = PlayerPrefs.GetInt(slot + "statBLA");
		heroCostume.stat.ACL = PlayerPrefs.GetInt(slot + "statACL");
		heroCostume.stat.skillId = PlayerPrefs.GetString(slot + "statSKILL");
		heroCostume.setBodyByCostumeId();
		heroCostume.setMesh2();
		heroCostume.setTexture();
		return heroCostume;
	}

	public static HeroCostume PhotonDataToHeroCostume2(PhotonPlayer player)
	{
		HeroCostume heroCostume = new HeroCostume();
		SEX sEX = IntToSex((int)player.customProperties["sex"]);
		heroCostume = new HeroCostume
		{
			sex = sEX,
			costumeId = (int)player.customProperties["costumeId"],
			id = (int)player.customProperties["heroCostumeId"],
			cape = (bool)player.customProperties["cape"],
			hairInfo = ((sEX != 0) ? CostumeHair.hairsF[(int)player.customProperties["hairInfo"]] : CostumeHair.hairsM[(int)player.customProperties["hairInfo"]]),
			eye_texture_id = (int)player.customProperties["eye_texture_id"],
			beard_texture_id = (int)player.customProperties["beard_texture_id"],
			glass_texture_id = (int)player.customProperties["glass_texture_id"],
			skin_color = (int)player.customProperties["skin_color"],
			hair_color = new Color((float)player.customProperties["hair_color1"], (float)player.customProperties["hair_color2"], (float)player.customProperties["hair_color3"]),
			division = IntToDivision((int)player.customProperties["division"]),
			stat = new HeroStat()
		};
		heroCostume.stat.SPD = (int)player.customProperties["statSPD"];
		heroCostume.stat.GAS = (int)player.customProperties["statGAS"];
		heroCostume.stat.BLA = (int)player.customProperties["statBLA"];
		heroCostume.stat.ACL = (int)player.customProperties["statACL"];
		heroCostume.stat.skillId = (string)player.customProperties["statSKILL"];
		if (heroCostume.costumeId == 25 && heroCostume.sex == SEX.FEMALE)
		{
			heroCostume.costumeId = 26;
		}
		heroCostume.setBodyByCostumeId();
		heroCostume.setMesh2();
		heroCostume.setTexture();
		return heroCostume;
	}

	private static int SexToInt(SEX id)
	{
		if (id == SEX.FEMALE)
		{
			return 0;
		}
		return 1;
	}

	private static int UniformTypeToInt(UNIFORM_TYPE id)
	{
		return id switch
		{
			UNIFORM_TYPE.CasualA => 0, 
			UNIFORM_TYPE.CasualB => 1, 
			UNIFORM_TYPE.UniformB => 3, 
			UNIFORM_TYPE.CasualAHSS => 4, 
			_ => 2, 
		};
	}
}
