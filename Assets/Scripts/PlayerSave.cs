using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


[System.Serializable]
public class PlayerData {
	public int money = 0;
	public int bus = 0;
	public string name = "User";
}


public class PlayerSave {

	public PlayerData data;

	public PlayerSave(){
		Load ();
	}
	
	public void Save(){
		BinaryFormatter b = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerData.dat");
		b.Serialize(file, data);
		file.Close();
	}
	
	public void Load(){
		if(File.Exists(Application.persistentDataPath + "/playerData.dat")){
			BinaryFormatter b = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath +"/playerData.dat", FileMode.Open);
			data = (PlayerData)b.Deserialize(file);
			file.Close();
		} else {
			data = new PlayerData();
			data.name +=  Random.Range(0,100).ToString() + "-" + Random.Range(0,100).ToString() + "-" + Random.Range(0,100).ToString();
		}
	}
}
