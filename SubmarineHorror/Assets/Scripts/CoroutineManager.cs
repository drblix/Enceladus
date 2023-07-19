using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
    private static CoroutineManager _instance;

    private void OnDestroy() => _instance.StopAllCoroutines();
    private void OnApplicationQuit() => _instance.StopAllCoroutines();

    private static CoroutineManager ConstructManager()
    {
        if (_instance != null) return _instance;

        _instance = FindObjectOfType<CoroutineManager>();
        if (_instance != null) return _instance;

        GameObject instanceObject = new("CoroutineManager", typeof(CoroutineManager));
        _instance = instanceObject.GetComponent<CoroutineManager>();

        return _instance;
    }

    public static void Start(IEnumerator routine) => ConstructManager().StartCoroutine(routine);
}
