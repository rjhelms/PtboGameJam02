using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEntity : MonoBehaviour {

	[SerializeField]
	protected Vector2 screenPosition;
	[SerializeField]
	protected Vector2 screenPositionOffset;
	protected GameObject EntitySprite;

	public Vector2 ScreenPosition
	{
		get { return screenPosition; }
	}
	public GameObject ScreenPrefab;
	// Use this for initialization
	protected virtual void Start () {
		Initialize();
	}
	
	// Update is called once per frame
	protected virtual void Update () {
		screenPosition.x = Mathf.RoundToInt(transform.position.x);
		screenPosition.y = Mathf.RoundToInt(transform.position.y);
		EntitySprite.transform.position = screenPosition;
	}

	public virtual void Initialize()
	{
		if (!EntitySprite)
		{
			screenPosition = transform.position;
			EntitySprite = Instantiate(ScreenPrefab, screenPosition,
									   Quaternion.identity);
		}
	}
}
