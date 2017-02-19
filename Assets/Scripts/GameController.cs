using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ControllerState {
    RUNNING,
    WIN,
    LOSE,
    FADE_IN,
}

public class GameController : MonoBehaviour {

	public PlayerEntity Player;
	public GameObject Target;
	public float MoveSpeed = 120.0f;
	public Camera WorldCamera;
	public List<Enemy> Enemies;
	public Vector2 PlayerPosition {
		get { return (Vector2)Player.OffsetPosition; }
	}
	public Sprite[] PointerSprites;
	public SpriteRenderer Pointer;
	private ControllerState state;
	private PlayerFollower playerFollower;
	public GameObject ProjectilePrefab;
	private Vector2 last_movement;
	// Use this for initialization
	void Start () {
		Time.timeScale = 1f;
		playerFollower = FindObjectOfType<PlayerFollower>();
		Debug.Log(playerFollower);
		state = ControllerState.RUNNING;
		last_movement = Vector2.right;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F5))
		{
			SceneManager.LoadSceneAsync("main");
		}
		if (Input.GetKeyDown(KeyCode.F6))
		{
			foreach (Enemy enemy in Enemies)
			{
				enemy.PathTo(Player.gameObject);
			}
		}
		if (Target != null)
		{
			RaycastHit2D pointer_location = Physics2D.Linecast(
				WorldCamera.transform.position, 
				(Vector2)Target.transform.position + new Vector2(16, 16),
				LayerMask.GetMask("UI"));
			if (pointer_location.collider == null)
			{
				Pointer.enabled = false;
			} else {
				if (pointer_location.collider.name == "Pointer_Collider_Top")
				{
					Pointer.sprite = PointerSprites[0];
				} else if (pointer_location.collider.name ==
					"Pointer_Collider_Left")
				{
					Pointer.sprite = PointerSprites[1];
				} else if (pointer_location.collider.name ==
					"Pointer_Collider_Bottom")
				{
					Pointer.sprite = PointerSprites[2];
				} else if (pointer_location.collider.name ==
					"Pointer_Collider_Right")
				{
					Pointer.sprite = PointerSprites[3];
				}
				Pointer.enabled = true;
				Pointer.transform.position = pointer_location.point;
			}
		}
	}

	void FixedUpdate () {
		DoMovement();
	}

	void DoMovement() {
		float x_movement = Input.GetAxis("Horizontal") * MoveSpeed;
			// fudge aspect ratio correction for movement
		float y_movement = Input.GetAxis("Vertical") * MoveSpeed; 
		Vector2 movement = new Vector2(x_movement, y_movement);
		Player.Move(movement);
		if (movement.magnitude > 0)
			last_movement = movement;
		if (Input.GetButtonDown("Fire1"))
		{
			Projectile new_projectile= Instantiate(ProjectilePrefab, (Vector3)PlayerPosition, Quaternion.identity).GetComponent<Projectile>();
			new_projectile.Initialize(last_movement);
		}
	}

	public void RegisterPlayer (GameObject player)
	{
		Player = player.GetComponent<PlayerEntity>();
		Player.Initialize();
		Debug.Log(Player);
		playerFollower.SetPlayer(Player);
	}

	public void RegisterTarget (GameObject target)
	{
		Target = target;
		Target.GetComponent<StaticEntity>().Initialize();
	}

	public void RegisterEnemy (GameObject enemyGameObject)
	{
		Enemy enemy = enemyGameObject.GetComponent<Enemy>();
		enemy.Initialize();
		Enemies.Add(enemy);
	}

	public void Lose()
	{
		if (state == ControllerState.RUNNING)
		{
			state = ControllerState.LOSE;
			Debug.Log("Womp womp");
			Time.timeScale = 0f;
			SceneManager.LoadSceneAsync("main");
		}
	}

	public void Win()
	{
		if (state == ControllerState.RUNNING)
		{
			state = ControllerState.WIN;
			Debug.Log("Huzzah!");
			Time.timeScale = 0f;
			SceneManager.LoadSceneAsync("main");
		}
	}
	// void OnDrawGizmos()
	// {
	// 	Gizmos.color = Color.red;
	// 	Vector3 vectorToTarget = (Target.transform.position - WorldCamera.transform.position);
	// 	Gizmos.DrawRay(WorldCamera.transform.position, vectorToTarget);
	// }
}
