using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

///<summary>
/// 풀링할 프리팹 오브젝트 입력용
///</summary>
public class PoolItem
{
    public string key;
    public GameObject item;
}
///<summary>
/// 0r4c1E's ObjectPoolManager ===== ver. 1.1
///</summary>
public class PoolManager : MonoBehaviour
{
    public static PoolManager inst;
    [Header("Set Pooling Object & Key")]
    ///<summary>
    /// 풀링할 오브젝트와 키값을 입력
    ///</summary>
    public List<PoolItem> objects;

    ///<summary>
    /// 풀링한 오브젝트들을 보관
    ///</summary>
    private Dictionary<string, Queue<GameObject>> items = new Dictionary<string, Queue<GameObject>>();
    Queue<GameObject> _buff;
    GameObject _obj;

    private void Awake()
    {
        inst = this;
    }
    ///<summary>
    /// 모든 오브젝트를 입력한 숫자만큼 풀링하는 메서드
    ///</summary>
    ///<param name="cnt"> 각 항목당 풀링할 갯수 </param>
    public static void ObjectPool(int cnt)
    {
        for (int i = 0; i < inst.objects.Count; i++)
        {
            inst._buff = new Queue<GameObject>();
            for (int j = 0; j < cnt; j++)
            {
                inst._buff.Enqueue(CreateObject(i));
            }
            inst.items.Add(inst.objects[i].key, inst._buff);
        }
    }
    ///<summary>
    /// 특정 오브젝트를 만들어주는 메서드
    ///</summary>
    ///<param name="id"> 'objects'에서의 리스트 번호 </param>
    private static GameObject CreateObject(int id)
    {
        inst._obj = Instantiate(inst.objects[id].item, Vector3.zero, Quaternion.identity);
        inst._obj.SetActive(false);
        inst._obj.transform.SetParent(inst.transform);
        return inst._obj;
    }
    ///<summary>
    /// 입력한 오브젝트를 Return 시켜주는 메서드
    ///</summary>
    ///<param name="key"> 'items'에서의 딕셔너리 키값 </param>
    public static GameObject GetPoolItem(string key)
    {
        if (inst.items[key].Count > 0)
        {
            inst._obj = inst.items[key].Dequeue();
        }
        else
        {
            inst._obj = CreateObject(ReturnListCount(key));
        }
        return inst._obj;
    }
    ///<summary>
    /// 입력한 'key'값에 맞는 'objects'상의 리스트 번호를 리턴
    ///</summary>
    ///<param name="key"> 'items'에서의 딕셔너리 키값 </param>
    private static int ReturnListCount(string key)
    {
        for (int i = 0; i < inst.objects.Count; i++)
        {
            if (inst.objects[i].key == key) return i;
        }
        return 0;
    }
    ///<summary>
    /// 오브젝트를 'items'의 큐로 되돌리는 메서드
    ///</summary>
    ///<param name="key"> 'items'에서의 딕셔너리 키값 </param>
    ///<param name="obj"> 되돌릴 게임오브젝트 </param>
    public static void ReturnPoolObject(string key, GameObject obj)
    {
        obj.SetActive(false);
        inst.items[key].Enqueue(obj);
    }
}
