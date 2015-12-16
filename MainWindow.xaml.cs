//---------------------------------------------------------------------------------------------------
// <copyright file="MainWindow.xaml.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//
// <Description>
// This program tracks up to 6 people simultaneously.
// If a person is tracked, the associated gesture detector will determine if that person is seated or not.
// If any of the 6 positions are not in use, the corresponding gesture detector(s) will be paused
// and the 'Not Tracked' image will be displayed in the UI.
// </Description>
//----------------------------------------------------------------------------------------------------

namespace Microsoft.Samples.Kinect.DiscreteGestureBasics
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using Microsoft.Kinect;
    using Microsoft.Kinect.VisualGestureBuilder;
    using SpeechLib;
    
    using System.Timers;
    /// <summary>
    /// Interaction logic for the MainWindow
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static Timer aTimer;
        static string ans;
        /// <summary> Active Kinect sensor </summary>
        private KinectSensor kinectSensor = null;
        
        /// <summary> Array for the bodies (Kinect will track up to 6 people simultaneously) </summary>
        private Body[] bodies = null;

        /// <summary> Reader for body frames </summary>
        private BodyFrameReader bodyFrameReader = null;

        /// <summary> Current status text to display </summary>
        private string statusText = null;

        /// <summary> KinectBodyView object which handles drawing the Kinect bodies to a View box in the UI </summary>
        private KinectBodyView kinectBodyView = null;
        
        /// x<summary> List of gesture detectors, there will be one detector created for each potential body (max of 6) </summary>
        private List<GestureDetector> gestureDetectorList = null;

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {

            

            // only one sensor is currently supported
            this.kinectSensor = KinectSensor.GetDefault();
            
            // set IsAvailableChanged event notifier
            this.kinectSensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;

            // open the sensor
            this.kinectSensor.Open();

            // set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.NoSensorStatusText;

            // open the reader for the body frames
            this.bodyFrameReader = this.kinectSensor.BodyFrameSource.OpenReader();

            // set the BodyFramedArrived event notifier
            this.bodyFrameReader.FrameArrived += this.Reader_BodyFrameArrived;

            // initialize the BodyViewer object for displaying tracked bodies in the UI
            this.kinectBodyView = new KinectBodyView(this.kinectSensor);

            // initialize the gesture detection objects for our gestures
            this.gestureDetectorList = new List<GestureDetector>();

            // initialize the MainWindow
            this.InitializeComponent();

            // set our data context objects for display in UI
            this.DataContext = this;
            this.kinectBodyViewbox.DataContext = this.kinectBodyView;

            // create a gesture detector for each body (6 bodies => 6 detectors) and create content controls to display results in the UI
            int col0Row = 0;
            int col1Row = 0;
            int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
            for (int i = 0; i < maxBodies; ++i)
            {
                GestureResultView result = new GestureResultView(i, false, false, 0.0f);
                GestureDetector detector = new GestureDetector(this.kinectSensor, result);
                this.gestureDetectorList.Add(detector);                
                
                // split gesture results across the first two columns of the content grid
                ContentControl contentControl = new ContentControl();
                contentControl.Content = this.gestureDetectorList[i].GestureResultView;
                
                if (i % 2 == 0)
                {
                    // Gesture results for bodies: 0, 2, 4
                    Grid.SetColumn(contentControl, 0);
                    Grid.SetRow(contentControl, col0Row);
                    ++col0Row;
                }
                else
                {
                    // Gesture results for bodies: 1, 3, 5
                    Grid.SetColumn(contentControl, 1);
                    Grid.SetRow(contentControl, col1Row);
                    ++col1Row;
                }

                this.contentGrid.Children.Add(contentControl);
                
                
            }
        }

        /// <summary>
        /// INotifyPropertyChangedPropertyChanged event to allow window controls to bind to changeable data
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the current status text to display
        /// </summary>
        /// 
        
        public void readinput()
        {
            int[] counter =new int[14] ;
            
            for (int i = 0; i < 14;i++ )
            {
                counter[i] = 0;
            }
                string Select = App.somedata;
                int max = 0;

            for (int i = 0; i < 100; i++)
            {
                max = 0;
                string prop=App.somedata;
                aTimer = new System.Timers.Timer(1);
                for (int j = 0; j < 14; j++)
                {
                    if (Category.words[j] == prop)
                    {
                        counter[j]++;
                    }
                    if(counter[j]>max)
                    {
                        max = counter[j];
                        Select = Category.words[j];
                    }
                }
            }
            
            { string s = Send_tag(Select); }
            
        }

        public string StatusText
        {
            get
            {
                return this.statusText;
            }

            set
            {
                if (this.statusText != value)
                {
                    this.statusText = value;

                    // notify any bound elements that the text has changed
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("StatusText"));
                    }
                }
            }
        }

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (this.bodyFrameReader != null)
            {
                // BodyFrameReader is IDisposable
                this.bodyFrameReader.FrameArrived -= this.Reader_BodyFrameArrived;
                this.bodyFrameReader.Dispose();
                this.bodyFrameReader = null;
            }

            if (this.gestureDetectorList != null)
            {
                // The GestureDetector contains disposable members (VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader)
                foreach (GestureDetector detector in this.gestureDetectorList)
                {
                    detector.Dispose();
                }

                this.gestureDetectorList.Clear();
                this.gestureDetectorList = null;
            }

            if (this.kinectSensor != null)
            {
                this.kinectSensor.IsAvailableChanged -= this.Sensor_IsAvailableChanged;
                this.kinectSensor.Close();
                this.kinectSensor = null;
            }
        }

        /// <summary>
        /// Handles the event when the sensor becomes unavailable (e.g. paused, closed, unplugged).
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            // on failure, set the status text
            this.StatusText = this.kinectSensor.IsAvailable ? Properties.Resources.RunningStatusText
                                                            : Properties.Resources.SensorNotAvailableStatusText;
        }

        /// <summary>
        /// Handles the body frame data arriving from the sensor and updates the associated gesture detector object for each body
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Reader_BodyFrameArrived(object sender, BodyFrameArrivedEventArgs e)
        {
            bool dataReceived = false;

            using (BodyFrame bodyFrame = e.FrameReference.AcquireFrame())
            {
                if (bodyFrame != null)
                {
                    if (this.bodies == null)
                    {
                        // creates an array of 6 bodies, which is the max number of bodies that Kinect can track simultaneously
                        this.bodies = new Body[bodyFrame.BodyCount];
                    }

                    // The first time GetAndRefreshBodyData is called, Kinect will allocate each Body in the array.
                    // As long as those body objects are not disposed and not set to null in the array,
                    // those body objects will be re-used.
                    bodyFrame.GetAndRefreshBodyData(this.bodies);
                    dataReceived = true;
                }
            }

            if (dataReceived)
            {
                // visualize the new body data
                this.kinectBodyView.UpdateBodyFrame(this.bodies);

                // we may have lost/acquired bodies, so update the corresponding gesture detectors
                if (this.bodies != null)
                {
                    // loop through all bodies to see if any of the gesture detectors need to be updated
                    int maxBodies = this.kinectSensor.BodyFrameSource.BodyCount;
                    for (int i = 0; i < maxBodies; ++i)
                    {
                        Body body = this.bodies[i];
                        ulong trackingId = body.TrackingId;

                        // if the current body TrackingId changed, update the corresponding gesture detector with the new value
                        if (trackingId != this.gestureDetectorList[i].TrackingId)
                        {
                            this.gestureDetectorList[i].TrackingId = trackingId;

                            // if the current body is tracked, unpause its detector to get VisualGestureBuilderFrameArrived events
                            // if the current body is not tracked, pause its detector so we don't waste resources trying to get invalid gesture results
                            this.gestureDetectorList[i].IsPaused = trackingId == 0;
                        }
                    }
                }
            }
        }

        public bool findmyelement(string[] tag_array, string s, int len, int i)
        {

            {
                for (i = 0; i < len; i++)
                    if (tag_array[i] == s)
                        return true;
            }
            return false;
        }
        public string form_sentence(string[] tag_array,int len)
        {
            string name_1 = "Mridul";
            string foroutput = "Help Us Improve.";
            
            {
                if (findmyelement(tag_array, "when", len, 0) == true && findmyelement(tag_array, "game", len, 0) == true)
                {
                    if (findmyelement(tag_array, "your", len, 0) == false)
                        foroutput = "When is your game?";
                    else if (findmyelement(tag_array, "my", len, 0) == true)
                    {
                        foroutput = "When is my game?";
                    }
                    else
                        foroutput = "When is the game?";
                    return foroutput;
                }
                if (findmyelement(tag_array, "i", len, 0) == true)
                {
                    if (findmyelement(tag_array, "play", len, 0) == true)
                    {
                        foroutput = "I am playing.";
                        if (findmyelement(tag_array, "game", len, 0) == true)
                        {
                            foroutput = "I am playing game.";
                            if (findmyelement(tag_array, "you", len, 0) == true)
                            {
                                foroutput = "Will you play a game with me?";
                            }
                            }
                            if (findmyelement(tag_array, "you", len, 0) == true)
                            {
                                foroutput = "Will you play with me?";
                            }
                        }


                        else if (findmyelement(tag_array, "water", len, 0) == true)
                        {
                            foroutput = "I need water.";
                        }
                        else if (findmyelement(tag_array, "why", len, 0))
                        {
                            foroutput = "Why me?";
                        }
                        else foroutput = "I am " + name_1;
                        return foroutput;
                }
                if (findmyelement(tag_array, "play", len, 0) == true)
                {
                    if (findmyelement(tag_array, "your", len, 0) == true)
                    {
                        if (findmyelement(tag_array, "game", len, 0) == true)
                            foroutput = "Play your own game.";
                    }
                    return foroutput;
                }
                if (findmyelement(tag_array, "you", len, 0) == true)
                {
                    if (findmyelement(tag_array, "water", len, 0) == true)
                    {
                        foroutput = "Do you need water?";
                    }
                    else if (findmyelement(tag_array, "play", len, 0) == true)
                    {
                        foroutput = "Do you want to play?";
                        if (findmyelement(tag_array, "game", len, 0) == true)
                        {
                            foroutput = "Do you want to play a game?";
                        }
                        else if (findmyelement(tag_array, "why", len, 0) == true)
                        {
                            foroutput = "Why are you playing?";
                        }
                    }
                    else if (findmyelement(tag_array, "who", len, 0) == true)
                    {
                        foroutput = "Who are you?";
                    }
                    else if (findmyelement(tag_array, "how", len, 0) == true)
                    {
                        foroutput = "how are you?";
                    }
                    else if (findmyelement(tag_array, "where", len, 0) == true)
                    {
                        foroutput = "where are you?";
                    }
                    return foroutput;
                }
                if(findmyelement(tag_array,"where",len,0))
                {
                    foroutput = "where are you?";
                    if(findmyelement(tag_array,"water",len,0))
                    {
                        foroutput = "Where is water?";
                    }
                    return foroutput;
                }
                if(findmyelement(tag_array,"my",len,0))
                {
                    if(findmyelement(tag_array,"name",len,0))
                    {foroutput="My name is "+ name_1;}
                }
                if (findmyelement(tag_array, "hi", len, 0) == true)
                {
                    foroutput = "Hi";
                }

            }
            return foroutput;
        }
        //sentence structure= noun/pronoun+verb

        public string Send_tag(string new_tag)
        {

            string s = TagClass.tag.ToString();
            string[] arr = s.Split(' ');
            
            if (findmyelement(arr, new_tag, arr.Length, 0) == false)
            {
                TagClass.tag += new_tag;
                TagClass.tag += " ";

            }
            Display_Tags.Text = TagClass.tag;
            string name_1 = "Mridul";
            
            

            return name_1;

        }

        private void Controller_Click(object sender, RoutedEventArgs e)
        {
            if(Controller.Content=="Play")
            {
                Controller.Content = "Stop";
                //Category s = new Category();

                Send_tag(App.somedata);
                


            }
            else
            {
                Category tresert = new Category();

                Controller.Content = "Play";
                Send_tag(App.somedata);
            }
        }

        private void The_voice_Click(object sender, RoutedEventArgs e)
        {
            
            string outputresult;
            string tag_arry = TagClass.tag;
            string[] tag_array = tag_arry.Split(' ');
            int len = tag_array.Length;
            outputresult = form_sentence(tag_array, len);

            Display.Text = outputresult;
            SpVoice voice1 = new SpVoice();
            voice1.Speak(outputresult, SpeechVoiceSpeakFlags.SVSFDefault);
            TagClass.tag = "";
        }

        

        
    }
}
