using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab; // 복사할 원본 프리팹
    private Queue<T> pool = new Queue<T>();

    public Transform Root { get; private set; } // pool을 담아둘 부모 prefab

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="prefab"> 복제할 원본 프리팹 </param>
    /// <param name="initCount"> 처음에 몇 개를 미리 만들지 </param>
    /// <param name="parent"> Root를 어떤 부모 아래에 둘 거냐 </param>
    public ObjectPool(T prefab, int initCount, Transform parent = null)
    {
        this.prefab = prefab;

        // 풀 컨테이너 생성(Root) -> 이름은 "[프리팹이름]_pool"
        Root = new GameObject($"{prefab.name}_pool").transform;

        if (parent != null)
        {
            Root.SetParent(parent, false);
        }
        else if (PoolManager.Instance != null)
        {
            Root.SetParent(PoolManager.Instance.transform, false);
        }

        // 처음에 지정한 갯수만큼 미리 만들어서 큐에 넣는다
        for (int i = 0; i < initCount; i++)
        {
            var inst = Object.Instantiate(prefab, Root); // Root의 자식으로 생성
            inst.name = prefab.name; // 이름
            inst.gameObject.SetActive(false); // 꺼진 상태로
            pool.Enqueue(inst); // 큐에 넣는다
        }
    }

    // 꺼내서 사용
    public T Dequeue()
    {
        if (pool.Count == 0) return null;
        var inst = pool.Dequeue(); // 큐에서 하나 빼자
        inst.gameObject.SetActive(true); // 켜서 사용
        return inst;
    }

    // 사용을 끝낸 오브젝트를 다시 넣자
    public void Enqueue(T instance)
    {
        if (instance == null) return;

        instance.gameObject.SetActive(false); // 끄기
        pool.Enqueue(instance); // 넣기
    }
}
