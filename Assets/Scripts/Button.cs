using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Button : MonoBehaviour
{
    [SerializeField]
    private float MinPushDistance = 0.008f;

    [SerializeField]
    private float MaxPushDistance = 0.01f;

    [SerializeField]
    private float ResetTolerance;


    private bool IsPushed;

    public UnityEvent OnPushed;

    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ButtonLocalPosition = this.transform.localPosition;
        ButtonLocalPosition.x = 0;
        ButtonLocalPosition.z = 0;
        ButtonLocalPosition.y = Mathf.Clamp(ButtonLocalPosition.y, MinPushDistance, MaxPushDistance); /* Reset Max value after watching last 20 minutes of Unit 3 lesson 3
                                                                                             Need to fix settings on Button Frame*/
        this.transform.localPosition = ButtonLocalPosition;

        if (!IsPushed && ButtonLocalPosition.y <= MinPushDistance)
        {
            IsPushed = true;
            print("button pressed");

            OnPushed.Invoke();
        }

        else if (IsPushed && (ButtonLocalPosition.y >= MaxPushDistance - ResetTolerance)) // y in this case is whatever max value is
        {
            IsPushed = false;
            print("button not pressed");
        }

    }

    public void Test()
    {
        print("Invoked");
    }
}



