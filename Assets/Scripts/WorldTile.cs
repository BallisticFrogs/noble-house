using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WorldTile : UnityEngine.Tilemaps.Tile
{

    public TileType tileType;
    public GameObject gameObj;

    public AudioSource fx;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayFx(AudioClip clip) {
        fx.PlayOneShot(clip);
    }
}
