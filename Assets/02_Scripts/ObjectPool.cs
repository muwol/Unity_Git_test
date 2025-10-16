using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab; // ������ ���� ������
    private Queue<T> pool = new Queue<T>();

    public Transform Root { get; private set; } // pool�� ��Ƶ� �θ� prefab

    /// <summary>
    /// ������
    /// </summary>
    /// <param name="prefab"> ������ ���� ������ </param>
    /// <param name="initCount"> ó���� �� ���� �̸� ������ </param>
    /// <param name="parent"> Root�� � �θ� �Ʒ��� �� �ų� </param>
    public ObjectPool(T prefab, int initCount, Transform parent = null)
    {
        this.prefab = prefab;

        // Ǯ �����̳� ����(Root) -> �̸��� "[�������̸�]_pool"
        Root = new GameObject($"{prefab.name}_pool").transform;

        if (parent != null)
        {
            Root.SetParent(parent, false);
        }
        else if (PoolManager.Instance != null)
        {
            Root.SetParent(PoolManager.Instance.transform, false);
        }

        // ó���� ������ ������ŭ �̸� ���� ť�� �ִ´�
        for (int i = 0; i < initCount; i++)
        {
            var inst = Object.Instantiate(prefab, Root); // Root�� �ڽ����� ����
            inst.name = prefab.name; // �̸�
            inst.gameObject.SetActive(false); // ���� ���·�
            pool.Enqueue(inst); // ť�� �ִ´�
        }
    }

    // ������ ���
    public T Dequeue()
    {
        if (pool.Count == 0) return null;
        var inst = pool.Dequeue(); // ť���� �ϳ� ����
        inst.gameObject.SetActive(true); // �Ѽ� ���
        return inst;
    }

    // ����� ���� ������Ʈ�� �ٽ� ����
    public void Enqueue(T instance)
    {
        if (instance == null) return;

        instance.gameObject.SetActive(false); // ����
        pool.Enqueue(instance); // �ֱ�
    }
}
