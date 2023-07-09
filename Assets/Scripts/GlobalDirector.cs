using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ObjectMap<T>
{
    public string id;
    public T gameObject;
}

public class GlobalDirector : MonoBehaviour
{

    public static GlobalDirector Shared { get; private set; }

    public List<MapScript> maps;
    public List<ObjectMap<GameObject>> prefabs;
    public List<ObjectMap<CharacterScriptableObject>> characters;

    public PlayerInput currentPlayerObject;
    private string _lastMapId;
    private MapScript _currentMap;

    public readonly Dictionary<string, GameObject> GameObjectsStash = new();
    private readonly HashSet<string> _scenarioKeys = new();
    private Canvas _vnCanvas;

    public GlobalDirector()
    {
        Shared = this;
    }
    
    private void Awake()
    {
        _vnCanvas = Instantiate(prefabs.First(v => v.id == "VNCanvasPrefab").gameObject).GetComponent<Canvas>();
    }

    private void PrepareToLoadMap()
    {
        foreach (var keyValuePair in GameObjectsStash)
        {
            Destroy(keyValuePair.Value); 
        }
        GameObjectsStash.Clear();

        if (currentPlayerObject)
        {
            Destroy(currentPlayerObject.gameObject);
            currentPlayerObject = null;
        }

        if (_currentMap != null)
        {
            Destroy(_currentMap.gameObject);
            _currentMap = null;
        }
    }

    public static void LoadMap(string map)
    {
        Shared._lastMapId = Shared._currentMap?.id ?? "Initial";
        Shared.PrepareToLoadMap();
        Shared._currentMap = Instantiate(Shared.maps.First(v => v.id == map));
    }

    public static string GetLastMapId()
    {
        return Shared._lastMapId;
    }

    public static void LoadCharacter(string id, CharacterScriptableObject character, Vector2 position, bool isPlayer)
    {
        if (isPlayer && Shared.currentPlayerObject)
        {
            Debug.LogError("Player object is already presented");
            return;
        }

        var obj = Instantiate(Shared.prefabs.First(v => v.id == "CharPrefab").gameObject, position, Quaternion.identity);
        
        var characterObj = obj.GetComponent<CharacterScript>();
        characterObj.entityId = id;
        
        characterObj.characterModel = character != null ? 
            character : 
            Shared.characters.FirstOrDefault(x => x.id == id)?.gameObject;

        if (isPlayer)
        {
            Shared.currentPlayerObject = characterObj.AddComponent<PlayerInput>();
        }
        else
        {
            characterObj.GetComponent<Rigidbody2D>().mass = 99999;
        }

        Shared.GameObjectsStash[id] = characterObj.gameObject;
        Debug.Log($"Character created: {id}");
    }

    public static void LoadItem(string id, Sprite sprite, Vector2 position, bool isPhysical)
    {
        var obj = Instantiate(Shared.prefabs.First(v => v.id == "ItemPrefab").gameObject, position, Quaternion.identity);

        var itemObj = obj.GetComponent<ItemScript>();
        itemObj.entityId = id;
        itemObj.SetSprite(sprite);

        itemObj.GetComponent<BoxCollider2D>().isTrigger = !isPhysical;
        
        Shared.GameObjectsStash[id] = itemObj.gameObject;
    }

    public static void SetCameraTarget(string objectId, bool instantly)
    {
        CameraScript.Shared.followedObject = Shared.GameObjectsStash[objectId];
        Debug.Log($"Set camera to object {objectId}");
        
        if (instantly)
        {
            var transform = CameraScript.Shared.transform;
            
            var pos = Shared.GameObjectsStash[objectId].transform.position;
            pos.z = transform.position.z;
            transform.position = pos;
        }
    }

    public static void SetPlayerInputEnabled(bool enabled)
    {
        if (Shared == null || Shared.currentPlayerObject == null) return;
        Shared.currentPlayerObject.ForceStop();
        Shared.currentPlayerObject.enabled = enabled;
    }

    public static GameObject GetEntityById(string id)
    {
        return Shared.GameObjectsStash[id];
    }

    public static void DestroyEntityById(string id)
    {
        var obj = Shared.GameObjectsStash[id];
        Destroy(obj);
        // Shared.GameObjectsStash[id] = null;
    }

    public static void SetScenarioKey(string key, bool set)
    {
        if (set)
            Shared._scenarioKeys.Add(key);
        else
            Shared._scenarioKeys.Remove(key);
    }

    public static bool GetScenarioKey(string key)
    {
        return Shared._scenarioKeys.Contains(key);
    }

    public static void SetVnCanvasPresented(bool presented)
    {
        Shared._vnCanvas.enabled = true;
    }

    public static CharacterScriptableObject GetCharacterModelById(string id)
    {
        return string.IsNullOrEmpty(id) ? 
            ScriptableObject.CreateInstance<CharacterScriptableObject>() : 
            Shared.characters.First(x => x.id == id).gameObject;
    }
}
