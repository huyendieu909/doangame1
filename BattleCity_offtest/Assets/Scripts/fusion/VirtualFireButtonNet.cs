using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualFireButtonNet : MonoBehaviour
{
    public static bool IsFiring = false;
    

    // Gán sự kiện cho nút, ví dụ nút bắn OnClick
    public void OnFireButtonDown()
    {
        IsFiring = true;
    }
    
    public void OnFireButtonUp()
    {
        IsFiring = false;
    }
}
