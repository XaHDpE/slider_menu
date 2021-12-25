using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Lean.Touch;

namespace leanExtended
{
	/// <summary>This component allows you to get the pinch of all fingers.</summary>
	[HelpURL(LeanTouch.PlusHelpUrlPrefix + "LeanMultiDirectionExtended")]
	[AddComponentMenu(LeanTouch.ComponentPathPrefix + "Multi Direction Extended")]
	public class LeanMultiDirectionExtended : MonoBehaviour
	{
		public enum CoordinateType
		{
			ScaledPixels,
			ScreenPixels,
			ScreenPercentage
		}

		[System.Serializable] public class FloatEvent : UnityEvent<List<LeanFinger>, float> {}

		/// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
		public LeanFingerFilter use = new LeanFingerFilter(true);

		/// <summary>If there is no movement, ignore it?</summary>
		public bool IgnoreIfStatic { set { ignoreIfStatic = value; } get { return ignoreIfStatic; } } [SerializeField] private bool ignoreIfStatic;

		/// <summary>The angle of the arc in degrees that the swipe must be inside.
		/// -1 = No requirement.
		/// 90 = Quarter circle (+- 45 degrees).
		/// 180 = Semicircle (+- 90 degrees).</summary>
		public float RequiredArc { set { requiredArc = value; } get { return requiredArc; } } [SerializeField] private float requiredArc = -1.0f;
		
		/// <summary>The angle we want to detect movement along.
		/// 0 = Up.
		/// 90 = Right.
		/// 180 = Down.
		/// 270 = Left.</summary>
		public float RequiredAngle { set { requiredAngle = value; } get { return requiredAngle; } }  [SerializeField] private float requiredAngle;

		/// <summary>Set delta to 0 if it goes negative?</summary>
		public bool OneWay { set { oneWay = value; } get { return oneWay; } } [SerializeField] private bool oneWay;

		/// <summary>The coordinate space of the <b>OnDelta</b> values.</summary>
		public CoordinateType Coordinate { set { coordinate = value; } get { return coordinate; } } [SerializeField] private CoordinateType coordinate;

		/// <summary>The swipe delta will be multiplied by this value.</summary>
		public float Multiplier { set { multiplier = value; } get { return multiplier; } } [SerializeField] private float multiplier = 1.0f;

		/// <summary>This event is invoked when the requirements are met.
		/// Float = Position Delta based on your Coordinate setting.</summary>
		public FloatEvent OnDelta { get { if (onDelta == null) onDelta = new FloatEvent(); return onDelta; } } [SerializeField] private FloatEvent onDelta;

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually add a finger.</summary>
		public void AddFinger(LeanFinger finger)
		{
			use.AddFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove a finger.</summary>
		public void RemoveFinger(LeanFinger finger)
		{
			use.RemoveFinger(finger);
		}

		/// <summary>If you've set Use to ManuallyAddedFingers, then you can call this method to manually remove all fingers.</summary>
		public void RemoveAllFingers()
		{
			use.RemoveAllFingers();
		}

		protected virtual void Awake()
		{
			use.UpdateRequiredSelectable(gameObject);
		}

		private bool AngleIsValid(Vector2 vector)
		{
			if (!(requiredArc >= 0.0f)) return true;

			var angle      = Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
			angle = oneWay ? angle : Mathf.Abs(angle);
			
			var angleDelta = Mathf.DeltaAngle(angle, requiredAngle);
				
			return !(angleDelta < requiredArc * -0.5f) && !(angleDelta >= requiredArc * 0.5f);
			
		}
		

		protected virtual void Update()
		{
			// Get fingers
			var fingers = use.UpdateAndGetFingers();

			if (!AngleIsValid(LeanGesture.GetScreenDelta(fingers))) return;

			if (fingers.Count > 0 && onDelta != null)
			{
				var finalDelta = (Quaternion.Euler(0.0f, 0.0f, requiredAngle) * LeanGesture.GetScreenDelta(fingers)).y;

				switch (coordinate)
				{
					case CoordinateType.ScaledPixels:     finalDelta *= LeanTouch.ScalingFactor; break;
					case CoordinateType.ScreenPercentage: finalDelta *= LeanTouch.ScreenFactor;  break;
				}

				if (oneWay && finalDelta < 0.0f)
				{
					finalDelta = 0.0f;
				}

				finalDelta *= multiplier;

				onDelta.Invoke(fingers, finalDelta);
			}
		}
	}
}
