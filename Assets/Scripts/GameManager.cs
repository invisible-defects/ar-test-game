using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct GameState 
{
    public string stage;
    public List<GameObject> obstacles;

    public GameState(string stage) 
    {
        this.stage = stage;
        obstacles = new List<GameObject>();
    }
}

public class GameManager : MonoBehaviour
{
    public GameObject lockButton;
    public GameObject startButton;
    private GameState state = new GameState("notStarted");
    private float obstacleSpawnDelay = 2f; // sec
    private float obstacleSpeed = 0.01f; // m/sec
    private float lastObstacledSpawned = 0;

    public string getState() 
    {
        return state.stage;
    }

    public void startGame() 
    {
        if(state.stage == "notStarted") 
        {
            state.stage = "started";
        }
    }

    void Update() 
    {   
        if(state.stage == "started") 
        {
            // Move spawned obstacles to the player
            moveObstacles();
            // Check if the player hit any (or moved away from the playfield)
            checkCollision();
            // Delete obstacles behind the player
            deleteObstacles();
            // Spawn new obstacles
            spawnObstacles();
        }

        // Update UI from the gamestate
        updateFromState();
    }

    private void updateFromState()
    {
        if(state.stage == "started") 
        {
            lockButton.SetActive(false);
            startButton.SetActive(false);
        }
        else
        {
            lockButton.SetActive(true);
            lockButton.SetActive(true);
        }
    }

    private void checkCollision()
    {
        throw new NotImplementedException();
    }

    private void generateObstacle()
    {
        // Obstacle starting cell
        int obstacleStart = Mathf.RoundToInt(UnityEngine.Random.Range(0, 4));
        // Length of obstacle (amount of cells)
        int obstaclelength = Mathf.Min(Mathf.RoundToInt(UnityEngine.Random.Range(1, 4)), 5 - obstacleStart);

        // TODO: instantiate prefab and add to state.obstacles
    }

    private void spawnObstacles()
    {
        lastObstacledSpawned += Time.deltaTime;
        if(lastObstacledSpawned > obstacleSpawnDelay) 
        {
            lastObstacledSpawned = 0;
            generateObstacle();
        }
    }

    private void deleteObstacles()
    {
        foreach(GameObject obstacle in state.obstacles) {
            if(obstacle.transform.position.z < -1) 
            {
                Destroy(obstacle);
            }
        }
    }

    private void moveObstacles()
    {
        foreach(GameObject obstacle in state.obstacles) 
        {
            var currentPosition = obstacle.transform.position;
            currentPosition.z -= Time.deltaTime * obstacleSpeed;
            obstacle.transform.SetPositionAndRotation(currentPosition, obstacle.transform.rotation);
        }
    }
}
