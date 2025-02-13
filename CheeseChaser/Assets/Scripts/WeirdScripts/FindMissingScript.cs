using UnityEngine;
using UnityEditor;

public class FindMissingScripts : MonoBehaviour
{
    [MenuItem("Tools/Find Missing Scripts")]
    static void FindMissing()
    {
        // Obtener todos los GameObjects de la escena
        GameObject[] allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            Component[] components = obj.GetComponents<Component>();

            foreach (Component component in components)
            {
                if (component == null)
                {
                    Debug.LogWarning($"❌ {obj.name} tiene un script faltante.", obj);
                    count++;
                }
            }
        }

        if (count == 0)
            Debug.Log("✅ No hay scripts faltantes en la escena.");
    }
}

