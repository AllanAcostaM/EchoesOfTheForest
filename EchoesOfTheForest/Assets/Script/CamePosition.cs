using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamePosition : MonoBehaviour
{
    
    public GameObject player;
    private Vector3 offset = new Vector3(-0.07f, 2.22f, -0.23f);

    // Update is called once per frame
    private void LateUpdate()
    {
        transform.position = player.transform.position + offset * Time.deltaTime;
    }


}
