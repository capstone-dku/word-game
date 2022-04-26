using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class UserData
{
    // TODO: 저장할 데이터 변수들 추가
    // 사용자의 단어 학습 별 갯수
    public int[][] stars = new int[4][];
    public int[][] vocaSet = new int[4][];

    public UserData()
    {
        for (int i = 0; i < 4; i++)
        {
            // TODO: 특정 난이도의 단어 갯수만큼만 배열 크기 선언하기
            // 대충 크게만 선언해줘도 상관은 없을 것 같습니다
            stars[i] = new int[150];
            vocaSet[i] = new int[150];
        }
    }
}

public class SaveLoad : MonoBehaviour
{
    private UserData currentData;
    private string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/data.dat";
    }
    private void Start()
    {
        if(File.Exists(filePath))
        {
            // 파일 데이터가 저장되어있을 때
            // 저장된 데이터를 불러온다.
            currentData = LoadData();
        }
        else
        {
            // 저장된 데이터가 없을때
            // 새로 파일을 만들어 저장한다.
            UserData data = new UserData();
            SaveData(data);
        }
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
        // TODO: 저장 기능 구현
        using(var fs = File.Open(filePath, FileMode.OpenOrCreate))
        {
            using (var bw = new BinaryWriter(fs))
            {

            }
        }
    }
    /// <summary>
    /// 데이터를 불러온다.
    /// </summary>
    /// <returns>불러온 데이터</returns>
    public UserData LoadData()
    {
        UserData data = null;
        // TODO: 불러오기 기능 구현
        using (var fs = File.OpenRead(filePath))
        {
            using (var br = new BinaryReader(fs))
            {
                return data;
            }
        }
    }

    public int GetStars(int difficulty, int set)
    {
        return currentData.stars[difficulty][set];
    }
}
