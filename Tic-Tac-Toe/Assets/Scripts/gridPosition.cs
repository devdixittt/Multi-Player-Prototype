using UnityEngine;

public class gridPosition : MonoBehaviour
{
    [SerializeField]private int gridPosX;
    [SerializeField]private int gridPosY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnMouseDown()
    {
        GameManager.Instance.positionClickedRpc(gridPosX,gridPosY, GameManager.Instance.GetLocalPlayerType());
    }
}
