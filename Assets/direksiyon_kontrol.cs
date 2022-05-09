using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class direksiyon_kontrol : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool direksiyon_tutuluyor = false;
    public RectTransform Wheel;
    private float WheelAngel = 0f;
    private float LastWheelAngel = 0f;
    private Vector2 center;
    public float MaxSteerAngle = 200f;
    public float ReleaseSpeed = 300f;
    public float OutPut;


    private void Update()
    {
        if(!direksiyon_tutuluyor&&WheelAngel!=0f)
        {
            float DeltaAngle = ReleaseSpeed * Time.deltaTime;
            if (Mathf.Abs(DeltaAngle) > Mathf.Abs(WheelAngel))
                WheelAngel = 0f;
            else if (WheelAngel > 0f)
                WheelAngel -= DeltaAngle;
            else
                WheelAngel += DeltaAngle;
        }
        Wheel.localEulerAngles = new Vector3(0, 0, -WheelAngel);
        OutPut = WheelAngel / MaxSteerAngle;
    }
    public void OnPointerDown(PointerEventData data)
    {
        direksiyon_tutuluyor = true;
        center = RectTransformUtility.WorldToScreenPoint(data.pressEventCamera, Wheel.position);
        LastWheelAngel = Vector2.Angle(Vector2.up, data.position - center);
        
    }
    public void OnDrag(PointerEventData data)
    {
        float NewAngle = Vector2.Angle(Vector2.up, data.position - center);
        if ((data.position - center).sqrMagnitude >= 400)
        {
            if (data.position.x > center.x)
                WheelAngel += NewAngle - LastWheelAngel;
            else
                WheelAngel -= NewAngle - LastWheelAngel;
        }
        WheelAngel = Mathf.Clamp(WheelAngel, -MaxSteerAngle, MaxSteerAngle);
        LastWheelAngel = NewAngle;
    }
    public void OnPointerUp(PointerEventData data)
    {
        OnDrag(data);
        direksiyon_tutuluyor = false;
    }


}
