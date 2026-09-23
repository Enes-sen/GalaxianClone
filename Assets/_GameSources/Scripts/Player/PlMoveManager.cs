using UnityEngine;

public class PlMoveManager : MonoBehaviour,IMove
{
    [SerializeField] private int maxMinX;
    [SerializeField]private PlayerController _pcon;
    private Rigidbody2D _rb;
    private int _Speed;

    private void Start()
    {
        _rb = _pcon.PLb();
        _Speed = _pcon.PLSo().Speed;
    }
    
    public void Move()
    {
        if (_rb == null) return;

        float moveInput = InputManager.Instance.GetXAxis();
        Vector2 dir = new(moveInput, 0f);

        Vector2 targetPos = _rb.position + dir * (_Speed * Time.fixedDeltaTime);
        targetPos.x = Mathf.Clamp(targetPos.x, -maxMinX, maxMinX);

        _rb.MovePosition(targetPos);
    }
}
