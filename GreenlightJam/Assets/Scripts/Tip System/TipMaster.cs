using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipMaster : MonoBehaviour
{
    [SerializeField] private Tip tip;
    private bool coroutineActive;
    private bool runningThroughQueue;

    public void AddTipToQueue(TipQueue queue)
    {
        TipQueue newQueue = new TipQueue(queue);

        StartCoroutine(QueueTip(newQueue));
    }
    IEnumerator QueueTip(TipQueue queue)
    {
        while (runningThroughQueue)
        {
            yield return null;
        }
        runningThroughQueue = true;

        StartCoroutine(ScrollQueue(queue));

        while (coroutineActive)
        {
            yield return null;
        }
        runningThroughQueue = false;
    }

    IEnumerator ScrollQueue(TipQueue queue)
    {
        coroutineActive = true;

        tip.Init(queue.info, queue.worldPos);

        yield return new WaitForSeconds(queue.info.Length / 28 + 3);

        tip.DestroyTip();

        coroutineActive = false;
    }
    public void CancelQueue()
    {
        StopAllCoroutines();
        tip.DestroyTip();
        coroutineActive = false;
        runningThroughQueue = false;
    }
}
[System.Serializable]
public class TipQueue
{
    [TextArea(5, 20)]
    public string info;
    [SerializeField] private Transform worldTransform;
    [SerializeField] private Vector3 offset;

    public Vector3 worldPos;

    public TipQueue(TipQueue queue)
    {
        info = queue.info;
        worldPos = queue.worldTransform.position + queue.offset;
    }
}