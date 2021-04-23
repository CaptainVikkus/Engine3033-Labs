using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zombie;

namespace Zombie
{
    public enum Type
    {
        Default = -1,
        Female01,
        Female02,
        Male01,
        Male02
    }
}

public class ZombiePicker : MonoBehaviour
{
    public Type type;


    [SerializeField] List<GameObject> hair;
    [SerializeField] List<GameObject> body;

    // Start is called before the first frame update
    void Start()
    {
        if (hair.Count < 4 || body.Count < 4)
        {
            Debug.LogError("Zombie does not have enough choices for picker");
            type = Type.Default; //set default typ and change nothing
        }
        else
        {
            if (type == Type.Default) //Choose a random if default zombie
                SetRandomType();
            SwapFeatures();
        }
    }

    public void SetRandomType()
    {
        int rand = Mathf.RoundToInt(Random.Range(0, (float)Type.Male02));
        type = (Type)rand;
    }

    private void SwapFeatures()
    {
        foreach (var item in hair)
        {
            item.SetActive(false);
        }
        foreach (var item in body)
        {
            item.SetActive(false);
        }

        hair[(int)type].SetActive(true);
        body[(int)type].SetActive(true);
    }
}