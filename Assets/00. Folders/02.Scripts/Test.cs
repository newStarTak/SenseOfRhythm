using UnityEngine;

public class Test : MonoBehaviour
{
    public Transform t1;
    public Transform t2;
    public Transform target;

    void Start()
    {
        Debug.Log(t1.position - t2.position);
        Debug.Log((t1.position - t2.position) / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position == t1.position)
        {
            target = t2;
            //Debug.Log(Time.deltaTime);
        }
        else if (transform.position == t2.position)
        {
            target = t1;
            //Debug.Log(Time.deltaTime);
        }
        transform.position = Vector3.MoveTowards(transform.position, target.position, 20f * Time.deltaTime);
    }
}
