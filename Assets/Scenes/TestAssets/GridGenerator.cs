using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    GameObject prefab;
	// Use this for initialization
	void Start ()
    {
        Vector2 bottomRightCorner = cam.ScreenToWorldPoint(Vector2.zero);
        // offset by center of cube
        bottomRightCorner.x += .5f;
        bottomRightCorner.y += .5f;
        Vector2 nextPos = bottomRightCorner;
        for (int y = 0; y < 15; ++y)
        {
            for (int x = 0; x < 24; ++x)
            {
                Instantiate(prefab, nextPos, Quaternion.identity, transform);
                nextPos.x += 1;
            }
            nextPos.x = bottomRightCorner.x;
            nextPos.y += 1;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
