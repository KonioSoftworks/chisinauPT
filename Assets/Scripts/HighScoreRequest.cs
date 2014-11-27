using UnityEngine;
using System.Collections;
using System.IO;
using System.Net;
using System.Text;

public class NewBehaviourScript : MonoBehaviour {


		public static void Main ()
		{
			
			WebRequest request = WebRequest.Create ("http://litenews.tk/chisinaupt/addscore.php");
			request.Method = "POST";
			// Create POST data and convert it to a byte array.
			string postData = "This is a test that posts this string to a Web server.";
			byte[] byteArray = Encoding.UTF8.GetBytes (postData);
			// Set the ContentType property of the WebRequest.
			request.ContentType = "application/x-www-form-urlencoded";
			// Set the ContentLength property of the WebRequest.
			request.ContentLength = byteArray.Length;
			// Get the request stream.
			Stream dataStream = request.GetRequestStream ();
			// Write the data to the request stream.
			dataStream.Write (byteArray, 0, byteArray.Length);
			// Close the Stream object.
			dataStream.Close ();
			// Get the response.
			WebResponse response = request.GetResponse ();
			// Display the status.
				// Get the stream containing content returned by the server.
				dataStream = response.GetResponseStream ();
			// Open the stream using a StreamReader for easy access.
			StreamReader reader = new StreamReader (dataStream);
			// Read the content.
			string responseFromServer = reader.ReadToEnd ();
			// Display the content.
			// Clean up the streams.
			reader.Close ();
			dataStream.Close ();
			response.Close ();
	
	}
}
