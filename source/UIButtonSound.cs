using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Sound")]
public class UIButtonSound : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease
	}

	public AudioClip audioClip;

	public float pitch = 1f;

	public Trigger trigger;

	public float volume = 1f;

	private void OnClick()
	{
		if (base.enabled && trigger == Trigger.OnClick && AudioListener.volume > 0f && FengGameManagerMKII.InterfaceVolume > 0f && GameSettings.Clicking > 0)
		{
			NGUITools.PlaySound(audioClip, volume, pitch);
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.enabled && ((isOver && trigger == Trigger.OnMouseOver) || (!isOver && trigger == Trigger.OnMouseOut)) && AudioListener.volume > 0f && FengGameManagerMKII.InterfaceVolume > 0f && GameSettings.Clicking > 0)
		{
			NGUITools.PlaySound(audioClip, volume, pitch);
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.enabled && ((isPressed && trigger == Trigger.OnPress) || (!isPressed && trigger == Trigger.OnRelease)) && AudioListener.volume > 0f && FengGameManagerMKII.InterfaceVolume > 0f && GameSettings.Clicking > 0)
		{
			NGUITools.PlaySound(audioClip, volume, pitch);
		}
	}
}
