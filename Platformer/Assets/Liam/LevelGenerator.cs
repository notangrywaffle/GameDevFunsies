using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class LevelGenerator : MonoBehaviour {


    /*
        Rules
        ======

        Can't include the same chunk twice in a row
        Player distance increases the % of high danger chunks
        Chunks have a higher % of being paired with chunks from the same zone



        Each chunk has a % drop rate
        Each chunk has a danger rating -100 to +100
        Each chunk has a size
        Each chunk has a start and finish height
        Each chunk has a minimum level (story)
        Each chunk has a minimum distance (Endless)
        Each chunk has a Zone



        Note: Checkpoint rerolls level?
    */

    public bool OnlyLoadDefault = true;


    public GameObject PrefabGroundGrasslands;
    public GameObject PrefabCheckpoint;
    public GameObject PrefabRampUp;
    public GameObject PrefabRampDown;
    public GameObject PrefabInstantDeath;
    public GameObject PrefabScarySpider;


    List<ChunkInfo> chunkTemplates;

    public Vector2 ContinuePoint { get { return continuePoint; } }
    private Vector2 continuePoint;



    public int levelEndsAt;
    private int lastEndHeight;
    private GameObject LevelChunkContainer;


    public bool needNewChunk = true;

	// Use this for initialization
	void Start () {
        chunkTemplates = new List<ChunkInfo>();

        TextAsset asset = Resources.Load<TextAsset>("ChunkList");
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(asset.text);
        XmlNodeList nodes = doc.DocumentElement.SelectNodes("/Chunks");

        foreach (XmlNode node in nodes)
        {

            foreach (XmlNode chunk in node.ChildNodes)
                chunkTemplates.Add(new ChunkInfo(chunk));//chunk));
        }

        Debug.Log("Chunks loaded: " + chunkTemplates.Count);

        LevelChunkContainer = new GameObject();
        LevelChunkContainer.transform.position = Vector3.zero;
        LevelChunkContainer.name = "LevelChunkContainer";
	}
	
	// Update is called once per frame
	void Update () {

        if (needNewChunk)
        {
            //Choose a chunk;
            //Instantiate Container and elements
            //Place container


            ChunkInfo newChunkInfo;
            if (OnlyLoadDefault)
            {
                newChunkInfo = chunkTemplates[0];
            }
            else
            {
                newChunkInfo = chunkTemplates[Random.Range(0, chunkTemplates.Count)];
            }
            GameObject newChunkContainer = new GameObject();
            newChunkContainer.transform.position = Vector3.zero;

            for (int y = 0; y < newChunkInfo.Map.GetLength(1); y++)
            {
                for (int x = 0; x < newChunkInfo.Map.GetLength(0); x++)
                {
                    GameObject toInstantiate = null;
                    Vector2 placementAdjustment = Vector2.zero;
                    Vector3 rotation = Vector3.zero;

                    switch(newChunkInfo.Map[x, y])
                    {
                        case ChunkInfo.charStart:
                            continuePoint = new Vector2(levelEndsAt + 1f, lastEndHeight + 1f);
                            toInstantiate = PrefabGroundGrasslands;
                            break;
                        case ChunkInfo.charEnd:
                        case ChunkInfo.charGround:
                            toInstantiate = PrefabGroundGrasslands;
                            break;
                        case ChunkInfo.charCheckpoint:
                            toInstantiate = PrefabCheckpoint;
                            break;
                        case ChunkInfo.charRampDown:
                            toInstantiate = PrefabRampDown;
                            placementAdjustment = new Vector2(-0.5f, -0.5f);
                            rotation.z = -45f;
                            break;
                        case ChunkInfo.charRampUp:
                            toInstantiate = PrefabRampUp;
                            placementAdjustment = new Vector2(0.5f, -0.5f);
                            rotation.z = 45f;
                            break;
                        case ChunkInfo.charInstantDeath:
                            toInstantiate = PrefabInstantDeath;
                            break;
                        case ChunkInfo.charEnemyScarySpider:
                            toInstantiate = PrefabScarySpider;
                            break;
                        default:
                            toInstantiate = null;
                            break;
                    }

                    if (toInstantiate != null)
                    {
                        int adjustedY = 0;
                        if (y > newChunkInfo.StartIndex.y)
                            adjustedY = (int)newChunkInfo.StartIndex.y - y;
                        if (y < newChunkInfo.StartIndex.y)
                            adjustedY = (int)newChunkInfo.StartIndex.y - y;

                        GameObject.Instantiate(toInstantiate, new Vector3(x + placementAdjustment.x, adjustedY + placementAdjustment.y, 0), Quaternion.Euler(rotation), newChunkContainer.transform);
                    }
                }
            }

            newChunkContainer.transform.position = new Vector3(levelEndsAt, lastEndHeight, 0);
            newChunkContainer.transform.SetParent(LevelChunkContainer.transform);
            newChunkContainer.name = "ChunkContainer";

            levelEndsAt += newChunkInfo.Width;
            lastEndHeight += (int)newChunkInfo.StartIndex.y - (int)newChunkInfo.EndIndex.y;

            needNewChunk = false;
        }

        //Clean up old chunks
	}
}
