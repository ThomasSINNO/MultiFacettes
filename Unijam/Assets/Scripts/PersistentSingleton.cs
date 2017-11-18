using UnityEngine;

public class PersistentSingleton<T>  : MonoBehaviour where T : Component
	{

		private static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{	
					// search for object of same kind
					instance = FindObjectOfType<T>();
					if (instance == null)
					{
						GameObject obj = new GameObject();
						instance = obj.AddComponent<T>();
					}
				}
				return instance;
			}
		}

		public virtual void Awake()
		{
			if (instance == null)
			{
				instance = this as T;
				DontDestroyOnLoad(transform.gameObject);
			}
			else
			{
				if (this != instance)
				{
					Destroy(this.gameObject);
				}
			}
		}

	}