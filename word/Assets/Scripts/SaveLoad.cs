using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;

public class SerializableVector3Converter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(SerializableVector3) || objectType == typeof(SerializableVector3?);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        SerializableVector3 vector = (SerializableVector3)value;

        writer.WriteStartObject();
        writer.WritePropertyName("VectorX");
        serializer.Serialize(writer, vector.x);
        writer.WritePropertyName("VectorY");
        serializer.Serialize(writer, vector.y);
        writer.WritePropertyName("VectorZ");
        serializer.Serialize(writer, vector.z);
        writer.WriteEndObject();
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        float x = 0.0f;
        float y = 0.0f;
        float z = 0.0f;
        while (reader.Read())
        {
            if (reader.TokenType != JsonToken.PropertyName)
                break;

            var propertyName = (string)reader.Value;
            if (!reader.Read())
                continue;

            if (propertyName == "VectorX")
            {
                x = serializer.Deserialize<float>(reader);
            }

            if (propertyName == "VectorY")
            {
                y = serializer.Deserialize<float>(reader);
            }

            if (propertyName == "VectorZ")
            {
                z = serializer.Deserialize<float>(reader);
            }
        }
        return new SerializableVector3(x, y, z);
    }
}
[JsonConverter(typeof(SerializableVector3Converter))]
public struct SerializableVector3
{
    public float x;
    public float y;
    public float z;
    [JsonConstructor]
    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }
    
    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}

[System.Serializable]
public class UserData
{
    // TODO: 저장할 데이터 변수들 추가
    // 사용자의 단어 학습 별 갯수
    public int[][] stars = new int[4][]; // 단어장[난이도][세트]의 별 갯수
    public int[][] vocaSet = new int[4][]; // 
    public bool[][] vocaSetLocked = new bool[4][];
    public int ticket = 0; // 사용자의 보유 티켓 수
    public int[] coin = new int[3]; // 사용자의 보유 코인 수 
    public int[] items = new int[300]; // 가지고 있는 아이템 수
    public int[] purchasable = new int[300]; // 구매할 수 있는 아이템 수
    public Dictionary<SerializableVector3, BUILDING> buildings = new Dictionary<SerializableVector3, BUILDING>();

    public UserData()
    {
        for (int i = 0; i < 4; i++)
        {
            // TODO: 특정 난이도의 단어 갯수만큼만 배열 크기 선언하기
            // 대충 크게만 선언해줘도 상관은 없을 것 같습니다
            stars[i] = new int[150];
            vocaSet[i] = new int[150];
            vocaSetLocked[i] = new bool[150];
            for(int j=0; j<75; j++){
                stars[i][j*2]=0;
                stars[i][j*2+1]=0;
                vocaSet[i][j*2]=0;
                vocaSet[i][j*2+1]=0;
                vocaSetLocked[i][j * 2] = true;
                vocaSetLocked[i][j * 2+1] = true;
            }
            // 단어 세트 처음 3개는 언락되어있음
            vocaSetLocked[i][0] = false;
            vocaSetLocked[i][1] = false;
            vocaSetLocked[i][2] = false;
        }
        coin= new int[3]{0,0,0};
        items = new int[300];
        for(int j=0; j<150; j++){
            items[j*2] = 0;
            items[j*2+1] = 0;
        }
    }
}
 
public class SaveLoad : MonoBehaviour
{
    [SerializeField] private PanelItem panelItem;
    private static UserData currentData;
    private string filePath;

    private void Awake()
    {
        JsonConvert.DefaultSettings = () =>
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new SerializableVector3Converter());
            return settings;
        };
        filePath = Application.dataPath + "/data.json";
        currentData = new UserData();
        if (File.Exists(filePath))
        {
            // 파일 데이터가 저장되어있을 때
            // 저장된 데이터를 불러온다.
            Debug.Log("저장된 데이터 불러옴");
            currentData = LoadData();
        }
        else
        {
            // 저장된 데이터가 없을때
            // 새로 파일을 만들어 저장한다.
            Debug.Log("새로 데이터 저장");
            UserData data = new UserData();
            SaveData(data);
        }

        for (int i = 0; i < currentData.coin.Length; i++)
        {
            panelItem.UpdateCoin(i, GetCoin(i));
        }
        panelItem.UpdateTicket(GetTicket());
    }
    private void Start()
    {
    }
    
    /// <summary>
    /// 현재 데이터를 저장한다.
    /// </summary>
    public void SaveData()
    {
        SaveData(currentData);
    }
    /// <summary>
    /// 매개변수로 들어온 데이터를 저장한다.
    /// </summary>
    /// <param name="data">저장할 데이터</param>
    public void SaveData(UserData data)
    {
        string jdata = JsonConvert.SerializeObject(currentData);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jdata);
        string format = System.Convert.ToBase64String(bytes);

        File.WriteAllText(Application.dataPath + "/data.json", format);

        File.WriteAllText(Application.dataPath + "/data-string.json", jdata);
    }
    /// <summary>
    /// 데이터를 불러온다.
    /// </summary>
    /// <returns>불러온 데이터</returns>
    public UserData LoadData()
    {
        JsonConvert.DefaultSettings = () =>
        {
            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new SerializableVector3Converter());
            return settings;
        };
        UserData data = null;
        string jdata = File.ReadAllText(Application.dataPath + "/data.json");
        byte[] bytes = System.Convert.FromBase64String(jdata);
        string reformat = System.Text.Encoding.UTF8.GetString(bytes);
        data = JsonConvert.DeserializeObject<UserData>(reformat);
        return data;
    }

    public int GetStars(int difficulty, int set)
    {
        return currentData.stars[difficulty][set];
    }

    public void SetStars(int difficulty, int set, int stars)
    {
        currentData.stars[difficulty][set] = stars;
    }

    public int GetTicket()
    {
        return currentData.ticket;
    }

    public void SetTicket(int ticket)
    {
        currentData.ticket = ticket;
    }

    public void AddTicket(int ticket)
    {
        currentData.ticket += ticket;
    }

    public bool IsVocaSetLocked(int difficulty, int set)
    {
        return currentData.vocaSetLocked[difficulty][set];
    }

    public void LockVocaSet(int difficulty, int set, bool lockOrUnlock)
    {
        currentData.vocaSetLocked[difficulty][set] = lockOrUnlock;
    }

    public void AddCoin(int type, int num)
    {
        currentData.coin[type] += num;
    }

    public int GetCoin(int type)
    {
        return currentData.coin[type];
    }

    public void Build(Vector3 pos, BUILDING building)
    {
        if (currentData.buildings.ContainsKey(pos))
            currentData.buildings[pos] = building;
        else
            currentData.buildings.Add(pos, building);
    }

    public BUILDING GetBuilding(Vector3 pos)
    {
        if (currentData.buildings.ContainsKey(pos))
            return currentData.buildings[pos];
        else
            return BUILDING.None;
    }
}
