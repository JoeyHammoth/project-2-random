using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOffscreen : MonoBehaviour
{
    private void Start() 
    {
        Destroy(gameObject, 5f);
    }

}
