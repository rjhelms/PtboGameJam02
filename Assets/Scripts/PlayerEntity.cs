using UnityEngine;

public class PlayerEntity : Actor
{
    [SerializeField]
    private Vector2 cameraTarget;

    public float CameraLookAheadFactor = 3f;

    public Vector2 CameraTarget
    {
        get { return cameraTarget;}
        private set { cameraTarget = value;}
    }
    
    public override void Move(Vector2 move_vector)
    {
        base.Move(move_vector);
        cameraTarget = screenPosition + (move_vector * CameraLookAheadFactor);
    }
}