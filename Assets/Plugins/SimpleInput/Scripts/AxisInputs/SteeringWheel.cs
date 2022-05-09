using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SimpleInputNamespace
{
	public class SteeringWheel : MonoBehaviour, ISimpleInputDraggable
	{
		public SimpleInput.AxisInput axis = new SimpleInput.AxisInput( "Horizontal" );

		private Graphic wheel;

		private RectTransform wheelTR;
		private Vector2 centerPoint;

		public float maximumSteeringAngle = 200f;
		public float wheelReleasedSpeed = 350f;
		public float valueMultiplier = 1f;

		private float wheelAngle = 0f;
		private float wheelPrevAngle = 0f;

		private bool wheelBeingHeld = false;

		private float m_value;
		public float Value { get { return m_value; } }

		public float Angle { get { return wheelAngle; } }

		private void Awake()
		{
			wheel = GetComponent<Graphic>();
			wheelTR = wheel.rectTransform;

			SimpleInputDragListener eventReceiver = gameObject.AddComponent<SimpleInputDragListener>();
			eventReceiver.Listener = this;
		}

		private void OnEnable()
		{
			axis.StartTracking();
			SimpleInput.OnUpdate += OnUpdate;
		}

		private void OnDisable()
		{
			axis.StopTracking();
			SimpleInput.OnUpdate -= OnUpdate;
		}

		private void OnUpdate()
		{
			// Tekerlek serbest bırakılırsa, dönüşü sıfırlayın
			// tekerlek tarafından ilk (sıfır) dönüşe
			if ( !wheelBeingHeld && wheelAngle != 0f )
			{
				float deltaAngle = wheelReleasedSpeed * Time.deltaTime;
				if( Mathf.Abs( deltaAngle ) > Mathf.Abs( wheelAngle ) )
					wheelAngle = 0f;
				else if( wheelAngle > 0f )
					wheelAngle -= deltaAngle;
				else
					wheelAngle += deltaAngle;
			}

			// Tekerlek görüntüsünü döndür
			wheelTR.localEulerAngles = new Vector3( 0f, 0f, -wheelAngle );

			m_value = wheelAngle * valueMultiplier / maximumSteeringAngle;
			axis.value = m_value;
		}

		public void OnPointerDown( PointerEventData eventData )
		{
			// Fare/parmak direksiyona dokunmaya başladığında yürütülür
			wheelBeingHeld = true;
			centerPoint = RectTransformUtility.WorldToScreenPoint( eventData.pressEventCamera, wheelTR.position );
			wheelPrevAngle = Vector2.Angle( Vector2.up, eventData.position - centerPoint );
		}

		public void OnDrag( PointerEventData eventData )
		{
			// Fare/parmak direksiyon simidi üzerinde sürüklendiğinde yürütülür
			Vector2 pointerPos = eventData.position;

			float wheelNewAngle = Vector2.Angle( Vector2.up, pointerPos - centerPoint );

			// İşaretçi tekerleğin merkezine çok yakınsa hiçbir şey yapmayın
			if ( ( pointerPos - centerPoint ).sqrMagnitude >= 400f )
			{
				if( pointerPos.x > centerPoint.x )
					wheelAngle += wheelNewAngle - wheelPrevAngle;
				else
					wheelAngle -= wheelNewAngle - wheelPrevAngle;
			}

			// Tekerlek açısının asla maksimum Direksiyon Açısı'nı aşmadığından emin olun
			wheelAngle = Mathf.Clamp( wheelAngle, -maximumSteeringAngle, maximumSteeringAngle );
			wheelPrevAngle = wheelNewAngle;
		}

		public void OnPointerUp( PointerEventData eventData )
		{
			// Fare/parmak direksiyona dokunmayı bıraktığında yürütülür
			// Her ihtimale karşı son bir OnDrag hesaplaması yapar
			OnDrag( eventData );

			wheelBeingHeld = false;
		}
	}
}