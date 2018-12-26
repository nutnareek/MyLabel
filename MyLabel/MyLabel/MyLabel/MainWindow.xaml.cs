using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace MyLabel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Dictionary<string, ActionType> ActionDict = new Dictionary<string, ActionType>()
        {
            {"G", ActionType.ACTION1},
            {"H", ActionType.ACTION2},
            {"J", ActionType.ACTION3},
            {"K", ActionType.ACTION4},
            {"L", ActionType.ACTION5},
            {"B", ActionType.ACTION6},
            {"N", ActionType.ACTION7 }
        };

        Dictionary<string, PoseType> PoseDict = new Dictionary<string, PoseType>()
        {
            {"A", PoseType.Pose1},
            {"Z", PoseType.Pose2},
            {"S", PoseType.Pose3},
            {"X", PoseType.Pose4},
            {"D", PoseType.Pose5},
            {"C", PoseType.Pose6},
            {"F", PoseType.Pose7},
            {"V", PoseType.Pose8}

          };


        // Video Variables
        private bool userIsDraggingSlider = false;
        MediaPlayerState playerState = MediaPlayerState.Stopping;
        double SliderWidth;
        DispatcherTimer timer;
        double offset = 0.00;

        // Labeling Variable
        double minLabelItemWidth = 5;
        Key pressedKeyPose;
        Key pressedKeyAction;
        bool ISDRAGGINGRIGHT = false;
        bool ISDRAGGINGLEFT = false;
        BehaviorType isDragType = BehaviorType.None;
        
        bool LABELINGPOSE = false;
        LabelItem currPoseLabelItem = null;
        Button currPoseLabelBtn = null;

        bool LABELINGACTION = false;
        Button currActionLabelBtn = null;
        LabelItem currActionLabelItem = null;

        int idCount = 0;
        List<LabelItem> poseLabels = null;
        List<LabelItem> actionLabels = null;

        // Edit Label Variables
        bool EDITED_TAG_MODE = false;

        #region Window Load & Main Menu
        public MainWindow()
        {
            InitializeComponent();

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += timer_Tick;

            poseLabels = new List<LabelItem>();
            actionLabels = new List<LabelItem>();

        }

        void timer_Tick(object sender, EventArgs e)
        {
            if ((mePlayer.Source != null) && (mePlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider) && (playerState == MediaPlayerState.Playing))
            {
                slrProgress.Minimum = 0;
                slrProgress.Maximum = mePlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                slrProgress.Value = mePlayer.Position.TotalMilliseconds;
            }
        }
        private void BtnOpenInstruction_Click(object sender, RoutedEventArgs e)
        {
            InstructionWindow win = new InstructionWindow();
            win.ShowDialog();
        }

        private void BtnLoadVid_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mkv;*.mp4;*.avi;*.mpeg;*.wmv)|*.mkv;*.mp4;*.avi;*.mpeg;*.wmv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                mePlayer.Source = new Uri(openFileDialog.FileName);
                mePlayer.Play();
                mePlayer.Stop();
            }

        }
        private void BtnLoadTrimVid_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Media files (*.mp4;*.avi;*.mpeg;*.wmv)|*.mp4;*.avi;*.mpeg;*.wmv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {

                VideoEditorWindow win = new VideoEditorWindow(new Uri(openFileDialog.FileName));
                Console.WriteLine(win.ShowDialog());
            
                mePlayer.Source = win.VideoPath;
            }

        }

        private Thumb Thumb
        {
            get
            {
                return GetThumb(this /* the slider */ ) as Thumb; ;
            }
        }

        private DependencyObject GetThumb(DependencyObject root)
        {
            if (root is Thumb)
                return root;

            DependencyObject thumb = null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(root); i++)
            {
                thumb = GetThumb(VisualTreeHelper.GetChild(root, i));

                if (thumb is Thumb)
                    return thumb;
            }

            return thumb;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SliderWidth = Thumb.Width;
            CvsLabel.Width = slrProgress.ActualWidth - SliderWidth;
            CvsTrackProgress.Width = slrProgress.ActualWidth - SliderWidth;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SliderWidth = Thumb.Width;
            CvsLabel.Width = slrProgress.ActualWidth - SliderWidth;
            CvsTrackProgress.Width = slrProgress.ActualWidth - SliderWidth;
            if (poseLabels.Count > 0)
            {

            }
        }

        #endregion

        #region Video Player

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
   

            if(playerState == MediaPlayerState.Stopping)
            {
                timer.Start();
                playerState = MediaPlayerState.Playing;
                mePlayer.Play();
            } else if(playerState == MediaPlayerState.Pausing)
            {
                mePlayer.Play();
                playerState = MediaPlayerState.Playing;
            } else // playerState == MediaPlayerState.Playing
            {
                
            }
   
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            mePlayer.Pause();
            playerState = MediaPlayerState.Pausing;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if(playerState == MediaPlayerState.Playing| playerState == MediaPlayerState.Pausing)
            {
         
                mePlayer.Stop();
                slrProgress.Value = slrProgress.Minimum;
                playerState = MediaPlayerState.Stopping;
                timer.Stop();
            }
        }

        // A method to show the play position
        private void ShowPosition()
        {
            double totalPlayTime = mePlayer.Position.TotalSeconds;
        }
        #endregion

        #region Progress Slider
        private void slrProgress_DragCompleted(object sender, RoutedEventArgs e)
        {
            userIsDraggingSlider = false;
            mePlayer.Position = TimeSpan.FromMilliseconds(slrProgress.Value);
            if(playerState == MediaPlayerState.Playing)
            {
                mePlayer.Play();
            }
        }

        private void slrProgress_DragStarted(object sender, RoutedEventArgs e)
        {
            if(playerState == MediaPlayerState.Playing)
            {
                mePlayer.Pause();
            }
            userIsDraggingSlider = true;
        }

        private void slrProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            if ((currPoseLabelBtn != null)||(currActionLabelItem!=null))
            {
                
                if (ISDRAGGINGLEFT && (isDragType!=BehaviorType.None))
                {

                    if (isDragType == BehaviorType.Pose)
                    {
                        if (((currPoseLabelItem.EndTime - slrProgress.Value) / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth) > minLabelItemWidth)
                        {
                            
                            lblStatus.Content = TimeSpan.FromMilliseconds(offset + slrProgress.Value).ToString(@"hh\:mm\:ss\.fff");
                            Canvas.SetLeft(TrackLine, slrProgress.Value / slrProgress.Maximum * TrackPose.Width);
                            mePlayer.Position = TimeSpan.FromMilliseconds(slrProgress.Value);
                            currPoseLabelItem.StartTime = slrProgress.Value;
                        }

                        UpdateCurrLabelBtnLeftDrag();
                    }
                    else //isDragType == BehaviorType.Action
                    {
                        if (((currActionLabelItem.EndTime - slrProgress.Value) / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth) > minLabelItemWidth)
                        {
                            lblStatus.Content = TimeSpan.FromMilliseconds(offset +slrProgress.Value).ToString(@"hh\:mm\:ss\.fff");
                            Canvas.SetLeft(TrackLine, slrProgress.Value / slrProgress.Maximum * TrackAction.Width);
                            mePlayer.Position = TimeSpan.FromMilliseconds(slrProgress.Value);
                            currActionLabelItem.StartTime = slrProgress.Value;
                        }
                        UpdateActionCurrLabelBtnLeftDrag();
                    }
                }
                else if(ISDRAGGINGRIGHT && (isDragType != BehaviorType.None))// ISDRAGRIGHT
                {

                    if (isDragType == BehaviorType.Pose)
                    {
                        if (((slrProgress.Value - currPoseLabelItem.StartTime) / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth) > minLabelItemWidth)
                        {
                            lblStatus.Content = TimeSpan.FromMilliseconds(offset + slrProgress.Value).ToString(@"hh\:mm\:ss\.fff");
                            Canvas.SetLeft(TrackLine, slrProgress.Value / slrProgress.Maximum * TrackPose.Width);
                            mePlayer.Position = TimeSpan.FromMilliseconds(slrProgress.Value);
                            currPoseLabelItem.EndTime = slrProgress.Value;
                        }
                        UpdateCurrLabelBtn();
                    }
                    else //isDragType == BehaviorType.Action
                    {
                        if (((slrProgress.Value - currActionLabelItem.StartTime) / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth) > minLabelItemWidth)
                        {
                            lblStatus.Content = TimeSpan.FromMilliseconds(offset + slrProgress.Value).ToString(@"hh\:mm\:ss\.fff");
                            Canvas.SetLeft(TrackLine, slrProgress.Value / slrProgress.Maximum * TrackAction.Width);
                            mePlayer.Position = TimeSpan.FromMilliseconds(slrProgress.Value);
                            currActionLabelItem.EndTime = slrProgress.Value;
                        }
                        UpdateActionCurrLabelBtn();
                    }
                }
                else
                {
                    lblStatus.Content = TimeSpan.FromMilliseconds(offset + slrProgress.Value).ToString(@"hh\:mm\:ss\.fff");
                    Canvas.SetLeft(TrackLine, slrProgress.Value / slrProgress.Maximum * TrackPose.Width);
                    mePlayer.Position = TimeSpan.FromMilliseconds(slrProgress.Value);
                    if (currPoseLabelBtn != null)
                    {
                        currPoseLabelItem.EndTime = slrProgress.Value;
                        UpdateCurrLabelBtn();
                    }
                    if (currActionLabelBtn != null)
                    {
                        currActionLabelItem.EndTime = slrProgress.Value;
                        UpdateActionCurrLabelBtn();
                    }
                }

            }
            else
            {
                lblStatus.Content = TimeSpan.FromMilliseconds(offset + slrProgress.Value).ToString(@"hh\:mm\:ss\.fff");
                Canvas.SetLeft(TrackLine, slrProgress.Value / slrProgress.Maximum * TrackPose.Width);
                mePlayer.Position = TimeSpan.FromMilliseconds(slrProgress.Value);
            }
        }

        #endregion

        #region Label Input >> Keyboard
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.A | e.Key == Key.Z | e.Key == Key.S | e.Key == Key.X | e.Key == Key.D | e.Key == Key.V | e.Key == Key.C | e.Key == Key.F)
            {
                if ((!LABELINGPOSE) && (pressedKeyPose == Key.None))
                {

                    LABELINGPOSE = true;
                    currPoseLabelBtn = new Button();
                    currPoseLabelItem = new LabelItem(slrProgress.Value, PoseDict[e.Key.ToString()].ToString(), idCount, BehaviorType.Pose);
                    idCount++;
  
                    pressedKeyPose = e.Key;
                    AddNewLabel(currPoseLabelItem.BehaviorType);
                }
            }

            if (e.Key == Key.G | e.Key == Key.H | e.Key == Key.J | e.Key == Key.K | e.Key == Key.L | e.Key == Key.B | e.Key == Key.N )
            {
                
                if ((!LABELINGACTION) && (pressedKeyAction == Key.None))
                {
                    LABELINGACTION = true;
                    currActionLabelBtn = new Button();

                    currActionLabelItem = new LabelItem(slrProgress.Value, ActionDict[e.Key.ToString()].ToString(), idCount, BehaviorType.Action);
                    idCount++;
                    pressedKeyAction = e.Key;
                    AddNewLabel(currActionLabelItem.BehaviorType);
                }
            }
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.A | e.Key == Key.Z | e.Key == Key.S | e.Key == Key.X | e.Key == Key.D | e.Key == Key.V | e.Key == Key.C | e.Key == Key.F) && (e.Key == pressedKeyPose))
            {
                pressedKeyPose = Key.None;
                LABELINGPOSE = false;
                currPoseLabelItem.EndTime = slrProgress.Value;
                UpdateCurrLabelBtn();
                FindPoseFullyIntersectedLabel();
                if(currPoseLabelBtn.Width < minLabelItemWidth)
                {
                    Console.WriteLine("Remov");
                    CvsLabel.Children.Remove(currPoseLabelBtn);
                    poseLabels.Remove(currPoseLabelItem);
                }
                currPoseLabelItem = null;
                currPoseLabelBtn = null;

            }

            if ((e.Key == Key.G | e.Key == Key.H | e.Key == Key.J | e.Key == Key.K | e.Key == Key.L | e.Key == Key.B | e.Key == Key.N) && (e.Key == pressedKeyAction))
            {
                pressedKeyAction = Key.None;
                LABELINGACTION = false;
                currActionLabelItem.EndTime = slrProgress.Value;
                UpdateActionCurrLabelBtn();
                FindActionFullyIntersectedLabel();
                if(currActionLabelBtn.Width < minLabelItemWidth)
                {
                    CvsLabel.Children.Remove(currActionLabelBtn);
                    actionLabels.Remove(currActionLabelItem);
                }
                currActionLabelItem = null;
                currActionLabelBtn = null;

            }
        }
        #endregion


        #region Mouse Button Events
        // Changing Size of Tag Labels
        private void NewLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((ISDRAGGINGRIGHT == true) || (ISDRAGGINGLEFT == true) && (isDragType != BehaviorType.None))
            {
                if (isDragType == BehaviorType.Pose)
                {
                    FindPoseFullyIntersectedLabel();
                    currPoseLabelItem = null;
                    currPoseLabelBtn = null;
                }
                else // isDragType == BehaviorType.Action
                {
                    FindActionFullyIntersectedLabel();
                    currActionLabelItem = null;
                    currActionLabelBtn = null;
                }
                ISDRAGGINGRIGHT = false;
                ISDRAGGINGLEFT = false;
                isDragType = BehaviorType.None;
            }

        }

        private void NewLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!EDITED_TAG_MODE)
            {
                Button btn = (Button)sender;
                int idx = -1;
                BehaviorType currBtnType = FindItemType((int)btn.Tag, out idx);
                if (currBtnType == BehaviorType.Pose)
                {
                    //if ((idx != -1))
                    {
                        if (poseLabels[idx].Status == TagStatus.Inactive)
                        {
                            poseLabels[idx].Status = TagStatus.Active;
                            btn.Background = new SolidColorBrush(Colors.DeepPink);
                            btn.Foreground = new SolidColorBrush(Colors.White);
                        }
                        else
                        {
                            poseLabels[idx].Status = TagStatus.Inactive;
                            btn.Background = new SolidColorBrush(Color.FromRgb((byte)243, (byte)164, (byte)255));
                            btn.Foreground = new SolidColorBrush(Colors.Black);
                        }

                    }
                }
                else if (currBtnType == BehaviorType.Action)
                {
                    //if ((idx != -1))
                    {
                        if (actionLabels[idx].Status == TagStatus.Inactive)
                        {
                            actionLabels[idx].Status = TagStatus.Active;
                            btn.Background = new SolidColorBrush(Colors.SteelBlue);
                            btn.Foreground = new SolidColorBrush(Colors.White);
                        }
                        else
                        {
                            actionLabels[idx].Status = TagStatus.Inactive;
                            btn.Background = new SolidColorBrush(Colors.SkyBlue);
                            btn.Foreground = new SolidColorBrush(Colors.Black);
                        }

                    }
                }
                else
                {
                    // Do nothing
                }
                if (CountActiveLabels() > 0)
                {
                    btnEditTag.IsEnabled = true;
                    btnSaveEditTag.IsEnabled = true;

                }
                else
                {
                    btnEditTag.IsEnabled = false;
                    btnSaveEditTag.IsEnabled = false;
                }
            }

        }

        private int CountActiveLabels()
        {
            int[] activePoses = null, activeAction = null;
            return FindActiveLabels(out activePoses, out activeAction);
        }

        private int FindActiveLabels(out int[] idxActivePoses, out int[] idxActiveAction)
        {
            int count = 0;
            List<int> idx = new List<int>();
            if (poseLabels != null)
            {
                if (poseLabels.Count > 0)
                {
                    for (int i = 0; i < poseLabels.Count; i++)
                        if (poseLabels[i].Status == TagStatus.Active)
                            idx.Add(i);
                    if (idx.Count > 0)
                    {
                        idxActivePoses = idx.ToArray();
                        count += idxActivePoses.Length;
                    }
                    else
                        idxActivePoses = null;
                }
                else { idxActivePoses = null; }
            }
            else
            {
                idxActivePoses = null;
            }

            idx = new List<int>();
            if(actionLabels != null)
            {
                if (actionLabels.Count > 0)
                {
                    for (int i = 0; i < actionLabels.Count; i++)
                        if (actionLabels[i].Status == TagStatus.Active)
                        {
                            idx.Add(i);
                        }
                    if (idx.Count > 0)
                    {
                        idxActiveAction = idx.ToArray();
                        count += idxActiveAction.Length;
                    }
                    else
                        idxActiveAction = null;
                } else
                {
                    idxActiveAction = null;
                }
            } else
            {
                idxActiveAction = null;
            }

            return count;
        }

        private void LabelObject_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((ISDRAGGINGRIGHT == true) || (ISDRAGGINGLEFT == true) && (isDragType != BehaviorType.None))
            {
                if (isDragType == BehaviorType.Pose)
                {
                    FindPoseFullyIntersectedLabel();
                    currPoseLabelItem = null;
                    currPoseLabelBtn = null;
                }
                else // isDragType == BehaviorType.Action
                {
                    FindActionFullyIntersectedLabel();
                    currActionLabelItem = null;
                    currActionLabelBtn = null;
                }
                ISDRAGGINGRIGHT = false;
                ISDRAGGINGLEFT = false;
                isDragType = BehaviorType.None;
            }
        }

        BehaviorType FindItemType(int id)
        {
            if (poseLabels.Count > 0)
            {
                for(int i = 0; i<poseLabels.Count; i++)
                {
                    if (poseLabels[i].ID == id)
                        return BehaviorType.Pose;
                }
            }

            if (actionLabels.Count > 0)
            {
                for(int i = 0; i<actionLabels.Count; i++)
                {
                    if (actionLabels[i].ID == id)
                        return BehaviorType.Action;
                }
            }

            return BehaviorType.None;
        }

        BehaviorType FindItemType(int id, out int idx)
        {
            if (poseLabels.Count > 0)
            {
                for (int i = 0; i < poseLabels.Count; i++)
                {
                    if (poseLabels[i].ID == id)
                    {
                        idx = i;
                        return BehaviorType.Pose;
                    }
                }
            }

            if (actionLabels.Count > 0)
            {
                for (int i = 0; i < actionLabels.Count; i++)
                {
                    if (actionLabels[i].ID == id)
                    {
                        idx = i;
                        return BehaviorType.Action;
                    }
                }
            }
            idx = -1;
            return BehaviorType.None;
        }

        private void LabelObject_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.RightButton == MouseButtonState.Pressed)
            {
                Button b = (Button)sender;
                Point p = e.GetPosition(b);

                int idx;
                isDragType = FindItemType((int)b.Tag, out idx);
                if ((p.X < 3) && (isDragType != BehaviorType.None))
                {
                    //Console.WriteLine("Drag Left");
                    if (isDragType == BehaviorType.Pose)
                    {
                        currPoseLabelItem = poseLabels[idx];
                        slrProgress.Value = currPoseLabelItem.StartTime;
                        currPoseLabelBtn = currPoseLabelItem.Button;
                    } else //(isDragType== BehaviorType.Action)
                    {
                        currActionLabelItem = actionLabels[idx];
                        slrProgress.Value = currActionLabelItem.StartTime;
                        currActionLabelBtn = currActionLabelItem.Button;
                    } 
                    ISDRAGGINGLEFT = true;
                } else if(((b.Width - p.X) < 3) && (isDragType != BehaviorType.None))
                {
                    //Console.WriteLine("Drag Right");
                    if (isDragType == BehaviorType.Pose)
                    {
                        currPoseLabelItem = poseLabels[FindPoseMatchingID((int)b.Tag)];
                        slrProgress.Value = currPoseLabelItem.EndTime;
                        currPoseLabelBtn = currPoseLabelItem.Button;
                    }
                    else//(isDragType== BehaviorType.Action)
                    {
                        currActionLabelItem = actionLabels[idx];
                        slrProgress.Value = currActionLabelItem.EndTime;
                        currActionLabelBtn = currActionLabelItem.Button;
                    }
                    ISDRAGGINGRIGHT = true;
                } else
                {

                }

            }
  
        }

        private void LabelObject_MouseMove(object sender, MouseEventArgs e)
        {
            
            Button b = (Button)sender;
            Point p = e.GetPosition(b);

            if ((p.X < 3) || ((b.Width - p.X) < 3))
            {              
                this.Cursor = ((TextBlock)this.Resources["CursorHorizontalAdjustment"]).Cursor;
               
            } else
            {
                this.Cursor = Cursors.Arrow;
            }

            if (e.RightButton == MouseButtonState.Pressed && (ISDRAGGINGLEFT||ISDRAGGINGRIGHT))
            {
                p = e.GetPosition(CvsLabel);

                double newSlrProVal = (p.X / CvsLabel.ActualWidth * (slrProgress.Maximum - slrProgress.Minimum));

                if (isDragType != BehaviorType.None)
                {
                    if (ISDRAGGINGRIGHT && !ISDRAGGINGLEFT)
                    {
                                          
                        if (currPoseLabelBtn != null) {
                            if ((((newSlrProVal - currPoseLabelItem.StartTime) / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth) > 5) && (isDragType == BehaviorType.Pose))
                                slrProgress.Value = newSlrProVal;
                        }

                        if (currActionLabelBtn != null)
                        {
                            if ((((newSlrProVal - currActionLabelItem.StartTime) / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth) > 5) && (isDragType == BehaviorType.Action))
                                slrProgress.Value = newSlrProVal;
                        }
                    }
                    else if (ISDRAGGINGLEFT && !ISDRAGGINGRIGHT)
                    {
                        if (currPoseLabelBtn != null)
                        {
                            if ((((currPoseLabelItem.EndTime - newSlrProVal) / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth) > 5) && (isDragType == BehaviorType.Pose))
                                slrProgress.Value = newSlrProVal;
                        }
                        if (currActionLabelBtn != null)
                        {
                            if ((((currActionLabelItem.EndTime - newSlrProVal) / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth) > 5) && (isDragType == BehaviorType.Action))
                                slrProgress.Value = newSlrProVal;
                        }
                    }
                    else
                    {

                    }
                }
            }
            else if (e.RightButton == MouseButtonState.Released && (ISDRAGGINGRIGHT || ISDRAGGINGLEFT))
            {
                if ((ISDRAGGINGRIGHT == true) || (ISDRAGGINGLEFT == true) && (isDragType!= BehaviorType.None))
                {
                    if (isDragType == BehaviorType.Pose)
                    {
                        FindPoseFullyIntersectedLabel();
                        currPoseLabelItem = null;
                        currPoseLabelBtn = null;
                    }
                    else // isDragType == BehaviorType.Action
                    {
                        FindActionFullyIntersectedLabel();
                        currActionLabelItem = null;
                        currActionLabelBtn = null;
                    }
                    ISDRAGGINGRIGHT = false;
                    ISDRAGGINGLEFT = false;
                    isDragType = BehaviorType.None;
                }

            }
            else
            {

            }

        }

        private void LabelObject_MouseLeave(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }
        #endregion

        #region Label Object Add/Remove/Edit
        void AddNewLabel(BehaviorType type)
        {
            if (type == BehaviorType.Pose) // is Pose Behavior
            {
                // Create new Label Button
                Button newLabel = new Button();
                newLabel.Background = new SolidColorBrush(Color.FromRgb((byte)243, (byte)164, (byte)255));
                newLabel.Height = TrackPose.Height;
                newLabel.Width = 1;
                newLabel.Content = currPoseLabelItem.Label;

                // Create new Events
                newLabel.MouseDoubleClick += NewLabel_MouseDoubleClick;
                newLabel.MouseLeave += LabelObject_MouseLeave;
                newLabel.MouseMove += LabelObject_MouseMove;
                newLabel.PreviewMouseDown += LabelObject_PreviewMouseDown;
                newLabel.MouseRightButtonUp += LabelObject_PreviewMouseRightButtonUp;
                newLabel.MouseUp += NewLabel_MouseUp;


                CvsLabel.Children.Add(newLabel);
                Canvas.SetLeft(newLabel, currPoseLabelItem.StartTime / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth);
                currPoseLabelBtn = newLabel;
                newLabel = null;
                currPoseLabelItem.EndTime = slrProgress.Value;

                currPoseLabelItem.Button = currPoseLabelBtn;
                poseLabels.Add(currPoseLabelItem);
            }
            else // is Action Behavior
            {
                // Create new Label Button
                Button newLabel = new Button();
                newLabel.Background = new SolidColorBrush(Colors.SkyBlue);
                newLabel.Height = TrackAction.Height;
                newLabel.Width = 1;
                newLabel.Content = currActionLabelItem.Label;

                // Create new Events
                newLabel.MouseDoubleClick += NewLabel_MouseDoubleClick;
                newLabel.MouseLeave += LabelObject_MouseLeave;
                newLabel.MouseMove += LabelObject_MouseMove;
                newLabel.PreviewMouseDown += LabelObject_PreviewMouseDown;
                newLabel.MouseRightButtonUp += LabelObject_PreviewMouseRightButtonUp;
                newLabel.MouseUp += NewLabel_MouseUp;


                CvsLabel.Children.Add(newLabel);
                Canvas.SetLeft(newLabel, currActionLabelItem.StartTime / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth);
                Canvas.SetTop(newLabel, TrackPose.Height);
                currActionLabelBtn = newLabel;
                newLabel = null;
                currActionLabelItem.EndTime = slrProgress.Value;

                currActionLabelItem.Button = currActionLabelBtn;
                actionLabels.Add(currActionLabelItem);
            }
        }

        void LoadGroudtruthLabel(int id, BehaviorType type, string Label, double starttime, double endtime)
        {
            idCount = 0;
            LabelItem newLoadLabelItem = new LabelItem(starttime, Label, id, type);

            if (type == BehaviorType.Pose) // is Pose Behavior
            {
                // Create new Label Button
                Button newLabel = new Button();
                newLabel.Background = new SolidColorBrush(Color.FromRgb((byte)243, (byte)164, (byte)255));
                newLabel.Height = TrackPose.Height;
                newLabel.Width = (endtime - starttime) / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth;
                newLabel.Content = Label;
                newLabel.Tag = id;

                // Create new Events
                newLabel.MouseDoubleClick += NewLabel_MouseDoubleClick;
                newLabel.MouseLeave += LabelObject_MouseLeave;
                newLabel.MouseMove += LabelObject_MouseMove;
                newLabel.PreviewMouseDown += LabelObject_PreviewMouseDown;
                newLabel.MouseRightButtonUp += LabelObject_PreviewMouseRightButtonUp;
                newLabel.MouseUp += NewLabel_MouseUp;

                CvsLabel.Children.Add(newLabel);
                Canvas.SetLeft(newLabel, starttime / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth);
                newLoadLabelItem.EndTime = endtime;

                newLoadLabelItem.Button = newLabel;
                poseLabels.Add(newLoadLabelItem);
            }
            else // is Action Behavior
            {
                // Create new Label Button
                Button newLabel = new Button();
                newLabel.Background = new SolidColorBrush(Colors.SkyBlue);
                newLabel.Height = TrackAction.Height;
                newLabel.Width = (endtime - starttime) / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth;
                newLabel.Content = Label;
                newLabel.Tag = id;

                // Create new Events
                newLabel.MouseDoubleClick += NewLabel_MouseDoubleClick;
                newLabel.MouseLeave += LabelObject_MouseLeave;
                newLabel.MouseMove += LabelObject_MouseMove;
                newLabel.PreviewMouseDown += LabelObject_PreviewMouseDown;
                newLabel.MouseRightButtonUp += LabelObject_PreviewMouseRightButtonUp;
                newLabel.MouseUp += NewLabel_MouseUp;


                CvsLabel.Children.Add(newLabel);
                Canvas.SetLeft(newLabel, starttime/ (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth);
                Canvas.SetTop(newLabel, TrackPose.Height);
                newLoadLabelItem.EndTime = slrProgress.Value;

                newLoadLabelItem.Button = newLabel;
                actionLabels.Add(newLoadLabelItem);
            }
        }

        int FindPoseMatchingID(int ID)
        {
            if (poseLabels.Count > 0)
            {
                for (int i = 0; i < poseLabels.Count; i++)
                {
                    if (poseLabels[i].ID == ID)
                        return i;
                }
            }
            return -1;
        }

        void UpdateCurrLabelBtn()
        {
            //Console.WriteLine("Update");
            if (currPoseLabelItem.Duration() > 0)
            {

                currPoseLabelBtn.Width = (currPoseLabelItem.Duration() / (slrProgress.Maximum - slrProgress.Minimum)) * TrackPose.ActualWidth;
               
                int intersected = FindPoseIntersectedLabel(currPoseLabelItem.EndTime);

                if (intersected != -1)
                {
                    LabelItem intersectedItem = poseLabels[intersected];
                    intersectedItem.StartTime = currPoseLabelItem.EndTime;
                    intersectedItem.Button.Width = (intersectedItem.Duration() / (slrProgress.Maximum - slrProgress.Minimum)) * TrackPose.ActualWidth;
                    Canvas.SetLeft(intersectedItem.Button, intersectedItem.StartTime / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth);
                }
                
            }
        }

        void UpdateCurrLabelBtnLeftDrag()
        {
            if (currPoseLabelItem.Duration() > 0)
            {
                
                currPoseLabelBtn.Width = (currPoseLabelItem.Duration() / (slrProgress.Maximum - slrProgress.Minimum)) * TrackPose.ActualWidth;

                Canvas.SetLeft(currPoseLabelBtn, currPoseLabelItem.StartTime / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth);


                int intersected = FindPoseIntersectedLabel(currPoseLabelItem.StartTime);

                if (intersected != -1)
                {
                    LabelItem intersectedItem = poseLabels[intersected];
                    intersectedItem.EndTime = currPoseLabelItem.StartTime;
                    intersectedItem.Button.Width = (intersectedItem.Duration() / (slrProgress.Maximum - slrProgress.Minimum)) * TrackPose.ActualWidth;
                    Canvas.SetLeft(intersectedItem.Button, intersectedItem.StartTime / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth);
                }

            }
        }

        int FindPoseIntersectedLabel(double curTime)
        {
            if(poseLabels.Count == 0)
            {
                return -1;
            }
            else
            {
                for(int i = 0; i<poseLabels.Count; i++)
                {
                   
                    if (poseLabels[i].StartTime <= curTime && curTime <= poseLabels[i].EndTime && poseLabels[i].ID != currPoseLabelItem.ID)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        int FindPoseFullyIntersectedLabel()
        {
            if (poseLabels.Count == 0)
            {
                return -1;
            }
            else
            {
                List<int> removeIdx = new List<int>();
                for (int i = 0; i < poseLabels.Count; i++)
                {
                    
                    if ((currPoseLabelItem.StartTime < poseLabels[i].StartTime) && (poseLabels[i].EndTime < currPoseLabelItem.EndTime) && (poseLabels[i].ID != currPoseLabelItem.ID))
                    {
           
                        removeIdx.Add(i);
                    }
                }
                if (removeIdx.Count > 0)
                {
                    for (int i = removeIdx.Count - 1; i >= 0; i--)
                    {
                        CvsLabel.Children.Remove(poseLabels[removeIdx[i]].Button);
                        poseLabels.RemoveAt(removeIdx[i]);
                    }
                }

            }
            return -1;
        }
        #endregion

        #region Action Label Object Add/Remove/Edit
       

        int FindActionMatchingID(int ID)
        {
            if (actionLabels.Count > 0)
            {
                for (int i = 0; i < actionLabels.Count; i++)
                {
                    if (actionLabels[i].ID == ID)
                        return i;
                }
            }
            return -1;
        }

        void UpdateActionCurrLabelBtn()
        {

            if (currActionLabelItem.Duration() > 0)
            {

                currActionLabelBtn.Width = (currActionLabelItem.Duration() / (slrProgress.Maximum - slrProgress.Minimum)) * TrackAction.ActualWidth;

                int intersected = FindActionIntersectedLabel(currActionLabelItem.EndTime);

                if (intersected != -1)
                {
                    LabelItem intersectedItem = actionLabels[intersected];
                    intersectedItem.StartTime = currActionLabelItem.EndTime;
                    intersectedItem.Button.Width = (intersectedItem.Duration() / (slrProgress.Maximum - slrProgress.Minimum)) * TrackAction.ActualWidth;
                    Canvas.SetLeft(intersectedItem.Button, intersectedItem.StartTime / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth);
                }

            }
        }

        void UpdateActionCurrLabelBtnLeftDrag()
        {
            if (currActionLabelItem.Duration() > 0)
            {

                currActionLabelBtn.Width = (currActionLabelItem.Duration() / (slrProgress.Maximum - slrProgress.Minimum)) * TrackAction.ActualWidth;

                Canvas.SetLeft(currActionLabelBtn, currActionLabelItem.StartTime / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth);


                int intersected = FindActionIntersectedLabel(currActionLabelItem.StartTime);

                if (intersected != -1)
                {
                    LabelItem intersectedItem = actionLabels[intersected];
                    intersectedItem.EndTime = currActionLabelItem.StartTime;
                    intersectedItem.Button.Width = (intersectedItem.Duration() / (slrProgress.Maximum - slrProgress.Minimum)) * TrackAction.ActualWidth;
                    Canvas.SetLeft(intersectedItem.Button, intersectedItem.StartTime / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth);
                }

            }
        }

        int FindActionIntersectedLabel(double curTime)
        {
            if (actionLabels.Count == 0)
            {
                return -1;
            }
            else
            {
                for (int i = 0; i < actionLabels.Count; i++)
                {
                    if ((actionLabels[i].StartTime <= curTime) && (curTime <= actionLabels[i].EndTime) && (actionLabels[i].ID != currActionLabelItem.ID))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        int FindActionFullyIntersectedLabel()
        {
            if (actionLabels.Count == 0)
            {
                return -1;
            }
            else
            {
                List<int> removeIdx = new List<int>();
                for (int i = 0; i < actionLabels.Count; i++)
                {

                    if ((currActionLabelItem.StartTime < actionLabels[i].StartTime) && (actionLabels[i].EndTime < currActionLabelItem.EndTime) && (actionLabels[i].ID != currActionLabelItem.ID))
                    {
                     
                        removeIdx.Add(i);
                    }
                }
                if (removeIdx.Count > 0)
                {
                    for (int i = removeIdx.Count - 1; i >= 0; i--)
                    {
                        CvsLabel.Children.Remove(actionLabels[removeIdx[i]].Button);
                        actionLabels.RemoveAt(removeIdx[i]);
                    }
                }

            }
            return -1;
        }
        #endregion
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (poseLabels.Count > 0)
            {

                for(int i = 0; i<poseLabels.Count; i++)
                {
                    //Console.WriteLine(poseLabels[i].ID + ", " + poseLabels[i].Duration() + ", " +poseLabels[i].StartTime + ", " +poseLabels[i].EndTime);
                }
            }

            if (actionLabels.Count > 0)
            {
                //Console.WriteLine("Action Labels");
                for (int i = 0; i < actionLabels.Count; i++)
                {
                    //Console.WriteLine(actionLabels[i].ID + ", " + actionLabels[i].Duration() + ", " + actionLabels[i].StartTime + ", " + actionLabels[i].EndTime);
                }
            }
        }



        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (poseLabels.Count > 0)
            {
                for(int i = 0; i<poseLabels.Count; i++)
                {
                    CvsLabel.Children.Remove(poseLabels[i].Button);
                }
                poseLabels = new List<LabelItem>();
            }
            if(actionLabels.Count > 0){
                for (int i = 0; i < actionLabels.Count; i++)
                {
                    CvsLabel.Children.Remove(actionLabels[i].Button);
                }
                actionLabels = new List<LabelItem>();
            }
        }

        #region Canvas Event
        private void CvsLabel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed && (ISDRAGGINGLEFT || ISDRAGGINGRIGHT))
            {
                Point p = e.GetPosition(CvsLabel);
                double newSlrProVal = (p.X / CvsLabel.ActualWidth * (slrProgress.Maximum - slrProgress.Minimum));

                if (isDragType != BehaviorType.None)
                {
                    if (ISDRAGGINGRIGHT && !ISDRAGGINGLEFT)
                    {
                   
                        if (currPoseLabelBtn != null)
                        {
                            if ((((newSlrProVal - currPoseLabelItem.StartTime) / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth) > 5) && (isDragType == BehaviorType.Pose))
                                slrProgress.Value = newSlrProVal;
                        }

                        if (currActionLabelBtn != null)
                        {
                            if ((((newSlrProVal - currActionLabelItem.StartTime) / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth) > 5) && (isDragType == BehaviorType.Action))
                                slrProgress.Value = newSlrProVal;
                        }
                    }
                    else if (ISDRAGGINGLEFT && !ISDRAGGINGRIGHT)
                    {
                        if (currPoseLabelBtn != null)
                        {
                            if ((((currPoseLabelItem.EndTime - newSlrProVal) / (slrProgress.Maximum - slrProgress.Minimum) * TrackPose.ActualWidth) > 5) && (isDragType == BehaviorType.Pose))
                                slrProgress.Value = newSlrProVal;
                        }
                        if (currActionLabelBtn != null)
                        {
                            if ((((currActionLabelItem.EndTime - newSlrProVal) / (slrProgress.Maximum - slrProgress.Minimum) * TrackAction.ActualWidth) > 5) && (isDragType == BehaviorType.Action))
                                slrProgress.Value = newSlrProVal;
                        }
                    }
                    else
                    {

                    }
                }
            }
            else if (e.RightButton == MouseButtonState.Released && (ISDRAGGINGRIGHT || ISDRAGGINGLEFT))
            {
                if ((ISDRAGGINGRIGHT == true) || (ISDRAGGINGLEFT == true) && (isDragType != BehaviorType.None))
                {
                    if (isDragType == BehaviorType.Pose)
                    {
                        FindPoseFullyIntersectedLabel();
                        currPoseLabelItem = null;
                        currPoseLabelBtn = null;
                    }
                    else // isDragType == BehaviorType.Action
                    {
                        FindActionFullyIntersectedLabel();
                        currActionLabelItem = null;
                        currActionLabelBtn = null;
                    }
                    ISDRAGGINGRIGHT = false;
                    ISDRAGGINGLEFT = false;
                    isDragType = BehaviorType.None;
                }
            }
            else
            {

            }



          
        }

        private void CvsLabel_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((ISDRAGGINGRIGHT == true) || (ISDRAGGINGLEFT == true) && (isDragType != BehaviorType.None))
            {
                if (isDragType == BehaviorType.Pose)
                {
                    FindPoseFullyIntersectedLabel();
                    currPoseLabelItem = null;
                    currPoseLabelBtn = null;
                }
                else // isDragType == BehaviorType.Action
                {
                    FindActionFullyIntersectedLabel();
                    currActionLabelItem = null;
                    currActionLabelBtn = null;
                }
                ISDRAGGINGRIGHT = false;
                ISDRAGGINGLEFT = false;
                isDragType = BehaviorType.None;
            }
    
        }

        private void CvsLabel_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((ISDRAGGINGRIGHT == true) || (ISDRAGGINGLEFT == true) && (isDragType != BehaviorType.None))
            {
                if (isDragType == BehaviorType.Pose)
                {
                    FindPoseFullyIntersectedLabel();
                    currPoseLabelItem = null;
                    currPoseLabelBtn = null;
                }
                else // isDragType == BehaviorType.Action
                {
                    FindActionFullyIntersectedLabel();
                    currActionLabelItem = null;
                    currActionLabelBtn = null;
                }
                ISDRAGGINGRIGHT = false;
                ISDRAGGINGLEFT = false;
                isDragType = BehaviorType.None;
            }
         
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (poseLabels.Count > 0)
            {
                for(int i = poseLabels.Count-1; i>=0; i--)
                {
                    // if is selected, remove it
                    if(poseLabels[i].Status == TagStatus.Active)
                    {
                        Button b = poseLabels[i].Button;
                        CvsLabel.Children.Remove(b);
                        poseLabels.RemoveAt(i);
                    }
                }
            }

            if (actionLabels.Count > 0)
            {
                for (int i = actionLabels.Count - 1; i >= 0; i--)
                {
                    // if is selected, remove it
                    if (actionLabels[i].Status == TagStatus.Active)
                    {
                        Button b = actionLabels[i].Button;
                        CvsLabel.Children.Remove(b);
                        actionLabels.RemoveAt(i);
                    }
                }
            }
        }
        #endregion

        private void BtnSaveGroundTruths_Click(object sender, RoutedEventArgs e)
        {
            if (!EDITED_TAG_MODE)
            {
                SaveFileDialog s = new SaveFileDialog();
                s.Filter = "CSV|*.csv";
                if (s.ShowDialog() == true)
                {
                    StreamWriter writer = new StreamWriter(s.FileName);
                    writer.WriteLine("Id,TYPE,LABEL,START_TIME,END_TIME");
                    if (poseLabels.Count > 0)
                    {
                        foreach (LabelItem item in poseLabels)
                        {
                            writer.WriteLine(item.ID + "," + item.BehaviorType + "," + ((PoseDict.ContainsKey(item.Label))? PoseDict[item.Label].ToString(): item.Label) + "," + (item.StartTime / 1000) + "," + (item.EndTime / 1000));
                        }
                    }

                    if (actionLabels.Count > 0)
                    {
                        foreach (LabelItem item in actionLabels)
                        {
                            writer.WriteLine(item.ID + "," + item.BehaviorType + "," + ((ActionDict.ContainsKey(item.Label)) ? ActionDict[item.Label].ToString() : item.Label) + "," + (item.StartTime / 1000) + "," + (item.EndTime / 1000));
                        }
                    }
                    writer.Close();

                    using (StreamWriter sw = File.CreateText(System.IO.Path.GetDirectoryName(s.FileName) + "\\" + System.IO.Path.GetFileNameWithoutExtension(s.FileName) + ".txt"))
                    {
                        // write offset to file
                        DateTime now = DateTime.Now;
                        sw.WriteLine("TimeStamp:" + now.ToString());
                        sw.WriteLine("FileName:" + System.IO.Path.GetFileNameWithoutExtension(s.FileName));
                        sw.WriteLine("TimeOffset:" + offset);
                    }
                }
            } else
            {
                MessageBox.Show("Please save edited tags first!!!");
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void btnEditTag_Click(object sender, RoutedEventArgs e)
        {
            EDITED_TAG_MODE = true;
            int[] activePoses = null, activeActions = null;
            int count = FindActiveLabels(out activePoses, out activeActions);
            if(count > 0)
            {
                TextBox newTxtBx = null;
                if(activePoses != null)
                {
                    for(int i = 0; i<activePoses.Length; i++)
                    {
                        newTxtBx = new TextBox();
                        newTxtBx.Text = poseLabels[activePoses[i]].Label;
                        poseLabels[activePoses[i]].Button.Content = newTxtBx;
                    }
                }

                if (activeActions != null)
                {
                    for (int i = 0; i < activeActions.Length; i++)
                    {
                        newTxtBx = new TextBox();
                        newTxtBx.Text = actionLabels[activeActions[i]].Label;
                        actionLabels[activeActions[i]].Button.Content = newTxtBx;
                    }
                }
            }
        }

        private void btnSaveEditTag_Click(object sender, RoutedEventArgs e)
        {
            EDITED_TAG_MODE = false;
            int[] activePoses = null, activeActions = null;
            int count = FindActiveLabels(out activePoses, out activeActions);
            if (count > 0)
            {
                TextBox newTxtBx = null;
                if (activePoses != null)
                {
                    for (int i = 0; i < activePoses.Length; i++)
                    {
                        newTxtBx = poseLabels[activePoses[i]].Button.Content as TextBox;
                        poseLabels[activePoses[i]].SetLabelName(newTxtBx.Text);
                        newTxtBx = null;
                    }
                }

                if (activeActions != null)
                {
                    for (int i = 0; i < activeActions.Length; i++)
                    {
                        newTxtBx = actionLabels[activeActions[i]].Button.Content as TextBox;
                        actionLabels[activeActions[i]].SetLabelName(newTxtBx.Text);
                        newTxtBx = null;
                    }
                }
            }
        }

        private void txtBxTimeOffset_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
        }

        private void btnUpdateOffsetTime_Click(object sender, RoutedEventArgs e)
        {
            offset = Double.Parse(txtBxTimeOffset.Text);
        }

        private void BtnLoadGroundTruths_Click(object sender, RoutedEventArgs e)
        {
            
            // First remove all the old labels
            if (poseLabels.Count > 0)
            {
                for (int i = poseLabels.Count - 1; i >= 0; i--)
                {
                    Button b = poseLabels[i].Button;
                    CvsLabel.Children.Remove(b);
                    poseLabels.RemoveAt(i);
                }
            }

            if (actionLabels.Count > 0)
            {
                for (int i = actionLabels.Count - 1; i >= 0; i--)
                {

                    Button b = actionLabels[i].Button;
                    CvsLabel.Children.Remove(b);
                    actionLabels.RemoveAt(i);
                }
            }

            poseLabels = new List<LabelItem>();
            actionLabels = new List<LabelItem>();

            OpenFileDialog o = new OpenFileDialog();
            o.Filter = "CSV|*.csv";
            if(o.ShowDialog() == true)
            {
                using(StreamReader reader = File.OpenText(o.FileName))
                {
                    reader.ReadLine();
                    string line = null;
                    int id = -1;
                    BehaviorType type = BehaviorType.None;
                    string label = "";
                    double start = 0, end = 0;
                    while ((line = reader.ReadLine())!= null)
                    {
                        string[] words = line.Split(',');
                        //Id,TYPE,LABEL,START_TIME,END_TIME
                        id = Convert.ToInt32(words[0]);
                        if (words[1] == BehaviorType.Pose.ToString())
                        {
                            type = BehaviorType.Pose;
                        } else if(words[1] == BehaviorType.Action.ToString())
                        {
                            type = BehaviorType.Action;
                        }
                        else // None
                        {
                            type = BehaviorType.None;
                        }

                        label = words[2];
                        start = Convert.ToDouble(words[3])*1000;
                        end = Convert.ToDouble(words[4]) * 1000;

                        LoadGroudtruthLabel(id, type, label, start, end);
                    }
                }
            }
        }

        private void BtnResetLabels_Click(object sender, RoutedEventArgs e)
        {
            //if (poseLabels.Count > 0)
            //{
            //    for (int i = poseLabels.Count - 1; i >= 0; i--)
            //    {
            //            Button b = poseLabels[i].Button;
            //            CvsLabel.Children.Remove(b);
            //            poseLabels.RemoveAt(i);
            //    }
            //}

            //if (actionLabels.Count > 0)
            //{
            //    for (int i = actionLabels.Count - 1; i >= 0; i--)
            //    {
                    
            //            Button b = actionLabels[i].Button;
            //            CvsLabel.Children.Remove(b);
            //            actionLabels.RemoveAt(i);
            //    }
            //}
        }

        private void mePlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            //Console.WriteLine("ooo");
            slrProgress.Minimum = 0;
            slrProgress.Maximum = (int)mePlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void playSpeed_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(playSpeed.SelectedIndex == 0)
            {
                mePlayer.SpeedRatio = 0.3;
            }
            else if (playSpeed.SelectedIndex == 1)
            {
                mePlayer.SpeedRatio = 0.5;

            }
            else if (playSpeed.SelectedIndex == 3)
            {
                mePlayer.SpeedRatio = 1.3;

            }
            else if(playSpeed.SelectedIndex == 4)
            {
                mePlayer.SpeedRatio = 1.5;

            } else
            {
                mePlayer.SpeedRatio = 1;
            }
        }

        private void chckBoxSetOffset_Checked(object sender, RoutedEventArgs e)
        {
            if(chckBoxSetOffset.IsChecked == true)
            {
                slrProgress.IsSnapToTickEnabled = true;
                slrProgress.IsMoveToPointEnabled = true;
                slrProgress.TickFrequency = 1;
                Console.WriteLine("slrProgress.IsSnapToTickEnabled 10" + slrProgress.IsSnapToTickEnabled);
            }
        }
        
        private void chckBoxSetOffset10_Checked(object sender, RoutedEventArgs e)
        {
            if (chckBoxSetOffset10.IsChecked == true)
            {
                slrProgress.IsSnapToTickEnabled = true;
                slrProgress.IsMoveToPointEnabled = true;
                slrProgress.TickFrequency = 10;
                Console.WriteLine("slrProgress.IsSnapToTickEnabled 10" + slrProgress.IsSnapToTickEnabled);
            }
        }

        private void chckBoxSetOffset_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chckBoxSetOffset.IsChecked == false)
            {
                slrProgress.IsSnapToTickEnabled = false;
                slrProgress.IsMoveToPointEnabled = false;
                slrProgress.TickFrequency = 1;
                Console.WriteLine("slrProgress.IsSnapToTickEnabled " + slrProgress.IsSnapToTickEnabled);
            }
        }

        private void chckBoxSetOffset10_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chckBoxSetOffset10.IsChecked == false)
            {
                slrProgress.IsSnapToTickEnabled = false;
                slrProgress.IsMoveToPointEnabled = false;
                slrProgress.TickFrequency = 1;
                Console.WriteLine("slrProgress.IsSnapToTickEnabled 10 " + slrProgress.IsSnapToTickEnabled);
            }
        }
    }
}
