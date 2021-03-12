using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Road _road = null;
    [SerializeField] private bool _isRotate = false;
    [SerializeField] private bool _isMove = false;

    private void Update()
    {
        Transform block = _road.LastLetterBlock;

        if (_isRotate)
            transform.rotation = Quaternion.Lerp(transform.rotation, block.rotation, 0.05f);

        if (_isMove)
        {
            Vector3 position = new Vector3(block.position.x, transform.position.y, block.position.z);
            transform.position = Vector3.Lerp(transform.position, position, 0.05f);
        }
    }
}
