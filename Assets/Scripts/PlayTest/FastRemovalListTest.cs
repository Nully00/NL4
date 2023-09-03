using NL4.DataStructure;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FastRemovalListTest : MonoBehaviour
{
    private void Start()
    {
        var fastRemovalList = new FastRemovalList<string>();
        var id1 = fastRemovalList.Add("01");
        var id2 = fastRemovalList.Add("02");
        var id3 = fastRemovalList.Add("03");
        var id4 = fastRemovalList.Add("04");
        var id5 = fastRemovalList.Add("05");

        DebugLogList(fastRemovalList);

        fastRemovalList.Remove(id3);

        DebugLogList(fastRemovalList);

        fastRemovalList.Add("06");

        DebugLogList(fastRemovalList);

        Debug.Log(id5.index);
        fastRemovalList.Remove(id4);
        Debug.Log(id5.index);
        fastRemovalList.Remove(id5);

        DebugLogList(fastRemovalList);

        var id7 = fastRemovalList.Add("07");

        DebugLogList(fastRemovalList);

        fastRemovalList.Remove(id1);
        fastRemovalList.Remove(id2);
        fastRemovalList.Remove(id7);
        DebugLogList(fastRemovalList);
    }

    private void DebugLogList<T>(IEnumerable<T> list)
    {
        Debug.Log(string.Join(", ", list));
    }
}
