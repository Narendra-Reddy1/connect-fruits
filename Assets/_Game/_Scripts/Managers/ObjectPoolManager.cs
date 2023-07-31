using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitFrenzy
{
    public class ObjectPoolManager : MonoBehaviour
    {
        #region SINGLETON
        public static ObjectPoolManager instance { get; private set; }
        #endregion SINGLETON


        #region Varibales

        private Dictionary<string, List<GameObject>> m_objectPoolDictionary;

        #endregion Varibales

        #region Unity Methods
        private void Awake()
        {
            instance = this;
            _Init();
        }
        #endregion Unity Methods

        #region Private Methods
        private void _Init()
        {
            m_objectPoolDictionary = new Dictionary<string, List<GameObject>>();
        }
        #endregion Private Methods
        #region Public Methods
        public void InitializePool(string poolName, GameObject template, int poolCount, Transform parent = null)
        {
            if (m_objectPoolDictionary.ContainsKey(poolName))
            {
                Debug.LogError($"{poolName} already exits!! Can't create new one");
                return;
            }

            m_objectPoolDictionary.Add(poolName, new List<GameObject>());
            for (int i = 0; i < poolCount; i++)
            {
                GameObject go = Instantiate(template, parent ? parent : transform);
                go.SetActive(false);
                m_objectPoolDictionary[poolName].Add(go);
            }
        }
        public GameObject GetObjectFromPool(string poolName)
        {
            if (!m_objectPoolDictionary.ContainsKey(poolName))
            {
                Debug.LogError($"Pool doesn't exist!!. Initialize it first.");
                return null;
            }
            for (int i = 0, count = m_objectPoolDictionary[poolName].Count; i < count; i++)
            {
                if (!m_objectPoolDictionary[poolName][i].activeInHierarchy)
                    return m_objectPoolDictionary[poolName][i];
            }
            return null;
        }

        public void RemovePool(string poolName)
        {
            foreach (GameObject go in m_objectPoolDictionary[poolName])
            {
                Destroy(go);
            }
            m_objectPoolDictionary[poolName].Clear();
            m_objectPoolDictionary[poolName] = null;
            m_objectPoolDictionary.Remove(poolName);
        }

        #endregion Public Methods

    }
}