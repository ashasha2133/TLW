using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Center On Child")]
public class UICenterOnChild : MonoBehaviour
{
	private GameObject mCenteredObject;

	private UIDraggablePanel mDrag;

	public SpringPanel.OnFinished onFinished;

	public float springStrength = 8f;

	public GameObject centeredObject => mCenteredObject;

	private void OnDragFinished()
	{
		if (base.enabled)
		{
			Recenter();
		}
	}

	private void OnEnable()
	{
		Recenter();
	}

	public void Recenter()
	{
		if (mDrag == null)
		{
			mDrag = NGUITools.FindInParents<UIDraggablePanel>(base.gameObject);
			if (mDrag == null)
			{
				base.enabled = false;
				return;
			}
			mDrag.onDragFinished = OnDragFinished;
			if (mDrag.horizontalScrollBar != null)
			{
				mDrag.horizontalScrollBar.onDragFinished = OnDragFinished;
			}
			if (mDrag.verticalScrollBar != null)
			{
				mDrag.verticalScrollBar.onDragFinished = OnDragFinished;
			}
		}
		if (!(mDrag.panel != null))
		{
			return;
		}
		Vector4 clipRange = mDrag.panel.clipRange;
		Transform cachedTransform = mDrag.panel.cachedTransform;
		Vector3 vector = cachedTransform.localPosition;
		vector.x += clipRange.x;
		vector.y += clipRange.y;
		vector = cachedTransform.parent.TransformPoint(vector);
		Vector3 vector2 = vector - mDrag.currentMomentum * (mDrag.momentumAmount * 0.1f);
		mDrag.currentMomentum = Vector3.zero;
		float num = float.MaxValue;
		Transform transform = null;
		Transform transform2 = base.transform;
		int i = 0;
		for (int childCount = transform2.childCount; i < childCount; i++)
		{
			Transform child = transform2.GetChild(i);
			float num2 = Vector3.SqrMagnitude(child.position - vector2);
			if (num2 < num)
			{
				num = num2;
				transform = child;
			}
		}
		if (transform != null)
		{
			mCenteredObject = transform.gameObject;
			Vector3 vector3 = cachedTransform.InverseTransformPoint(transform.position);
			Vector3 vector4 = cachedTransform.InverseTransformPoint(vector);
			Vector3 vector5 = vector3 - vector4;
			if (mDrag.scale.x == 0f)
			{
				vector5.x = 0f;
			}
			if (mDrag.scale.y == 0f)
			{
				vector5.y = 0f;
			}
			if (mDrag.scale.z == 0f)
			{
				vector5.z = 0f;
			}
			SpringPanel.Begin(mDrag.gameObject, cachedTransform.localPosition - vector5, springStrength).onFinished = onFinished;
		}
		else
		{
			mCenteredObject = null;
		}
	}
}
