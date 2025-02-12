using Unity.Netcode;
using UnityEngine;

public class GameVisualManager : MonoBehaviour
{
    public const float GRID_SIZE = 3.1f;
    [SerializeField] private Transform cross;
    [SerializeField] private Transform circle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.onClickedonGridPosition += GameManager_onClickedPosition;
    }


    private void GameManager_onClickedPosition(object sender, GameManager.onCickedOnGridPositionEventArgs e)
    {
        Transform objectId = cross;
        if (e.PlayerType == GameManager.playerType.Cross)
        {
            objectId = cross;
        }
        else
        {
            objectId = circle;
        }
        Transform crossPos = Instantiate(objectId, GetGridWorldPosition(e.x, e.y), Quaternion.identity);
        crossPos.GetComponent<NetworkObject>().Spawn();
    }

    private Vector2 GetGridWorldPosition(int x, int y)
    {
        return new Vector2(-GRID_SIZE + GRID_SIZE * x, -GRID_SIZE + GRID_SIZE * y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
