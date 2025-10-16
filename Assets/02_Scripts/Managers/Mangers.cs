using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

[매니저 클래스]
- 게임 전체에서 공용으로 쓸 매니저들을 한 군데에 모아두는 정적 클래스
- 여기서는 풀매니저 같은 매니저를 관리
- 정적 클래스이기 때문에 new 키워드로 만들지 않아도 됨

*/

public static class Mangers
{
    private static GameObject _root;
    private static PoolManager _pool;

    private static void Init()
    {
        if (_root == null)
        {
            _root = new GameObject("@Managers");
            Object.DontDestroyOnLoad( _root);
        }
    }

    private static void CreateManager<T>(ref T manager, string name) where T : Component
    {
        if (manager == null)
        {
            Init();

            GameObject obj = new GameObject(name);

            manager = obj.AddComponent<T>();

            Object.DontDestroyOnLoad(obj);

            obj.transform.SetParent(_root.transform);
        }
    }

    public static PoolManager Pool
    {
        get
        {
            CreateManager(ref _pool, "PoolManager");
            return _pool;
        }
    }
}
