using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class BotMono : MonoBehaviour {

	public Bot bot;
	public bool enableWriteData = true;
	
	public string filename = "test.xml";
	

	void ConstructExample(){

		bot = new Bot("Sample Bot");
		Cell cell = new Cell(bot);
		new Engine(cell, 1);
		new Engine(cell, 5);
	}

	// Use this for initialization
	void Start () {
//		ConstructExample();
//		if (enableWriteData){
//			WriteData();
//		}
//		else{
//			ReadData();
//		}
//		bot.DebugPrint();
	
	}
	
	// Update is called once per frame
	void Update () {
	//	WriteData()
	
	}
	
	
	string CreateFilePath(){
		return Application.persistentDataPath + "/" +  filename;
	}
	
	public void WriteData(){
		XmlSerializer serializer = new XmlSerializer(typeof(Bot));
		FileStream stream = new FileStream(CreateFilePath(), FileMode.Create);
		//serializer.Serialize(stream, bot.rootModule);
		serializer.Serialize(stream, bot);
		stream.Close();
	}
	
	public void ReadData(){
		XmlSerializer serializer = new XmlSerializer(typeof(Bot));
		FileStream stream = new FileStream(CreateFilePath(), FileMode.Open);
		bot = serializer.Deserialize(stream) as Bot;
		stream.Close();
	}
}
