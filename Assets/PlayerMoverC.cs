using UnityEngine;

public class PlayerMoverC : MonoBehaviour
{
    [SerializeField]
    private float _defaultSpeed = 5.0f;
    [SerializeField]
    private float _ratio = 0.1f;
    [SerializeField]
    private float _addHorizontalMovePower = 2.5f;
    [SerializeField]
    private AnimationCurve _moveStart;
    [SerializeField]
    private Rigidbody2D _rigidbody;
    private void Update()
    {
        float addX = Input.GetAxis("Horizontal") * _addHorizontalMovePower * Time.deltaTime;
        float updateX = 0;
        if (addX < 0.002f)
        {
            updateX = Mathf.Clamp(_rigidbody.velocity.x + 0.2f * (_rigidbody.velocity.x < 0 ? 1 : -1), -_defaultSpeed, _defaultSpeed);
            Debug.Log(updateX);
        }
        else
        {
            updateX = Mathf.Clamp(_rigidbody.velocity.x + addX, -_defaultSpeed, _defaultSpeed);

            float threshold = _defaultSpeed * _ratio;
            float t = threshold / updateX;
            if (t < 1)
            {
                updateX = Mathf.Clamp(_rigidbody.velocity.x + _moveStart.Evaluate(t) * addX, -_defaultSpeed, _defaultSpeed);
            }
        }

        _rigidbody.velocity = new Vector2(updateX, _rigidbody.velocity.y);
    }
}