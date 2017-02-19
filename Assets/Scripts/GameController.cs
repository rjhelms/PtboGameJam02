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
	private ControllerState state;
	private PlayerFollower playerFollower;
	// Use this for initialization
	void Start () {
		Time.timeScale = 1f;
		playerFollower = FindObjectOfType<PlayerFollower>();
		Debug.Log(playerFollower);
		state = ControllerState.RUNNING;
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
