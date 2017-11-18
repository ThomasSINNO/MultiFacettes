using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucioleAnimation : MonoBehaviour {

    Transform playerTransform;
    bool isPlayerMoving;

    [SerializeField]
    float offsetY = 0;

    float m_centerPositionY;
    float m_degrees;

    float m_speed;
    float m_amplitude;

    [SerializeField]
    float m_speed_idle = 1.0f;
    [SerializeField]
    float m_speed_move = 1.0f;

    [SerializeField]
    float m_amplitude_idle = 0.05f;
    [SerializeField]
    float m_amplitude_move = 0.2f;

    [SerializeField]
    float m_period = 1.0f;

    void Update()
    {

        playerTransform = transform.parent;
        isPlayerMoving = GetComponentInParent<Player>().isMoving;
        if (isPlayerMoving)
        {
            m_amplitude = m_amplitude_move;
            m_speed = m_speed_move;
        }
        else
        {
            m_amplitude = m_amplitude_idle;
            m_speed = m_speed_idle;
        }
        m_centerPositionY = playerTransform.position.y + offsetY;

        float deltaTime = Time.deltaTime;

        // Update degrees
        float degreesPerSecond = 360.0f / m_period;
        m_degrees = Mathf.Repeat(m_degrees + (deltaTime * degreesPerSecond), 360.0f);
        float radians = m_degrees * Mathf.Deg2Rad;

        // Offset by sin wave
        Vector3 offset = new Vector3(0.0f, m_amplitude * Mathf.Sin(radians), 0.0f);
        transform.position = new Vector3(transform.position.x,m_centerPositionY,0.0f) + offset;
    }
}
