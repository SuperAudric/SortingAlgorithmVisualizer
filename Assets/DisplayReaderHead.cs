using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayReaderHead : MonoBehaviour
{
    // Start is called before the first frame update
public void Display(int position, int Others)
    {
        transform.position = new Vector3((position+0.5f)/Others*10-5, transform.position.y, transform.position.z);
    }
}
