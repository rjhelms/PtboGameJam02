using UnityEngine;
public class StaticEntity : MonoBehaviour {
	[SerializeField]
	private Vector2 screenPositionOffset;
    

    public void Initialize()
    {
        Vector2 worldPosition = (Vector2)transform.position;
		Vector2 screenPosition = new Vector2(Mathf.RoundToInt(worldPosition.x),
                                             Mathf.RoundToInt(worldPosition.y));
		transform.position = screenPosition + screenPositionOffset;
    }
}