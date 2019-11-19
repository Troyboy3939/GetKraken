using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{
    [SerializeField] private float m_fGridWidth = 21;
    [SerializeField] private float m_fGridHeight = 16;

    private PositionManager instance;

    [SerializeField] private List<Vector2> m_HolePositions = new List<Vector2>()
    {
        new Vector2(2, 2),
        new Vector2(2, 13),
        new Vector2(2, 7),
        new Vector2(6, 4),
        new Vector2(6, 7),
        new Vector2(6, 10),
        new Vector2(10, 2),
        new Vector2(10, 13),
        new Vector2(14, 4),
        new Vector2(14, 7),
        new Vector2(14, 10),
        new Vector2(18, 2),
        new Vector2(18, 7),
        new Vector2(18, 13)
    };

    [SerializeField] private List<Vector2> m_BufferArea = new List<Vector2>()
    {
        new Vector2(7, 4),
        new Vector2(7, 5),
        new Vector2(7, 6),
        new Vector2(7, 7),
        new Vector2(7, 8),
        new Vector2(7, 9),
        new Vector2(7, 10),
        new Vector2(7, 11),
        new Vector2(8, 4),
        new Vector2(8, 5),
        new Vector2(8, 6),
        new Vector2(8, 7),
        new Vector2(8, 8),
        new Vector2(8, 9),
        new Vector2(8, 10),
        new Vector2(8, 11),
        new Vector2(9, 4),
        new Vector2(9, 5),
        new Vector2(9, 6),
        new Vector2(9, 7),
        new Vector2(9, 8),
        new Vector2(9, 9),
        new Vector2(9, 10),
        new Vector2(9, 11),
        new Vector2(10, 4),
        new Vector2(10, 5),
        new Vector2(10, 6),
        new Vector2(10, 7),
        new Vector2(10, 8),
        new Vector2(10, 9),
        new Vector2(10, 10),
        new Vector2(10, 11),
        new Vector2(11, 4),
        new Vector2(11, 5),
        new Vector2(11, 6),
        new Vector2(11, 7),
        new Vector2(11, 8),
        new Vector2(11, 9),
        new Vector2(11, 10),
        new Vector2(11, 11),
        new Vector2(12, 4),
        new Vector2(12, 5),
        new Vector2(12, 6),
        new Vector2(12, 7),
        new Vector2(12, 8),
        new Vector2(12, 9),
        new Vector2(12, 10),
        new Vector2(12, 11),
        new Vector2(13, 4),
        new Vector2(13, 5),
        new Vector2(13, 6),
        new Vector2(13, 7),
        new Vector2(13, 8),
        new Vector2(13, 9),
        new Vector2(13, 10),
        new Vector2(13, 11)
    };

    private List<Vector2> m_BufferAreaEdges = new List<Vector2>(100);

    public List<Vector2> GetHolePositions()
    {
        return m_HolePositions;
    }

    public List<Vector2> GetBufferArea()
    {
        return m_BufferArea;
    }

    public List<Vector2> GetBufferAreaEdges()
    {
        return m_BufferAreaEdges;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }

        CreateBufferEdges();
    }

    private void CreateBufferEdges()
    {
        for (int i = 0; i < m_fGridHeight; i++)
        {
            m_BufferAreaEdges.Add(new Vector2(0, i));
        }

        for (int i = 0; i < m_fGridHeight; i++)
        {
            m_BufferAreaEdges.Add(new Vector2(20, i));
        }

        for (int i = 1; i < m_fGridWidth - 1; i++)
        {
            m_BufferAreaEdges.Add(new Vector2(i, 0));
        }

        for (int i = 1; i < m_fGridWidth - 1; i++)
        {
            m_BufferAreaEdges.Add(new Vector2(i, 15));
        }
    }
}
