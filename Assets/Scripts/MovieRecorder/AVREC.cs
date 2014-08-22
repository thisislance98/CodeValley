using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

public class AVREC : MonoBehaviour {
	
	float timeToScreenCapture;
	float targetFramerate;
	
	bool isRecording = false;
	bool isRecordingAudio = false;
	bool isFinalizing = false;
	bool isDebugging = false;
	bool isRecordingGUI = false;

	string temporaryFolder;
	string tempVideo;
	string tempAudio;
	string outputMovie;
	
	TcpListener videoServer;
	TcpListener audioServer;
	
	TcpClient videoSocket;
	TcpClient audioSocket;
	
	NetworkStream videoStream;
	NetworkStream audioStream;
	
	Process videoProcess;
	Process audioProcess;
	Process movieProcess;
	
	Texture2D screenTexture;
	Color32[] data;
	GCHandle dataHandle;
	byte[] pixels;
	
	int screenWidth;
	int screenHeight;
	
	private Byte[] audioByteBuffer;	
	private GCHandle audioHandle;
	private IntPtr audioPointer = IntPtr.Zero;
	
	string ffmpeg() 
	{
	#if UNITY_EDITOR
		return (Application.platform == RuntimePlatform.WindowsEditor) ? Application.streamingAssetsPath + "/ffmpeg/windows/ffmpeg.exe" : Application.streamingAssetsPath + "/ffmpeg/macosx/ffmpeg.command";
	#elif UNITY_STANDALONE_WIN
		return Application.streamingAssetsPath + "/ffmpeg/windows/ffmpeg.exe";
	#elif UNITY_STANDALONE_OSX
		return Application.streamingAssetsPath + "/ffmpeg/macosx/ffmpeg.command";
	#endif
	
	}
	
	public bool IsRecording()
	{
		return isRecording;	
	}
	
	public bool IsFinalizing()
	{
		return isFinalizing;	
	}
	
	void OnAudioFilterRead(float[] data, int channels)
    {
    	if (isRecordingAudio && isRecording)
		{
			Marshal.Copy(data, 0, audioPointer, 2048);	
			audioStream.Write(audioByteBuffer, 0, audioByteBuffer.Length);
        }
    }
	
	void RemoveAllTemporaryFiles()
	{
		if(Directory.Exists(temporaryFolder))
		{
			foreach (FileInfo file in new System.IO.DirectoryInfo(temporaryFolder).GetFiles())
			    File.Delete(file.FullName);

			Directory.Delete(temporaryFolder);
		}
	}
	
	public void StartREC(string fileName, bool recordGUI = false, int targetFramerate = 24, bool debug = false)
	{
		if(!isRecording && !isFinalizing)
		{
			isRecordingGUI = recordGUI;
			isRecording = true;
			
			isDebugging = debug;
			
			screenWidth = Screen.width;
			screenHeight = Screen.height;
			
			if (screenWidth % 2 != 0)
				screenWidth -= 1;
			
			if (screenHeight % 2 != 0)
				screenHeight -= 1;
			
			CleanUp();
			
			this.targetFramerate = targetFramerate;
			string directory = new System.IO.FileInfo(fileName).Directory.FullName;
			
			temporaryFolder = directory + "/" + Guid.NewGuid().ToString() + "/";
			tempVideo = temporaryFolder + Guid.NewGuid().ToString() + ".mp4";
			tempAudio = temporaryFolder + Guid.NewGuid().ToString() + ".wav";
			outputMovie = directory + "/" + new System.IO.FileInfo(fileName).Name.Replace(".mp4", "") + ".mp4";
			
			Directory.CreateDirectory(temporaryFolder);	
			
			screenTexture = new Texture2D(screenWidth, screenHeight, TextureFormat.ARGB32, false);
			pixels = new byte[screenWidth * screenHeight * sizeof(Int32)];
			
			videoServer = new System.Net.Sockets.TcpListener(IPAddress.Parse("127.0.0.1"), 1234); 
			videoServer.Start();
			
			videoProcess = StartProcess(ffmpeg(), "-f rawvideo -r " + targetFramerate.ToString() + " -pix_fmt rgba -s " + screenWidth.ToString() + 
			"x" + screenHeight.ToString() + " -i tcp://127.0.0.1:1234" + 
			" -vf vflip -preset ultrafast -pix_fmt yuv420p -vcodec libx264 -y \"" + tempVideo + "\"");
			
			videoProcess.EnableRaisingEvents = true;
			videoProcess.Exited += ForcedExit;
		
			videoSocket = SetupSocket(videoServer);
			videoStream = videoSocket.GetStream();
			
			if(GetComponent(typeof(AudioListener)))
			{
				audioServer = new System.Net.Sockets.TcpListener(IPAddress.Parse("127.0.0.1"), 1235); 
				audioServer.Start();
			
				audioProcess = StartProcess(ffmpeg(), "-f f32le -ar " + AudioSettings.outputSampleRate + 
					" -ac 2 -i tcp://127.0.0.1:1235 -preset ultrafast -y \"" + tempAudio + "\"");
				
				audioProcess.EnableRaisingEvents = true;
				audioProcess.Exited += ForcedExit;
				
				audioByteBuffer = new Byte[8192];
				audioHandle = GCHandle.Alloc(audioByteBuffer, GCHandleType.Pinned);
				audioPointer = audioHandle.AddrOfPinnedObject();
				
				audioSocket = SetupSocket(audioServer);
				audioStream = audioSocket.GetStream();
				
				isRecordingAudio = true;
			}
			else
			{
				UnityEngine.Debug.LogError("AudioListeners is missing, no audio will be recorded");	
			}
			
			if (isRecordingGUI)
			{
				StartCoroutine(RecordVideoWithGUI());	
			}
			
		}
	}
	
	TcpClient SetupSocket(TcpListener tcpServer)
	{
		TcpClient socket = tcpServer.AcceptTcpClient();
		
		int maxBufferSize = Int32.MaxValue;
		while(true)
		{
		 	try
			{ 
				socket.Client.SendBufferSize = maxBufferSize;
				break;
		    }
		    catch
			{
				maxBufferSize = maxBufferSize / 10;
		    }
		}
		return socket;
	}

	void CleanUp()
	{
		RemoveAllTemporaryFiles();
		
		timeToScreenCapture = 0.0f;
		isFinalizing = false;
	}
	
	void Update()
	{
		if(isRecording)
		{
			float width = Screen.width % 2 != 0 ? Screen.width - 1 : Screen.width;
			float height = Screen.height % 2 != 0 ? Screen.height - 1 : Screen.height;
			
			if(screenWidth != width || screenHeight != height )
			{
				UnityEngine.Debug.LogError("Screen size changed: Quitting recording");
				StopREC();
				return;
			}
		}
		
		if(isFinalizing)
		{
			if(movieProcess != null && movieProcess.HasExited)
			{
				isFinalizing = false;
				
				CleanUp();
				print("Movie created: " + outputMovie);
			}
		}
	}
	
	public void StopREC()
	{
		if(isRecording && !isFinalizing)
		{  
			isRecording = false;
			isRecordingAudio = false;
			isFinalizing = true;
			
			videoSocket.Client.Disconnect(false);
		
			videoSocket.Close();
			videoServer.Stop();
			videoProcess.Close();
			
			if(audioSocket != null && audioSocket.Connected)
			{
				audioSocket.Client.Disconnect(false);
			
				audioSocket.Close();
				audioServer.Stop();
				audioProcess.Close();
				
				movieProcess = StartProcess(ffmpeg(), "-i \"" + tempVideo + "\" -i \"" + tempAudio + "\" -preset ultrafast -strict -2 -y \"" + outputMovie + "\"");
			}
			else {
				int maxTries = 1000;
				while(File.Exists(outputMovie) && maxTries > 0)
				{
					File.Delete(outputMovie);
					System.Threading.Thread.Sleep(1);
					maxTries--;
				}
				
				if(!File.Exists(outputMovie))
				{
					File.Move(tempVideo, outputMovie);
					print("Movie created: " + outputMovie);
				}
				else 
				{
					print("Failed to replace already existing file: " + outputMovie);
				}
				
				CleanUp();
			}
		}
	}
	
	private void ForcedExit(object sender, System.EventArgs e)
	{
		if (isRecording)
		{
			// For some unknown reason the process exits when the debugger is attached..
			UnityEngine.Debug.LogError("Exporting exited by force, detach bugger if attached.");
			
			StopREC();
		}
	}
	
	IEnumerator RecordVideoWithGUI()
	{
		while (isRecording && isRecordingGUI)
		{
			yield return new WaitForEndOfFrame();
			ScreenCapture();
		}
	}
	
	void OnPostRender()
	{
		if(isRecording && !isRecordingGUI)
		{
			ScreenCapture();
		}
	}
	
	void ScreenCapture()
	{
		timeToScreenCapture += Time.deltaTime;
			
		int numScreens = 0;
		while(timeToScreenCapture > (1.0f / (float)targetFramerate) )
		{
			timeToScreenCapture -= (1.0f / (float)targetFramerate);
			numScreens++;
		}
		
		if(numScreens > 0)
		{
		   	screenTexture.ReadPixels(new Rect(0, 0, screenWidth, screenHeight), 0, 0, false);
			dataHandle = GCHandle.Alloc(screenTexture.GetPixels32(), GCHandleType.Pinned);
			Marshal.Copy(dataHandle.AddrOfPinnedObject(), pixels, 0, pixels.Length);
			dataHandle.Free();
			
			for(int i=0; i<numScreens; ++i)
				videoStream.Write(pixels, 0, pixels.Length);
		}	
	}
	
	public void DataReceived(object e, DataReceivedEventArgs outLine)
	{
	   print(DateTime.Now + " - " + outLine.Data);
	}
	
	Process StartProcess(string processName, string arguments)
	{

		if (!File.Exists(processName))
		{
			UnityEngine.Debug.LogError("ffmpeg is located in wrong location, please make sure that the following path is correct: " + processName);
			return null;
		}
		else
		{

			Process process = new Process();
			process.StartInfo.FileName = processName;
			process.StartInfo.Arguments = arguments;
			
			if (isDebugging)
			{
			 	process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.OutputDataReceived += new DataReceivedEventHandler(DataReceived);
				process.ErrorDataReceived += new DataReceivedEventHandler(DataReceived);
			}
	
			process.StartInfo.CreateNoWindow = true;		
			process.StartInfo.UseShellExecute = false;
			process.Start();
			
			if (isDebugging)
			{
				process.BeginErrorReadLine();
				process.BeginOutputReadLine();
			}
			
			return process;
		}
	}
}