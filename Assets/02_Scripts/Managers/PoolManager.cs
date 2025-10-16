using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

[������ ����] 
- ����Ʈ���� ���迡�� ���� �߻��ϴ� ������ �ذ��ϱ� ���� �����ص� ���� ������ ���� ���
- �ڵ� �� ��ü��� �ͺ��� ���� ���̵�� / ������ �ǹ���

    - ���� ����
    �� ���� ������ ������ ���� �ڵ��� �������� ������ ������Ű�� ��ü�� �����ϴ� �پ��� ���

    - ���� ����

    - �ൿ ����

[�̱��� ����]
- �̱����� Ŭ������ �ν��Ͻ��� �ϳ��� �ֵ��� �ϸ鼭 �� �ν��Ͻ��� ���� ���� ������ �����ϴ� ������ ����
- ���α׷� �ȿ��� � ��ü�� �� �ϳ��� �����ϵ��� �����ϴ� ���� ���

    [����]
    - ��ü�� �ϳ���(���� ��ü���� ���� �Ŵ���, ���� �Ŵ��� ���� �� �ϳ��� ������ ���)
    - ���� �� ������ ������ ������ �浹�� ���� �� ����
    - ��𼭵� ������ ����

    [����]
    - ���� ����ó�� ����� ����
    - �׽�Ʈ �����

*/

// ���� ������ Ǯ��(Object<T>) ������ �̸����� ��Ƽ� �����ϴ� �Ŵ���

public class PoolManager : MonoBehaviour
{
    // ���� ������ ������ �̱��� �ν��Ͻ�
    public static PoolManager Instance { get; private set; }

    // Key = string(������ �̸�), Value = object ����
    // new objectPool<bullet>
    // ���׸� Ÿ���� �ٸ� ObjectPool<T>���� �� ��ųʸ��� ����
    // ��, ���� ���� �ٽ� ĳ����
    private Dictionary<string, object> pools = new Dictionary<string, object>();
    private void Awake()
    {
        if (Instance == null) // Instance�� ������
        {
            Instance = this; // ���� �̱���(Instance)���� ���
            DontDestroyOnLoad(gameObject); // Scene�� �ٲ� �ı� X
        }
        else
        {
            Destroy(gameObject); // �ߺ��� �ڽ��� ����
        }
    }
    /// <summary>
    /// Ǯ ���
    /// </summary>
    /// <typeparam name="T"> MonoBehavior �Ļ� Ÿ�� </typeparam>
    /// <param name="prefab"> ������ ����� ������ </param>
    /// <param name="initCount"> ó���� �� ��? </param>
    /// <param name="parent"> Ǯ Root�� �� �θ� </param>
    public void CreatePool<T>(T prefab, int initCount, Transform parent = null) where T : MonoBehaviour
    {
        if (prefab == null) return; // �������� ������ ���� ����

        string key = prefab.name; // Ű�� ������ �̸�
        if (pools.ContainsKey(key)) return; // �̹� ���� �̸��� Ǯ�� ������ ���� x

        // ������ �̸����� ���ο� Ǯ�� ��ųʸ��� ����ؼ� �ʿ��� �� ã�� ���� ����
        pools.Add(key, new ObjectPool<T>(prefab, initCount, parent)); // �� Ǯ�� ����� ��ųʸ��� ���
    }

    // Ǯ���� �ϳ��� ����
    public T GetFromPool<T>(T prefab) where T : MonoBehaviour
    {
        if (prefab == null) return null;

        // ����� �� ��� ������ �̸����� Ǯ�� ã�� ���� �� null ����
        if (!pools.TryGetValue(prefab.name, out var box))
        {
            return null;
        }

        var pool = box as ObjectPool<T>; 
        // object�� ����� Ǯ�� ���� ���׸� Ÿ������ ĳ����

        if (pool != null)
        {
            // �����ߴٸ� Dequeue()�� �ϳ� ������ Ȱ��ȭ�� ä�� ��ȯ
            return pool.Dequeue();
        }
        else
        {
            return null;
        }
    }
    // ��� ���� �ν��Ͻ��� �ٽ� Ǯ�� ����
    public void ReturnPool<T>(T instance) where T : MonoBehaviour
    {
        if (instance == null) return;
        
        if (!pools.TryGetValue (instance.gameObject.name, out var box))
        {
            Destroy(instance.gameObject);
            return;
        }
        var pool = box as ObjectPool<T>;
        if (pool != null)
        {
            pool.Enqueue(instance);
        }
    }
}
