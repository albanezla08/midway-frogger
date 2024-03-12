using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    // exists in tree tile, not tree objcet directly
    [SerializeField] private Sprite[] spriteOptions;

    // Start is called before the first frame update
    void Start()
    {
        // create tree object
        GameObject tree = new GameObject("Tree");

        // set sprite for tree
        Sprite chosenSprite = spriteOptions[Random.Range(0, spriteOptions.Length)];
        SpriteRenderer treeSR = tree.AddComponent<SpriteRenderer>();
        treeSR.sprite = chosenSprite;
        treeSR.sortingOrder = 2;

        // set position for tree
        tree.transform.SetParent(transform);
        Vector3 tileHalfSize = GetComponent<SpriteRenderer>().bounds.extents;
        Vector3 treeHalfSize = treeSR.bounds.extents;
        Vector3 treeScale = tree.transform.localScale;
        Vector3 treePosition =
            new Vector3(0,
            tileHalfSize.y*treeScale.y );
        tree.transform.localPosition = treePosition;
    }
}
