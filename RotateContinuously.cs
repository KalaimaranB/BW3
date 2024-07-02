using UnityEngine;

public class RotateContinuously : MonoBehaviour
{
    public enum Direction {x,y,z,code};
    public Direction direction = Direction.y;
    public float RotationsPerMinute = 10;

    

    void Update()
    {
        switch (direction)
        {
            case Direction.x:
                transform.Rotate(6.0f * RotationsPerMinute * Time.deltaTime, 0, 0);
                break;
            case Direction.y:
                transform.Rotate(0, 6.0f * RotationsPerMinute * Time.deltaTime, 0);
                break;
            case Direction.z:
                transform.Rotate(0,0,6.0f * RotationsPerMinute * Time.deltaTime);
                break;
            default:
                break;
        }

        
    }
}
