using UnityEngine;
using System.Collections;
using System;

public class TimeConvert : MonoBehaviour {

	public string convert_time (float secs){
		TimeSpan t = TimeSpan.FromSeconds(secs);

		if (secs < 60) {
			string answer = string.Format ("{0:D2}s", 
               t.Seconds);
			return answer;
		}
		else if (secs < 60*60) {
			string answer = string.Format ("{0:D2}m:{1:D2}s", 
               t.Minutes, 
               t.Seconds);
			return answer;
		}
		else if (secs < 60*60*24) {
			string answer = string.Format ("{0:D2}h:{1:D2}m:{2:D2}s", 
			                               t.Hours,
			                               t.Minutes, 
			                               t.Seconds);
			return answer;
		}
		else{
			string answer = string.Format ("{0:D2}d {1:D2}h:{2:D2}m:{3:D2}s", 
			                               t.Days,
			                               t.Hours,
			                               t.Minutes, 
			                               t.Seconds);
			return answer;
		}

	}
}
