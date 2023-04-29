using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour {
    public bool is_active = true;
    public Player current_player;
    public float fog_max_depth = 0f;
    public float fog_default_depth = 10f;
    public int num_vertices = 16;
    public float vertex_dist = 0.5f;

    private Mesh fog_mesh;
    private Vector3[] verts;
    private int[] tris;
    private bool initialized = false;

    public void Start() {
        CreateFogMesh();
    }

    public void CreateFogMesh() {
        if(!is_active)
            return;

        fog_mesh = new Mesh();
        verts = new Vector3[num_vertices * num_vertices];
        for(int i = 0; i < num_vertices; i++) {
            for(int j = 0; j < num_vertices; j++) {
                verts[i * num_vertices + j] = new Vector3(i * vertex_dist,
                                                          fog_default_depth,
                                                          j * vertex_dist);
            }
        }

        tris = new int[(num_vertices - 1) * (num_vertices - 1) * 6];
        var tri_base = 0;
        var vert_base = 0;
        for(int i = 0; i < num_vertices - 1; i++) {
            for(int j = 0; j < num_vertices - 1; j++) {
                tris[tri_base + 0] = vert_base + 0;
                tris[tri_base + 1] = vert_base + 1;
                tris[tri_base + 2] = vert_base + num_vertices;
                tris[tri_base + 3] = vert_base + 1;
                tris[tri_base + 4] = vert_base + num_vertices + 1;
                tris[tri_base + 5] = vert_base + num_vertices;

                tri_base += 6;
                vert_base += 1;
            }
            vert_base += 1;
        }

        fog_mesh.vertices = verts;
        fog_mesh.triangles = tris;
        fog_mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = fog_mesh;
    }

    public void UpdateFogMesh() {
        if(!is_active) 
            return;
        
        fog_mesh.Clear();

        for(int i = 0; i < verts.Length; i++) {
            var vert_xz = new Vector2(verts[i].x, verts[i].z);
            var is_revealed = false;
            foreach(var unit in current_player.units) {
                var unit_xz = new Vector2(unit.transform.position.x, 
                                          unit.transform.position.z);
                if(Vector2.Distance(vert_xz, unit_xz) < unit.reveal_radius) {
                    verts[i].y = fog_max_depth;
                    is_revealed = true;
                    break;
                }
            }

            if(is_revealed)
                continue;

            foreach(var building in current_player.buildings) {
                var building_xz = new Vector2(building.transform.position.x,
                                              building.transform.position.z);
                if(Vector2.Distance(vert_xz, building_xz) < building.reveal_radius) {
                    verts[i].y = fog_max_depth;
                    is_revealed = true;
                    break;
                }
            }

            if(is_revealed)
                continue;

            verts[i].y = fog_default_depth;
        }

        fog_mesh.vertices = verts;
        fog_mesh.triangles = tris;
        fog_mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = fog_mesh;
    }

    public void Update() {
        if(!is_active) 
            return;

        if(!initialized) {
            UpdateFogMesh();
            initialized = true;
            Debug.Log("Fog Initialized...");
        }
    }
}
