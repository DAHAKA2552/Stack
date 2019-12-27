using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private bool isStartMove;

    void Awake()
    {
        isStartMove = true;
    }

    public void StopMoving()
    {
        StopAllCoroutines();
    }

    public IEnumerator MovePlatform(Vector3 start, Vector3 end, GameObject obj, float speed)
    {
        while(true)
        {
            if (isStartMove)
            {
                while (obj.transform.position != end)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, end, Time.deltaTime * speed);
                    yield return null;
                }
                isStartMove = false;
            }
            else
            {
                while (obj.transform.position != start)
                {
                    obj.transform.position = Vector3.MoveTowards(obj.transform.position, start, Time.deltaTime * speed);
                    yield return null;
                }
                isStartMove = true;
            }
        }
    }
}
