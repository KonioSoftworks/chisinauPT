using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData {
	public int money = 0;
	public int bus = 0;
	public string name = "User";
	public List<int> ownBuses;
}


public class PlayerSave {

	public PlayerData data;

	public PlayerSave(){
		Load ();
		if(data.ownBuses == null)
			data.ownBuses = new List<int>();
		if(data.ownBuses.Count == 0)
			data.ownBuses.Add(0);
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
			ServerScript s = new ServerScript();
			s.register();
		}
	}
}
