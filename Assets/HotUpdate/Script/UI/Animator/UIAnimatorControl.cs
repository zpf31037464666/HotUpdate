using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



    [DisallowMultipleComponent]
    public class UIAnimatorControl<T> : MonoBehaviour,LoopScrollPrefabSource, LoopScrollDataSource
    {
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private float duration = 0.5f;

        private Stack<Transform> pool = new Stack<Transform>();
         private LoopScrollRect ls;
        private RectTransform rectTransform;

        private List<T> items = new List<T>();

        protected virtual void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            ls = GetComponent<LoopScrollRect>();
        }

        protected virtual void Start()
        {
             ls.prefabSource = this;
             ls.dataSource = this;
        }

        public GameObject GetObject(int index)
        {
            Transform candidate = pool.Count == 0 ? Instantiate(itemPrefab).transform : pool.Pop();
            candidate.gameObject.SetActive(true);
            AnimateObject(candidate);
            Debug.Log("对象池调用: " + index);
            return candidate.gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            trans.SendMessage("ScrollCellReturn", SendMessageOptions.DontRequireReceiver);
            trans.gameObject.SetActive(false);
            trans.SetParent(transform, false);
            Debug.Log("返回对象池: " + trans.gameObject.name);
            pool.Push(trans);
        }

        public void ProvideData(Transform transform, int idx)
        {
            AnimateObject(transform);
            DataObject(transform, idx);
        }

        protected virtual void DataObject(Transform trans, int index)
        {
            if (index < items.Count)
            {
                var initializable = trans.GetComponent<IInitializable<T>>();
                if (initializable != null)
                {
                    initializable.Init(items[index]);
                }
            }
        }

        protected virtual void AnimateObject(Transform trans)
        {
            var totalWidth = rectTransform.sizeDelta.x;
            var uiAnimator = trans.GetComponent<UIAnimator>();
            uiAnimator.StretchToWidth(totalWidth / 2, totalWidth, duration);
        }

        public virtual void RefreshDataNumber(int amount)
        {
            ls.totalCount = amount;
            ls.RefillCells();
            ls.RefreshCells();
        }
        public virtual void ClearData()
        {
            ls.ClearCells();
        }

        public virtual void UpdateData(List<T> newItems)
        {
            this.items = newItems;
            RefreshDataNumber(newItems.Count);
        }
    }


