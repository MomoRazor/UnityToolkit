using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkBuildMode : MonoBehaviour
{
    public AudioClip build;

    public GameObject[] markers;
    GameObject[] buildModeMenu;
    bool buildModeBuffer = false;
    int currentBuilding = 1;
    List<GameObject> boxes = new List<GameObject>();
    MarkMapGenerator mmg;
    bool up = false;
    bool down = false;
    bool left = false;
    bool right = false;
    int buildingsIndexoffset = 2;
    public GameObject[] buildingPrefs;
    public GameObject wall;

    public Sprite[] bImgOn, bImgOff;
    Image[] bImg = new Image[5];
    Text[] bText = new Text[5];

    string[] objectTitles = new string[5];
    string[] objectText = new string[5];
    string[] objectCost = new string[5];

    Text infoTitle, infoCost, infoText;

    MarkEconomy me;


    // Start is called before the first frame update
    void Start()
    {
        //populate
        infoTitle = GameObject.FindGameObjectWithTag("iTitle").GetComponent<Text>();
        infoCost = GameObject.FindGameObjectWithTag("iCost").GetComponent<Text>();
        infoText = GameObject.FindGameObjectWithTag("iText").GetComponent<Text>();

        bImg[0] = GameObject.FindGameObjectWithTag("sb").GetComponent<Image>();
        bImg[1] = GameObject.FindGameObjectWithTag("tu").GetComponent<Image>();
        bImg[2] = GameObject.FindGameObjectWithTag("pi").GetComponent<Image>();
        bImg[3] = GameObject.FindGameObjectWithTag("ga").GetComponent<Image>();
        bImg[4] = GameObject.FindGameObjectWithTag("at").GetComponent<Image>();

        bText[0] = GameObject.FindGameObjectWithTag("sbt").GetComponent<Text>();
        bText[1] = GameObject.FindGameObjectWithTag("tut").GetComponent<Text>();
        bText[2] = GameObject.FindGameObjectWithTag("pitt").GetComponent<Text>();
        bText[3] = GameObject.FindGameObjectWithTag("gat").GetComponent<Text>();
        bText[4] = GameObject.FindGameObjectWithTag("att").GetComponent<Text>();

        objectTitles[0] = "Bits And Pieces";
        objectText[0] = "No man, er.. demon, is an Island! So make sure you make use of our Bits And Pieces tool, to turn that nasty lava into walk-able and build-able terrain. 100% local and organic, made of pesky Human and Angel bits!";
        objectCost[0] = bText[0].text + " Souls";

        objectTitles[1] = "Sloth Turret";
        objectText[1] = "Enjoy the luxuries of a lucrative life! These turrets will ensure you get a stream of income without beheading a single human! Just make sure they hit something because each arrow costs 0.15 of a soul!";
        objectCost[1] = bText[1].text + " Souls";

        objectTitles[2] = "5-Star hole-tel";
        objectText[2] = "Home sweet home. This is the building that makes all that hard work worth it. Here, souls are stored for use by you or your buildings. We also offer your guest souls refreshing dragon pee and hourly flayings.";
        objectCost[2] = bText[2].text + " Souls";

        objectTitles[3] = "Idiot Dispenser";
        objectText[3] = "No one likes having an empty torture chamber. Luckily for us, the world is full of stupid people that fall for the old 'We won't hurt you' routine. The Idiot Dispenser uses 0.2 of a soul for each dumb barnacle that finds it's way to your domain. Enjoy!";
        objectCost[3] = bText[3].text + " Souls";

        objectTitles[4] = "Baiter-inator";
        objectText[4] = "The Baiter-inator is the new, cutting edge technology in luring humans to their doom. At the price of 0.1 souls per second, this baby will make sure that every human in a 10 block radius will come running to it with no care to you, or any delicious bodily harm.";
        objectCost[4] = bText[4].text + " Souls";

        me = GameObject.FindGameObjectWithTag("eco").GetComponent<MarkEconomy>();

        //others
        mmg = GameObject.FindGameObjectWithTag("MapGen").GetComponent<MarkMapGenerator>();
        buildModeBuffer = Globals.isBuildMode;
        buildModeMenu = GameObject.FindGameObjectsWithTag("BuildModeMenu");
        for (int i = 0; i < buildModeMenu.Length; i++)
        {
            buildModeMenu[i].SetActive(buildModeBuffer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Toggle Build Mode
        if (Input.GetKeyDown(KeyCode.B)) {
            Globals.isBuildMode = !Globals.isBuildMode;
        }

        //Enable Menus
        if (buildModeBuffer != Globals.isBuildMode) {
            buildModeBuffer = Globals.isBuildMode;

            for (int i = 0; i < buildModeMenu.Length; i++)
            {
                buildModeMenu[i].SetActive(buildModeBuffer);
            }
        }

        //Work in this if function building stuff
        if (buildModeBuffer)
        {
            infoTitle.text = objectTitles[currentBuilding - 1];
            infoText.text = objectText[currentBuilding - 1];
            infoCost.text = objectCost[currentBuilding - 1];
            //Colors
            for (int i = 0; i < bText.Length; i++)
            {
                if (me.amount >= int.Parse(bText[i].text))
                {
                    bText[i].color = new Color(55, 104, 155);
                    bImg[i].sprite = bImgOn[i];
                }
                else
                {
                    bText[i].color = new Color(139, 10, 10);
                    bImg[i].sprite = bImgOff[i];
                }
            }

            if (Input.GetKey(KeyCode.RightArrow)) 
            {
                up = false;
                down = false;
                left = false;
                right = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                up = false;
                down = false;
                left = true;
                right = false;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                up = true;
                down = false;
                left = false;
                right = false;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                up = false;
                down = true;
                left = false;
                right = false;
            }

            //Inputs
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentBuilding = 1;
            }
            else
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                currentBuilding = 2;
            }
            else
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                currentBuilding = 3;
            }
            else
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                currentBuilding = 4;
            }
            else
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                currentBuilding = 5;
            }

            //Spawn Boxes
            int tempX, tempY;
            if (transform.position.x % 1 > 0.5f)
            {
                tempX = Mathf.CeilToInt(transform.position.x);
            }
            else {
                tempX = Mathf.FloorToInt(transform.position.x);
            }

            tempY = Mathf.FloorToInt(transform.position.y);

            Vector2Int pl = new Vector2Int(tempX, tempY);
            Vector2Int curr = new Vector2Int(tempX, tempY);

            bool canBePlaced = false;

            if (currentBuilding == 1) //Block
            {
                ClearBoxes();
                if (up) 
                {
                    curr += Vector2Int.up;
                    canBePlaced = CheckBlock(curr, 0);
                }
                else if (down)
                {
                    curr += Vector2Int.down;
                    canBePlaced = CheckBlock(curr, 0);
                }
                else if (left)
                {
                    curr += Vector2Int.left;
                    canBePlaced = CheckBlock(curr, 0);
                }
                else if (right)
                {
                    curr += Vector2Int.right;
                    canBePlaced = CheckBlock(curr, 0);
                }

                if (curr.x > mmg.mapSize.x - mmg.borderoffset) 
                {
                    canBePlaced = false;
                }
                if (curr.x < mmg.borderoffset)
                {
                    canBePlaced = false;
                }
                if (curr.y > mmg.mapSize.y - mmg.borderoffset)
                {
                    canBePlaced = false;
                }
                if (curr.y < mmg.borderoffset)
                {
                    canBePlaced = false;
                }

                if (canBePlaced)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && me.amount >= int.Parse(bText[0].text))
                    {
                        GetComponent<AudioSource>().PlayOneShot(build, 0.3f);
                        me.Buy(int.Parse(bText[0].text));
                        mmg.map[curr.x][curr.y] = 1;

                        //remove current
                        mmg.spawnedMap[curr.x][curr.y].SetActive(false);

                        //Add New
                        float val = -5 + (mmg.multiplier * (((float)curr.y * (float)mmg.map[curr.y].Length) + (float)curr.x));
                        mmg.spawnedMap[curr.x][curr.y] = Instantiate(buildingPrefs[currentBuilding - 1], new Vector3(curr.x, curr.y, val), Quaternion.identity);
                        mmg.spawnedGround.Add(mmg.spawnedMap[curr.x][curr.y]);

                        //Add Collider
                        //Debug.Log(mmg.spawnedMap[curr.x + 1][curr.y].tag);
                        //Debug.Log(mmg.spawnedMap[curr.x - 1][curr.y].tag);
                        //Debug.Log(mmg.spawnedMap[curr.x][curr.y + 1].tag);
                        //Debug.Log(mmg.spawnedMap[curr.x][curr.y - 1].tag);

                        if (mmg.spawnedMap[curr.x + 1][curr.y].tag == "tl") 
                        {
                            mmg.spawnedColliders.Add(Instantiate(wall, new Vector3((float)curr.x + 1, (float)curr.y, 0), Quaternion.identity));
                            mmg.spawnedMap[curr.x + 1][curr.y] = mmg.spawnedColliders[mmg.spawnedColliders.Count - 1];
                        }

                        if (mmg.spawnedMap[curr.x - 1][curr.y].tag == "tl")
                        {
                            mmg.spawnedColliders.Add(Instantiate(wall, new Vector3((float)curr.x - 1, (float)curr.y, 0), Quaternion.identity));
                            mmg.spawnedMap[curr.x - 1][curr.y] = mmg.spawnedColliders[mmg.spawnedColliders.Count - 1];
                            // Debug.Log("Working");
                        }

                        if (mmg.spawnedMap[curr.x][curr.y + 1].tag == "tl")
                        {
                            mmg.spawnedColliders.Add(Instantiate(wall, new Vector3((float)curr.x, (float)curr.y + 1, 0), Quaternion.identity));
                            mmg.spawnedMap[curr.x][curr.y + 1] = mmg.spawnedColliders[mmg.spawnedColliders.Count - 1];
                        }

                        if (mmg.spawnedMap[curr.x][curr.y - 1].tag == "tl")
                        {
                            mmg.spawnedColliders.Add(Instantiate(wall, new Vector3((float)curr.x, (float)curr.y - 1, 0), Quaternion.identity));
                            mmg.spawnedMap[curr.x][curr.y - 1] = mmg.spawnedColliders[mmg.spawnedColliders.Count - 1];
                        }

                        for (int i = 0; i < mmg.spawnedBuildings.Count; i++)
                        {
                            if (mmg.spawnedBuildings[i].tag == "Attractor")
                            {
                                mmg.spawnedBuildings[i].GetComponent<MarkAttractor>().RegenMPF();
                            }
                        }

                        //Globals.isBuildMode = false;
                    }
                }
            }
            else
            if (currentBuilding == 2) //Turret
            {
                ClearBoxes();
                if (up)
                {
                    curr += Vector2Int.up;
                    canBePlaced = CheckBlock(curr);
                }
                else if (down)
                {
                    curr += Vector2Int.down;
                    canBePlaced = CheckBlock(curr);
                }
                else if (left)
                {
                    curr += Vector2Int.left;
                    canBePlaced = CheckBlock(curr);
                }
                else if (right)
                {
                    curr += Vector2Int.right;
                    canBePlaced = CheckBlock(curr);
                }

                if (canBePlaced)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && me.amount >= int.Parse(bText[1].text))
                    {
                        GetComponent<AudioSource>().PlayOneShot(build, 0.3f);
                        me.Buy(int.Parse(bText[1].text));
                        mmg.spawnedBuildings.Add(Instantiate(buildingPrefs[currentBuilding - 1], new Vector3(curr.x, curr.y, buildingPrefs[currentBuilding - 1].transform.position.z), Quaternion.identity));
                        mmg.map[curr.x][curr.y] = mmg.spawnedBuildings.Count - 1 + buildingsIndexoffset;
                        for (int i = 0; i < mmg.spawnedBuildings.Count; i++)
                        {
                            if (mmg.spawnedBuildings[i].tag == "Attractor")
                            {
                                mmg.spawnedBuildings[i].GetComponent<MarkAttractor>().RegenMPF();
                            }
                        }
                    }
                }
            }
            else
            if (currentBuilding == 3) //Pit
            {
                ClearBoxes();
                Vector2Int[] sp = new Vector2Int[4];
                if (up)
                {
                    bool[] cbp = new bool[4];
                    sp[0] = pl + Vector2Int.up;
                    cbp[0] = CheckBlock(sp[0]);
                    sp[1] = pl + Vector2Int.up + Vector2Int.up;
                    cbp[1] = CheckBlock(sp[1]);
                    sp[2] = pl + Vector2Int.up + Vector2Int.right;
                    cbp[2] = CheckBlock(sp[2]);
                    sp[3] = pl + Vector2Int.up + Vector2Int.up + Vector2Int.right;
                    cbp[3] = CheckBlock(sp[3]);
                    if (cbp[0] && cbp[1] && cbp[2] && cbp[3])
                    {
                        canBePlaced = true;
                    }
                }
                else if (down)
                {
                    bool[] cbp = new bool[4];
                    sp[1] = pl + Vector2Int.down;
                    cbp[0] = CheckBlock(sp[1]);
                    sp[0] = pl + Vector2Int.down + Vector2Int.down;
                    cbp[1] = CheckBlock(sp[0]);
                    sp[2] = pl + Vector2Int.down + Vector2Int.left;
                    cbp[2] = CheckBlock(sp[2]);
                    sp[3] = pl + Vector2Int.down + Vector2Int.down + Vector2Int.left;
                    cbp[3] = CheckBlock(sp[3]);
                    if (cbp[0] && cbp[1] && cbp[2] && cbp[3])
                    {
                        canBePlaced = true;
                    }
                }
                else if (left)
                {
                    bool[] cbp = new bool[4];
                    sp[1] = pl + Vector2Int.left;
                    cbp[0] = CheckBlock(sp[1]);
                    sp[0] = pl + Vector2Int.left + Vector2Int.left;
                    cbp[1] = CheckBlock(sp[0]);
                    sp[2] = pl + Vector2Int.left + Vector2Int.up;
                    cbp[2] = CheckBlock(sp[2]);
                    sp[3] = pl + Vector2Int.left + Vector2Int.left + Vector2Int.up;
                    cbp[3] = CheckBlock(sp[3]);
                    if (cbp[0] && cbp[1] && cbp[2] && cbp[3])
                    {
                        canBePlaced = true;
                    }
                }
                else if (right)
                {
                    bool[] cbp = new bool[4];
                    sp[0] = pl + Vector2Int.right;
                    cbp[0] = CheckBlock(sp[0]);
                    sp[1] = pl + Vector2Int.right + Vector2Int.right;
                    cbp[1] = CheckBlock(sp[1]);
                    sp[2] = pl + Vector2Int.right + Vector2Int.up;
                    cbp[2] = CheckBlock(sp[2]);
                    sp[3] = pl + Vector2Int.right + Vector2Int.right + Vector2Int.up;
                    cbp[3] = CheckBlock(sp[3]);
                    if (cbp[0] && cbp[1] && cbp[2] && cbp[3]) {
                        canBePlaced = true;
                    }
                }

                if (canBePlaced)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && me.amount >= int.Parse(bText[2].text))
                    {
                        GetComponent<AudioSource>().PlayOneShot(build, 0.3f);
                        me.Buy(int.Parse(bText[2].text));
                        mmg.spawnedBuildings.Add(Instantiate(buildingPrefs[currentBuilding - 1], new Vector3(sp[0].x, sp[0].y, buildingPrefs[currentBuilding - 1].transform.position.z), Quaternion.identity));
                        mmg.map[sp[0].x][sp[0].y] = mmg.spawnedBuildings.Count - 1 + buildingsIndexoffset;
                        mmg.map[sp[1].x][sp[1].y] = mmg.spawnedBuildings.Count - 1 + buildingsIndexoffset;
                        mmg.map[sp[2].x][sp[2].y] = mmg.spawnedBuildings.Count - 1 + buildingsIndexoffset;
                        mmg.map[sp[3].x][sp[3].y] = mmg.spawnedBuildings.Count - 1 + buildingsIndexoffset;
                        for (int i = 0; i < mmg.spawnedBuildings.Count; i++)
                        {
                            if (mmg.spawnedBuildings[i].tag == "Attractor")
                            {
                                mmg.spawnedBuildings[i].GetComponent<MarkAttractor>().RegenMPF();
                            }
                        }
                    }
                }
            }
            else
            if (currentBuilding == 4) //Gatherer
            {
                ClearBoxes();
                if (up)
                {
                    curr += Vector2Int.up;
                    canBePlaced = CheckBlock(curr);
                }
                else if (down)
                {
                    curr += Vector2Int.down;
                    canBePlaced = CheckBlock(curr);
                }
                else if (left)
                {
                    curr += Vector2Int.left;
                    canBePlaced = CheckBlock(curr);
                }
                else if (right)
                {
                    curr += Vector2Int.right;
                    canBePlaced = CheckBlock(curr);
                }

                if (canBePlaced)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && me.amount >= int.Parse(bText[3].text))
                    {
                        GetComponent<AudioSource>().PlayOneShot(build, 0.3f);
                        me.Buy(int.Parse(bText[3].text));
                        mmg.spawnedBuildings.Add(Instantiate(buildingPrefs[currentBuilding - 1], new Vector3(curr.x, curr.y, buildingPrefs[currentBuilding - 1].transform.position.z), Quaternion.identity));
                        mmg.map[curr.x][curr.y] = mmg.spawnedBuildings.Count - 1 + buildingsIndexoffset;
                        for (int i = 0; i < mmg.spawnedBuildings.Count; i++)
                        {
                            if (mmg.spawnedBuildings[i].tag == "Attractor")
                            {
                                mmg.spawnedBuildings[i].GetComponent<MarkAttractor>().RegenMPF();
                            }
                        }
                    }
                }
            }
            else
            if (currentBuilding == 5) //Attractor
            {
                ClearBoxes();
                if (up)
                {
                    curr += Vector2Int.up;
                    canBePlaced = CheckBlock(curr);
                }
                else if (down)
                {
                    curr += Vector2Int.down;
                    canBePlaced = CheckBlock(curr);
                }
                else if (left)
                {
                    curr += Vector2Int.left;
                    canBePlaced = CheckBlock(curr);
                }
                else if (right)
                {
                    curr += Vector2Int.right;
                    canBePlaced = CheckBlock(curr);
                }

                if (canBePlaced)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && me.amount >= int.Parse(bText[4].text))
                    {
                        GetComponent<AudioSource>().PlayOneShot(build, 0.3f);
                        me.Buy(int.Parse(bText[4].text));
                        mmg.spawnedBuildings.Add(Instantiate(buildingPrefs[currentBuilding - 1], new Vector3(curr.x, curr.y, buildingPrefs[currentBuilding - 1].transform.position.z), Quaternion.identity));
                        mmg.map[curr.x][curr.y] = mmg.spawnedBuildings.Count - 1 + buildingsIndexoffset;
                        mmg.spawnedBuildings[mmg.spawnedBuildings.Count - 1].GetComponent<MarkAttractor>().SetLayer(mmg.map[curr.x][curr.y]);

                        for (int i = 0; i < mmg.spawnedBuildings.Count; i++) {
                            if (mmg.spawnedBuildings[i].tag == "Attractor") {
                                mmg.spawnedBuildings[i].GetComponent<MarkAttractor>().RegenMPF();
                            }
                        }
                    }
                }
            }
        }
        else {
            ClearBoxes();
        }
    }

    bool CheckBlock(Vector2Int curr, int buildBlock = 1) {
        bool canBePlaced = false;
        if (mmg.map[curr.x][curr.y] == buildBlock)
        {
            canBePlaced = true;
            CreateGreen(curr.x, curr.y);
        }
        else
        {
            canBePlaced = false;
            CreateRed(curr.x, curr.y);
        }
        return canBePlaced;
    }

    void ClearBoxes() {
        if (boxes.Count > 0)
        {
            for (int i = boxes.Count - 1; i > -1; i--)
            {
                Destroy(boxes[i]);
            }
            boxes.Clear();
        }
    }

    void CreateGreen(float posX, float posY) 
    {
        boxes.Add(Instantiate(markers[1], new Vector3(posX, posY, -6.9f), Quaternion.identity));
    }

    void CreateRed(float posX, float posY)
    {
        boxes.Add(Instantiate(markers[0], new Vector3(posX, posY, -6.9f), Quaternion.identity));
    }
}
