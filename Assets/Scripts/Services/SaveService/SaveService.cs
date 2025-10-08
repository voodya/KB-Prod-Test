using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveService : IBootable
{
    void SetData<T>(string key, T data);
    bool TryGetData<T>(string key, out T data) where T : class, new();
}

public class SaveService : ISaveService
{
    private Dictionary<string, string> _data;
    private string _saveKey;

    public int Priority => 0;

    public SaveService(string saveKey)
    {
        _saveKey = saveKey;
    }

    public async UniTask Boot()
    {
        if(PlayerPrefs.HasKey(_saveKey))
        {
            string rawData = PlayerPrefs.GetString(_saveKey);
           _data =  JsonConvert.DeserializeObject<Dictionary<string, string>>(rawData);
        }
        else
        {
            _data = new Dictionary<string, string>();
            PlayerPrefs.SetString(_saveKey, JsonConvert.SerializeObject(_data));
        }
    }

    public bool TryGetData<T>(string key, out T data) where T : class, new()
    {
        data = default(T);

        if(_data.ContainsKey(key))
        {
            data = JsonConvert.DeserializeObject<T>(_data[key]);
            return true;

        }
        return false;

    }

    public void SetData<T>(string key, T data)
    {
        _data[key] = JsonConvert.SerializeObject(data);
        PlayerPrefs.SetString(_saveKey, JsonConvert.SerializeObject(_data));
    }


}
