using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net;
using System.Text;

public class ServerScript : MonoBehaviour {

	public void save(string nickname,int score){
		var request = (HttpWebRequest)WebRequest.Create("http://litenews.tk/chisinaupt/addscore.php");
		
		var postData = "nickname="+nickname;
		postData += "&score=" + score;
		var data = Encoding.ASCII.GetBytes(postData);
		
		request.Method = "POST";
		request.ContentType = "application/x-www-form-urlencoded";
		request.ContentLength = data.Length;
		
		using (var stream = request.GetRequestStream())
		{
			stream.Write(data, 0, data.Length);
		}
		
		var response = (HttpWebResponse)request.GetResponse();
		
		var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
	}

	public void register(){
		var client = new WebClient();
		client.DownloadString("http://litenews.tk/chisinaupt/adduser.php");
	}
}
