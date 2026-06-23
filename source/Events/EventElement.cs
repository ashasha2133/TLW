namespace Events;

public class EventElement
{
	protected event EventDelegate eventdelegate;

	public void Dispatch(string kEvent)
	{
		if (this.eventdelegate != null)
		{
			this.eventdelegate(kEvent);
		}
	}

	public static EventElement operator +(EventElement kElement, EventDelegate kDelegate)
	{
		kElement.eventdelegate += kDelegate;
		return kElement;
	}

	public static EventElement operator -(EventElement kElement, EventDelegate kDelegate)
	{
		kElement.eventdelegate -= kDelegate;
		return kElement;
	}
}
