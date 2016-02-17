using UnityEngine;
using System.Collections;

public class BuildGrid : MonoBehaviour {

    public float m_fCellSize = 2.0f;
    public int m_nRows = 8;
    public int m_nColumns = 8;

    // Debug.
    public bool m_bDebug = true;
    public Color m_DebugColor = Color.white;

    private static BaseGrid m_TerrainRepresentation;

    public static BaseGrid FindingGrid
    {
        get { return m_TerrainRepresentation as BaseGrid; }
    }

    void Awake() {
        m_TerrainRepresentation = new BaseGrid();
        m_TerrainRepresentation.Init(transform.position, m_nRows, m_nColumns, m_fCellSize);
    }


    void Start () {
	
	}
	

	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = m_DebugColor;
        if (m_bDebug)
        {
            BaseGrid.DebugDraw(transform.position, m_nRows, m_nColumns, m_fCellSize, Gizmos.color);
        }
    }
}
