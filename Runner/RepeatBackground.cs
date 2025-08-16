using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        repeatWidth = GetComponent<BoxCollider>().size.x/2 * transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (startPos.x - repeatWidth > transform.position.x)
        {
            InitializePosition();
        }
    }
    public void InitializePosition() { transform.position = startPos; }
}
