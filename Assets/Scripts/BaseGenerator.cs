using UnityEngine;

public abstract class BaseGenerator : MonoBehaviour
{
	public int width;
	public int height;

	internal GeneratorHelper helper {
		get { return GetComponent<GeneratorHelper>(); }
	}

	public abstract void Generate();
}
