using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Road _road = null;

    private void Update()
    {
        Transform block = _road.LastLetterBlock;
        transform.rotation = Quaternion.Lerp(transform.rotation, block.localRotation, 0.05f);
        Vector3 position = new Vector3(block.position.x, transform.position.y, block.position.z);
        transform.position = Vector3.Lerp(transform.position, position, 0.05f);
    }
}
