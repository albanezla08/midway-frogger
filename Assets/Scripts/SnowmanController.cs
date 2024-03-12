using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanController : MonoBehaviour
{
    [SerializeField] private Sprite[] spriteOptions;
    private GameObject tree;

    // Start is called before the first frame update
    void Start()
    {
        // create tree object
        tree = new GameObject("Snowman");

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

    void destroySnowman()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
        Destroy(tree);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if (tree != null)
            {
                destroySnowman();
            }
        }
    }
}
