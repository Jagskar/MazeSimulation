using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private int currentCollectables = 0;
    public int CurrentCollectables
    {
        get { return currentCollectables; }
        set { currentCollectables = value; }
    }
    private bool isGameOver = false;

    private int cameraSwitchCounter = 0;
    private int collisionCounter = 0;
    //public int CollisionCounter
    //{
    //    get { return collisionCounter; }
    //    set { collisionCounter = value; }
    //}
    private int totalCollectables;
    private int fpSwitchCounter = 0;
    private int tpSwitchCounter = 0;
    private int tdSwitchCounter = 0;
    private int commandsIssued = 0;
    private int fpCommands = 0;
    private int tpCommands = 0;
    private int tdCommands = 0;
    private int fpCollisions = 0;
    private int tpCollisions = 0;
    private int tdCollisions = 0;

    private float startTime;
    private float currentTime;
    private float totalTimeTaken;
    private float startTimeFP; // Start timer first time switching to first person view
    private float startTimeTP; // Start timer first time switching to third person view
    private float startTimeTD; // Start timer first time switching to birds eye view
    private float timeFP; // Time spent in first person view
    private float timeTP; // Time spent in third person view
    private float timeTD; // Time spent in birds eye view

    private bool hasPaused = false;
    private bool[] pauseHistory = new bool[3];

    //private System.Timers.Timer fpTimer; // First person
    //private System.Timers.Timer tpTimer; // Third person
    //private System.Timers.Timer tdTimer; // Top Down
    private System.Diagnostics.Stopwatch fpTimer = new System.Diagnostics.Stopwatch(); // First person
    private System.Diagnostics.Stopwatch tpTimer = new System.Diagnostics.Stopwatch(); // Third person
    private System.Diagnostics.Stopwatch tdTimer = new System.Diagnostics.Stopwatch(); // Top Down

    private string cameraPref;
    private string filePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "ExperimentResults.txt");
    private string sagatFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "SAGAT.odt");
    private string mins = string.Empty;
    private string previousCamName = string.Empty;
    private static string currentCamName = string.Empty;
    public static string CurrentCamName
    {
        get { return currentCamName; }
        set { currentCamName = value; }
    }
    private string prevName = string.Empty;

    public Text timesCameraSwitched;
    public Text finalTime;
    public Text objectsRemaining;
    public Text timeTaken;
    public Text collisions;
    public Text totalCollisions;
    public Text fpCollisionText;
    public Text tpCollisionText;
    public Text tdCollisionText;


    public Camera firstPersonCam;
    //public CinemachineVirtualCameraBase thirdPersonCam;
    public Camera thirdPersonCam;
    public Camera birdsEyeCam;
    public GameObject experimentCompleteUI;
    public GameObject pauseMenuUI;

    // Start is called before the first frame update
    void Start()
    {
        SetCameraPref();
        SelectCameraView(cameraPref);
        SetCurrentCollectables();
        UpdateUI();
        startTime = Time.time;
        HideCamCollisionText();
        //Debug.Log(Screen.currentResolution);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime = Time.time - startTime;
        mins = ((int)currentTime / 60).ToString();
        string secs = (currentTime % 60).ToString("f2");
        timeTaken.text = mins + ":" + secs;
        if (cameraPref == "Free Selection")
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                FirstPersonView();
                fpSwitchCounter++;
                cameraSwitchCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                ThirdPersonView();
                tpSwitchCounter++;
                cameraSwitchCounter++;
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                BirdsEyeView();
                tdSwitchCounter++;
                cameraSwitchCounter++;
            }
        }

        //PrintInfo();
        if (CurrentCollectables == 0)
        {
            EndGame();
        }
        UpdateCollisionCounter();
        HandleSagat();
    }

    void HideCamCollisionText()
    {
        fpCollisionText.gameObject.SetActive(false);
        tpCollisionText.gameObject.SetActive(false);
        tdCollisionText.gameObject.SetActive(false);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "obstacle")
            SetCollisonVariables();
    }

    void SetCollisonVariables()
    {
        if (currentCamName == "First Person")
        {
            fpCollisions++;
            fpCollisionText.text = fpCollisions.ToString();
        }
        else if (currentCamName == "Third Person")
        {
            tpCollisions++;
            tpCollisionText.text = tpCollisions.ToString();
        }
        else if (currentCamName == "Top Down")
        {
            tdCollisions++;
            tdCollisionText.text = tdCollisions.ToString();
        }
        collisionCounter++;
    }

    void SetCameraPref()
    {
        cameraPref = PlayerPrefs.GetString("CameraMode");
    }

    public void UpdateUI()
    {
        objectsRemaining.text = "Objects Remaining: " + CurrentCollectables.ToString();
    }

    public void UpdateCollisionCounter()
    {
        collisions.text = "Collisions: " + collisionCounter.ToString();
    }

    void SetCurrentCollectables()
    {
        CurrentCollectables = GameObject.FindGameObjectsWithTag("collectable").Length;
        totalCollectables = CurrentCollectables;
    }

    void SelectCameraView(string view)
    {
        switch (view)
        {
            case "First Person View":
                FirstPersonView();
                break;
            case "Third Person View":
                ThirdPersonView();
                break;
            case "Top Down View":
                BirdsEyeView();
                break;
            case "Free Selection":
                FirstPersonView();
                break;
        }
    }

    void FirstPersonView()
    {
        PauseCamTimer(previousCamName);
        // Start timer
        fpTimer.Start();
        firstPersonCam.enabled = true;
        thirdPersonCam.gameObject.SetActive(false);
        birdsEyeCam.enabled = false;
        previousCamName = "First Person";
        currentCamName = "First Person";
    }

    void ThirdPersonView()
    {
        PauseCamTimer(previousCamName);
        tpTimer.Start();
        firstPersonCam.enabled = false;
        thirdPersonCam.gameObject.SetActive(true);
        birdsEyeCam.enabled = false;
        previousCamName = "Third Person";
        currentCamName = "Third Person";
    }

    void BirdsEyeView()
    {
        PauseCamTimer(previousCamName);
        tdTimer.Start();
        firstPersonCam.enabled = false;
        thirdPersonCam.gameObject.SetActive(false);
        birdsEyeCam.enabled = true;
        previousCamName = "Top Down";
        currentCamName = "Top Down";
    }

    void PauseCamTimer(string camName)
    {
        if (camName != string.Empty)
        {
            if (camName == "First Person")
            {
                fpTimer.Stop();
            }
            else if (camName == "Third Person")
            {
                tpTimer.Stop();
            }
            else if (camName == "Top Down")
            {
                tdTimer.Stop();
            }
        }
    }

    void ResumeCamTimer(string camName)
    {
        if (camName != string.Empty)
        {
            if (camName == "First Person")
            {
                fpTimer.Start();
            }
            else if (camName == "Third Person")
            {
                tpTimer.Start();
            }
            else if (camName == "Top Down")
            {
                tdTimer.Start();
            }
        }
    }

    void PrintInfo()
    {
        Debug.Log("Total Collisions: " + collisionCounter);
        Debug.Log("FpCollisions: " + fpCollisions);
        Debug.Log("TpCollisions: " + tpCollisions);
        Debug.Log("TdCollisions: " + tdCollisions);
    }

    void EndGame()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            EndGameRoutine();
            // Todo:
            // Build walls around the pit so that user cannot jump off the edge into it, in reality this would damage the robot
            // Add decceleration when robot moves up ramp 
            // Increase velocity when robot goes down ramp
            // Save path taken by user (should be done as the user plays)
        }
    }

    void EndGameRoutine()
    {
        // Hide text UI components
        objectsRemaining.gameObject.SetActive(false);
        timeTaken.gameObject.SetActive(false);
        collisions.gameObject.SetActive(false);
        
        // Stop timer
        totalTimeTaken = currentTime;

        experimentCompleteUI.SetActive(true);
        timesCameraSwitched.text = "Times Camera Switched: " + cameraSwitchCounter;
        finalTime.text = "Time Taken To Complete: " + totalTimeTaken;
        totalCollisions.text = "Total Collisions: " + collisions.text;
        WriteToFile();
    }

    void WriteToFile()
    {
        string[] fields = { "Time Taken", "Number of Collisions", "Camera Mode", "Number of Camera Switches" };
        try
        {
            using(System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
            {
                file.WriteLine(fields[0] + ": " + totalTimeTaken + ", " + fields[2] + ": " + cameraPref + ", " + fields[3] + ": " + cameraSwitchCounter);
                file.WriteLine("Times Switched to First Person:" + fpSwitchCounter + ", " + "Times Switched to Third Person:" + tpSwitchCounter + ", " + "Times Switched to Birds Eye:" + tdSwitchCounter);
                file.WriteLine("Time spent in first person view:" + fpTimer.Elapsed);
                file.WriteLine("Time spent in third person view:" + tpTimer.Elapsed);
                file.WriteLine("Time spent in birds eye view:" + tdTimer.Elapsed);
                file.WriteLine("Total Commands Issued: " + PlayerMovement.CommandsIssued);
                int count = ExtractCount(collisions.text);
                string strfpCollisionText = ParseText(fpCollisionText);
                string strtpCollisionText = ParseText(tpCollisionText);
                string strtdCollisionText = ParseText(tdCollisionText);
                file.WriteLine("Total Collisions: " + count);
                file.WriteLine("Total Collisions in First Person Mode: " + strfpCollisionText);
                file.WriteLine("Total Collisions in Third Person Mode: " + strtpCollisionText);
                file.WriteLine("Total Collisions in Top Down Mode: " + strtdCollisionText);
                file.WriteLine("\n");
            }
        }
        catch(System.Exception ex)
        {
            throw new System.ApplicationException("Error writing to file: ", ex);
        }
    }

    int ExtractCount(string text)
    {
        string res = Regex.Match(text, @"\d+").Value;
        int count = System.Convert.ToInt32(res);

        return count;
    }
    string ParseText(Text inText)
    {
        string toReturn = inText.text;
        if (toReturn.Equals("New Text"))
            toReturn = "0";
        return toReturn;
    }
    void PauseGame()
    {
        PauseMenu.Pause(pauseMenuUI);
        PauseCamTimer(currentCamName);
    }
    void ResumeGame()
    {
        PauseMenu.Resume(pauseMenuUI);
        ResumeCamTimer(currentCamName);
    }

    bool IsFileOpen(FileInfo file)
    {
        try
        {
            using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
            {
                stream.Close();
            }
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }

        //file is not locked
        return false;
    }

    public bool FileIsOpen(string path)
    {
        System.IO.FileStream a = null;

        try
        {
            a = System.IO.File.Open(path,
            System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
            return false;
        }
        catch (System.IO.IOException ex)
        {
            return true;
        }

        finally
        {
            if (a != null)
            {
                a.Close();
                a.Dispose();
            }
        }
    }

    void HandleSagat()
    {
        if (CurrentCollectables == totalCollectables - 2 && pauseHistory[0] == false)
        {
            PauseGame();
            //if (!IsFileOpen(new FileInfo(sagatFilePath)))
            //if (!FileIsOpen(sagatFilePath))
            //{
            //    System.Diagnostics.Process.Start(sagatFilePath); // Open file containing SAGAT questions
            //}
            if (Input.GetKeyDown(KeyCode.P))
            {
                ResumeGame();
                pauseHistory[0] = true;
            }
        }
        if (CurrentCollectables == 1 && pauseHistory[1] == false)
        {
            PauseGame();
            if (Input.GetKeyDown(KeyCode.P))
            {
                ResumeGame();
                pauseHistory[1] = true;
            }
        }
        if (mins.Equals("1") && pauseHistory[2] == false)
        {
            PauseGame();
            if (Input.GetKeyDown(KeyCode.P))
            {
                ResumeGame();
                pauseHistory[2] = true;
            }
        }
    }
}
