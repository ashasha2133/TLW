using BRM;
using UnityEngine;

public class TITAN_CONTROLLER : MonoBehaviour
{
	public bool bite;

	public bool bitel;

	public bool biter;

	public bool chopl;

	public bool chopr;

	public bool choptl;

	public bool choptr;

	public bool cover;

	public Camera currentCamera;

	public float currentDirection;

	public bool grabbackl;

	public bool grabbackr;

	public bool grabfrontl;

	public bool grabfrontr;

	public bool grabnapel;

	public bool grabnaper;

	public FengCustomInputs inputManager;

	public bool isAttackDown;

	public bool isAttackIIDown;

	public bool isHorse;

	public bool isJumpDown;

	public bool isSuicide;

	public bool isWALKDown;

	public bool sit;

	public float targetDirection;

	private void Start()
	{
		inputManager = CacheGameObject.Find("InputManagerController").GetComponent<FengCustomInputs>();
		currentCamera = CacheGameObject.Find("MainCamera").GetComponent<Camera>();
		if (IN_GAME_MAIN_CAMERA.gametype == GAMETYPE.SINGLE)
		{
			base.enabled = false;
		}
	}

	private void Update()
	{
		int num;
		int num2;
		float num5;
		if (isHorse)
		{
			num = (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseForward) ? 1 : (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseBack) ? (-1) : 0));
			num2 = (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseLeft) ? (-1) : (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseRight) ? 1 : 0));
			if (num2 != 0 || num != 0)
			{
				float y = currentCamera.transform.rotation.eulerAngles.y;
				float num3 = Mathf.Atan2(num, num2) * 57.29578f;
				num3 = 0f - num3 + 90f;
				float num4 = y + num3;
				targetDirection = num4;
			}
			else
			{
				targetDirection = -874f;
			}
			isAttackDown = false;
			isAttackIIDown = false;
			if (targetDirection != -874f)
			{
				currentDirection = targetDirection;
			}
			num5 = currentCamera.transform.rotation.eulerAngles.y - currentDirection;
			if (num5 >= 180f)
			{
				num5 -= 360f;
			}
			if (FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseJump))
			{
				isAttackDown = true;
			}
			isWALKDown = FengGameManagerMKII.inputRC.isInputHorse(InputCodeRC.horseWalk);
			return;
		}
		num = (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanForward) ? 1 : (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanBack) ? (-1) : 0));
		num2 = (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanLeft) ? (-1) : (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanRight) ? 1 : 0));
		if (num2 != 0 || num != 0)
		{
			float y2 = currentCamera.transform.rotation.eulerAngles.y;
			float num3 = Mathf.Atan2(num, num2) * 57.29578f;
			num3 = 0f - num3 + 90f;
			float num4 = y2 + num3;
			targetDirection = num4;
		}
		else
		{
			targetDirection = -874f;
		}
		isAttackDown = false;
		isJumpDown = false;
		isAttackIIDown = false;
		isSuicide = false;
		grabbackl = false;
		grabbackr = false;
		grabfrontl = false;
		grabfrontr = false;
		grabnapel = false;
		grabnaper = false;
		choptl = false;
		chopr = false;
		chopl = false;
		choptr = false;
		bite = false;
		bitel = false;
		biter = false;
		cover = false;
		sit = false;
		if (targetDirection != -874f)
		{
			currentDirection = targetDirection;
		}
		num5 = currentCamera.transform.rotation.eulerAngles.y - currentDirection;
		if (num5 >= 180f)
		{
			num5 -= 360f;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanPunch))
		{
			isAttackDown = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanSlam))
		{
			isAttackIIDown = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanJump))
		{
			isJumpDown = true;
		}
		if (inputManager.GetComponent<FengCustomInputs>().isInputDown[InputCode.restart])
		{
			isSuicide = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanCover))
		{
			cover = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanSit))
		{
			sit = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabFront) && num5 >= 0f)
		{
			grabfrontr = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabFront) && num5 < 0f)
		{
			grabfrontl = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabBack) && num5 >= 0f)
		{
			grabbackr = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabBack) && num5 < 0f)
		{
			grabbackl = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabNape) && num5 >= 0f)
		{
			grabnaper = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanGrabNape) && num5 < 0f)
		{
			grabnapel = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanAntiAE) && num5 >= 0f)
		{
			choptr = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanAntiAE) && num5 < 0f)
		{
			choptl = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanBite) && num5 > 7.5f)
		{
			biter = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanBite) && num5 < -7.5f)
		{
			bitel = true;
		}
		if (FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanBite) && num5 >= -7.5f && num5 <= 7.5f)
		{
			bite = true;
		}
		isWALKDown = FengGameManagerMKII.inputRC.isInputTitan(InputCodeRC.titanWalk);
	}
}
