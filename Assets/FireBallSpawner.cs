using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpawner : MonoBehaviour
{
    GameObject fireBall;
    public float delay = 3f;
    public enum Direction { left , right, up, down }

    public Direction direction = Direction.right;

    // Start is called before the first frame update
    void Start()
    {
        int dir = 0;
        switch (direction)
        {
            case Direction.left:
                dir = 180;
                break;
            case Direction.right:
                break;
            case Direction.up:
                dir = 90;
                break;
            case Direction.down:
                dir = -90;
                break;
            default:
                break;
        }
        transform.rotation *= Quaternion.Euler(0f, 0f, dir + 90);
        fireBall = Resources.Load<GameObject>("FireBall/FireBallDown");
        //fireBall = Instantiate((GameObject)Resources.Load("FireBall/FireBallDown", typeof(GameObject)), transform.position + new Vector3(0f, -0.04f), Quaternion.identity, transform);
        StartCoroutine(Shot(delay, dir));
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Shot(float delay, int dir)
    {
        Instantiate(fireBall, transform.position + new Vector3(0f, -0.04f), transform.rotation * Quaternion.Euler(0f, 0f, -90), transform);
        yield return new WaitForSeconds(delay);
        yield return Shot(delay, dir);
    }
}
