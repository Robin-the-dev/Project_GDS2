using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MakiPathing : MonoBehaviour {


    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private PixieCharacter maki;

    private Vector3Int startPos;
    public Vector3Int endPos;

    private HashSet<Node> openList;
    private HashSet<Node> closedList;
    private Node current;

    private Stack<Vector3Int> path;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    public void updateEndPos(Vector3 pos) {
        Vector3Int tempPos = grid.WorldToCell(pos);
        if (tilemap.GetTile(tempPos) == null) {
            endPos = tempPos;
        }
    }

    void Start() {
        Algorithm();
        endPos = startPos;
    }

    public Vector3 ToWorldSpace(Vector3Int convert) {
        return grid.GetCellCenterWorld(convert);
    }

    public Stack<Vector3Int> Algorithm(int maxCount = 100) {
        Initialise();
        if (endPos == startPos) return null;
        int count = 0;
        while (openList.Count > 0 && path == null && count < maxCount) {
            ExamineNeighbors(FindNeighbors(current), current);
            UpdateCurrentTile(ref current);

            path = GeneratePath(current);
            count++;
        }
        // AstarDebugger.Instance.CreateTiles(openList, closedList, startPos, endPos, path);
        return path;
    }

    private void Initialise() {
        startPos = grid.WorldToCell(maki.transform.position);
        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();
        path = null;
        current = GetNode(startPos);
        openList.Add(current);
    }

    private Node GetNode(Vector3Int position) {
        if (allNodes.ContainsKey(position)) {
            return allNodes[position];
        } else {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }

    private List<Node> FindNeighbors(Node parent) {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                if (y == 0 && x == 0) continue;
                Vector3Int neighborPos = new Vector3Int(parent.Position.x - x, parent.Position.y - y, parent.Position.z);
                Node neighbor = GetNode(neighborPos);
                neighbors.Add(neighbor);
            }
        }
        return neighbors;
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current) {
        foreach (Node n in neighbors) {
            int gScore = DetermineGScore(n, current);
            if (openList.Contains(n)) {
                if (current.G + gScore < n.G) {
                    CalcValues(current, n, gScore);
                }
            } else if (!closedList.Contains(n)) {
                if (ExamineNeighbor(n, current)) {
                    CalcValues(current, n, gScore);
                    openList.Add(n);
                }
            }
        }
    }

    private bool ExamineNeighbor(Node neighbor, Node current) {

        // set up filter
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = false;
        filter.SetLayerMask(~LayerMask.GetMask("Player", "AntiWallGrab", "SupportPlayer", "Moveable"));
        // get positions
        Vector2 currentPos = grid.GetCellCenterWorld(current.Position);
        Vector2 nodePos = grid.GetCellCenterWorld(neighbor.Position);
        List<RaycastHit2D> results = new List<RaycastHit2D>();
        // check for hits
        Physics2D.Raycast(currentPos, (nodePos - currentPos), filter, results, Vector3.Distance(currentPos, nodePos));
        return results.Count == 0;
    }

    private void CalcValues(Node parent, Node neighbor, int cost) {
        neighbor.Parent = parent;
        neighbor.G = parent.G + cost;
        neighbor.H = (Mathf.Abs(neighbor.Position.x - endPos.x) + Mathf.Abs(neighbor.Position.y - endPos.y)) * 10;
        neighbor.F = neighbor.G + neighbor.H;
    }

    private int DetermineGScore(Node neighbor, Node current) {
        int x = current.Position.x - neighbor.Position.x;
        int y = current.Position.y - neighbor.Position.y;

        if (Mathf.Abs(x-y) % 2 == 1) {
           return 10;
        } else {
           return 14;
        }
    }

    private void UpdateCurrentTile(ref Node current) {
        openList.Remove(current);
        closedList.Add(current);

        if (openList.Count > 0) {
            current = openList.OrderBy(x => x.F).First();
        }
    }

    private Stack<Vector3Int> GeneratePath(Node current) {
        if(current.Position == endPos) {
            Stack<Vector3Int> finalPath = new Stack<Vector3Int>();
            while (current.Position != startPos) {
                finalPath.Push(current.Position);
                current = current.Parent;
            }
            return finalPath;
        }
        return null;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Q)) {
            Algorithm();
        }
    }
}
