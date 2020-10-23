using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class IceBall : MonoBehaviour {

    public Tilemap iceTiles;
    public Tilemap terrainTiles;
    public List<Tilemap> waterTiles = new List<Tilemap>();
    public RuleTile iceTile;
    public Grid grid;
    public TileBase waterTop;

    // Start is called before the first frame update
    void Start() {
        iceTiles = GameObject.FindWithTag("Ice").GetComponent<Tilemap>();
        terrainTiles = GameObject.FindWithTag("Terrain").GetComponent<Tilemap>();
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Water");
        foreach (GameObject t in temp) {
            waterTiles.Add(t.GetComponent<Tilemap>());
        }
        grid = iceTiles.layoutGrid;
    }

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Terrain" || col.gameObject.tag == "Ice") {
            GenerateIce();
        }
    }

    private bool testCell(Vector3Int pos) {
        foreach(Tilemap w in waterTiles) {
            if (w.GetTile(pos) == waterTop) {
                return true;
            }
        }
        return false;
    }

    private void GenerateIce() {
        AudioManager.Instance.PlayIceShot();
        Vector3Int startPos = grid.WorldToCell(transform.position);
        for (int x = -1; x <= 1; x++) {
            for (int y = -1; y <= 1; y++) {
                Vector3Int newPos = new Vector3Int(Mathf.RoundToInt(startPos.x - x), Mathf.RoundToInt(startPos.y - y), 0);
                bool result = testCell(newPos);
                if (result) {
                    iceTiles.SetTile(newPos, iceTile);
                    iceTiles.RefreshTile(newPos);
                }
            }
        }
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Water") {
            GenerateIce();
        }
    }
}
