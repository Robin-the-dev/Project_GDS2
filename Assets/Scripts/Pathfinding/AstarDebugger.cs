using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AstarDebugger : MonoBehaviour {

    public static AstarDebugger Instance { get; private set; }

    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tile openTile, closedTile, pathTile, currentTile, startTile, endTile;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject debugTextPrefab;

    public AstarDebugText[] objs;

    private List<GameObject> debugObjects = new List<GameObject>();

    public void CreateTiles(HashSet<Node> openList, HashSet<Node> closedList, Vector3Int start, Vector3Int end, Stack<Vector3Int> path = null) {
        // reset debug
        tilemap.ClearAllTiles();
        objs = FindObjectsOfType<AstarDebugText>();
        foreach (AstarDebugText obj in objs) {
            Destroy(obj.gameObject);
        }
        // draw debug
        foreach (Node node in openList) {
            ColourTile(node.Position, openTile);
            AddInfo(node);
        }
        foreach (Node node in closedList) {
            ColourTile(node.Position, closedTile);
            AddInfo(node);
        }
        if (path != null) {
            foreach (Vector3Int pos in path) {
                if (pos == start || pos == end) continue;
                ColourTile(pos, pathTile);
            }
        }
        ColourTile(start, startTile);
        ColourTile(end, endTile);
    }

    private void AddInfo(Node node) {
        if (node.Parent != null) {
            AstarDebugText obj = Instantiate(debugTextPrefab, canvas.transform).GetComponent<AstarDebugText>();
            obj.transform.position = grid.GetCellCenterWorld(node.Position);
            Vector3 parentPos = grid.GetCellCenterWorld(node.Parent.Position);
            Vector3 dir = parentPos - obj.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            obj.arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            obj.SetScores(node.G, node.H, node.F);
        }
    }

    public void ColourTile(Vector3Int pos, Tile tile) {
        tilemap.SetTile(pos, tile);
        tilemap.RefreshTile(pos);
    }

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
    }
}
