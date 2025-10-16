using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

[디자인 패턴] 
- 소프트웨어 설계에서 자주 발생하는 문제를 해결하기 위해 정리해둔 재사용 가능한 설계 방법
- 코드 그 자체라는 것보단 설계 아이디어 / 구조를 의미함

    - 생성 패턴
    ㄴ 생성 디자인 패턴은 기존 코드의 유연성과 재사용을 증가시키는 객체를 생성하는 다양한 방법

    - 구조 패턴

    - 행동 패턴

[싱글톤 패턴]
- 싱글톤은 클래스의 인스턴스가 하나만 있도록 하면서 이 인스턴스가 전역 접근 지점을 제공하는 디자인 패턴
- 프로그램 안에서 어떤 객체가 딱 하나만 존재하도록 보장하는 설계 방법

    [장점]
    - 객체가 하나만(게임 전체에서 점수 매니저, 사운드 매니저 같은 건 하나만 있으면 충분)
    - 여러 개 생기지 않으니 데이터 충돌을 막을 수 있음
    - 어디서든 접근이 가능

    [단점]
    - 전역 변수처럼 남용될 위험
    - 테스트 어려움

*/

// 여러 종류의 풀을(Object<T>) 프리팹 이름으로 모아서 관리하는 매니저

public class PoolManager : MonoBehaviour
{
    // 전역 접근이 가능한 싱글톤 인스턴스
    public static PoolManager Instance { get; private set; }

    // Key = string(프리팹 이름), Value = object 저장
    // new objectPool<bullet>
    // 제네릭 타입이 다른 ObjectPool<T>들을 한 딕셔너리에 모음
    // 단, 꺼낼 때는 다시 캐스팅
    private Dictionary<string, object> pools = new Dictionary<string, object>();
    private void Awake()
    {
        if (Instance == null) // Instance가 없으면
        {
            Instance = this; // 나를 싱글톤(Instance)으로 등록
            DontDestroyOnLoad(gameObject); // Scene이 바뀌어도 파괴 X
        }
        else
        {
            Destroy(gameObject); // 중복된 자신은 삭제
        }
    }
    /// <summary>
    /// 풀 등록
    /// </summary>
    /// <typeparam name="T"> MonoBehavior 파생 타입 </typeparam>
    /// <param name="prefab"> 복제에 사용할 프리팹 </param>
    /// <param name="initCount"> 처음에 몇 개? </param>
    /// <param name="parent"> 풀 Root를 둘 부모 </param>
    public void CreatePool<T>(T prefab, int initCount, Transform parent = null) where T : MonoBehaviour
    {
        if (prefab == null) return; // 프리팹이 없으면 하지 마셈

        string key = prefab.name; // 키는 프리팹 이름
        if (pools.ContainsKey(key)) return; // 이미 같은 이름의 풀이 있으면 생성 x

        // 프리팹 이름으로 새로운 풀을 딕셔너리에 등록해서 필요할 때 찾아 쓰기 위해
        pools.Add(key, new ObjectPool<T>(prefab, initCount, parent)); // 새 풀을 만들어 딕셔너리에 등록
    }

    // 풀에서 하나를 꺼냄
    public T GetFromPool<T>(T prefab) where T : MonoBehaviour
    {
        if (prefab == null) return null;

        // 등록할 때 썼던 프리팹 이름으로 풀을 찾음 실패 시 null 리턴
        if (!pools.TryGetValue(prefab.name, out var box))
        {
            return null;
        }

        var pool = box as ObjectPool<T>; 
        // object로 저장된 풀을 원래 제네릭 타입으로 캐스팅

        if (pool != null)
        {
            // 성공했다면 Dequeue()로 하나 꺼내서 활성화된 채로 반환
            return pool.Dequeue();
        }
        else
        {
            return null;
        }
    }
    // 사용 끝난 인스턴스를 다시 풀로 돌림
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
