using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using MediaToolkit.Options;
using MediaToolkit.Model;
using MediaToolkit;

namespace MyLabel
{
    /// <summary>
    /// Interaction logic for VideoEditorWindow.xaml
    /// </summary>


    public partial class VideoEditorWindow : Window
    {
        List<TagItem> tagItems;
        public Uri VideoPath = null;
        DispatcherTimer timer = null;
        bool userIsDraggingSlider = false;
        MediaPlayerState playerState = MediaPlayerState.Stopping;
        public VideoEditorWindow()
        {
            InitializeComponent();
            tagItems = new List<TagItem>();
        }

        public VideoEditorWindow(Uri newUri)
        {
            InitializeComponent();
            VideoPath = newUri;
            editorPlayer.Source = VideoPath;
            tagItems = new List<TagItem>();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            //timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if ((editorPlayer.Source != null) && (editorPlayer.NaturalDuration.HasTimeSpan) && (!userIsDraggingSlider) && (playerState == MediaPlayerState.Playing))
            {
                SlrEditProg.Minimum = 0;
                SlrEditProg.Maximum = editorPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                SlrEditProg.Value = editorPlayer.Position.TotalMilliseconds;
            }
            //Console.WriteLine(editorPlayer.Position.TotalMilliseconds / 1000.0);
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (playerState == MediaPlayerState.Stopping)
            {      
                timer.Start();
                playerState = MediaPlayerState.Playing;
                editorPlayer.Play();
            } else if(playerState == MediaPlayerState.Playing)
            {
                editorPlayer.Pause();
                playerState = MediaPlayerState.Pausing;

                // Load Pause Button
                string packUri = @"pack://application:,,,/Images/pause.png";
                ImgPlay.ImageSource = (new ImageSourceConverter()).ConvertFromString(@packUri) as ImageSource;
            } else
            {
                editorPlayer.Play();

                // Load Play Button
                string packUri = @"pack://application:,,,/Images/play.png";
                ImgPlay.ImageSource = (new ImageSourceConverter()).ConvertFromString(@packUri) as ImageSource;
                playerState = MediaPlayerState.Playing;
            }
            Console.WriteLine(playerState);
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            if (playerState == MediaPlayerState.Playing| playerState == MediaPlayerState.Pausing)
            {
                editorPlayer.Stop();
                SlrEditProg.Value = SlrEditProg.Minimum;
                playerState = MediaPlayerState.Stopping;
                timer.Stop();
            }
        }

        private void SlrEditProg_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            editorPlayer.Position = TimeSpan.FromMilliseconds(SlrEditProg.Value);
            //lblStatus.Content = TimeSpan.FromMilliseconds(slrProgress.Value).ToString(@"hh\:mm\:ss\.ff");
            //Canvas.SetLeft(TrackLine, slrProgress.Value / slrProgress.Maximum * TrackEmbrace.Width);
        }

        private void SlrEditProg_DragCompleted(object sender, RoutedEventArgs e)
        {
            userIsDraggingSlider = false;
            editorPlayer.Position = TimeSpan.FromMilliseconds(SlrEditProg.Value);
            if (playerState == MediaPlayerState.Playing)
            {
                editorPlayer.Play();
            }
        }

        private void SlrEditProg_DragStarted(object sender, RoutedEventArgs e)
        {
            if (playerState == MediaPlayerState.Playing)
            {
                editorPlayer.Pause();
            }
            
            userIsDraggingSlider = true;
        }

        #region Tagging System
        private void BtnAddTag_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(tagItems.Count);
            if (tagItems.Count < 2)
            {
                TagItem newTag = new TagItem();
                newTag.TimeStamp = SlrEditProg.Value;
                Rectangle rect = new Rectangle();
                rect.Width = 6;
                rect.Height = 25;
                rect.Fill = new SolidColorBrush(Colors.Crimson);
                rect.MouseEnter += RectTag_MouseEnter;
                rect.MouseLeave += RectTag_MouseLeave;
                rect.MouseLeftButtonDown += RectTag_MouseLeftButtonDown;
                rect.Tag = tagItems.Count;
                CanvasTag.Children.Add(rect);
                Canvas.SetTop(rect, 0);

                double startPos = Thumb.Width / 2;
                double slrWidth = SlrEditProg.ActualWidth - (startPos * 2);
                double leftPos = startPos + (newTag.TimeStamp / (SlrEditProg.Maximum - SlrEditProg.Minimum) * slrWidth) - (rect.Width/2);
                //double leftpos = (Thumb.Width / 2) - (rect.Width / 2);
                Canvas.SetLeft(rect, leftPos);
                newTag.Tag = rect;

                //double leftPos = ((newTag.TimeStamp - (rect.Width / 2)) > (rect.Width / 2)) ? newTag.TimeStamp - (rect.Width / 2) : newTag.TimeStamp - (SlrEditProg.Value - SlrEditProg.Minimum);
                //newTag.Tag = rect;
                AddTagItem(newTag);
            } else
            {
                MessageBox.Show("You can only add 2 tags for start time and end time for triming.");
            }
        }

        private Thumb Thumb
        {
            get
            {
                return GetThumb(SlrEditProg) as Thumb; ;
                //return GetThumb(this /* the slider */ ) as Thumb; ;
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

        private void RectTag_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Rectangle)sender).Fill = new SolidColorBrush(Colors.LightSalmon);
           // Console.WriteLine(tagItems[(int)((Rectangle)sender).Tag].Status + ", " + ((Rectangle)sender).Tag);
        }

        private void RectTag_MouseLeave(object sender, MouseEventArgs e)
        {
            if(tagItems[(int)(((Rectangle)sender).Tag)].Status != TagStatus.Active)
                ((Rectangle)sender).Fill = new SolidColorBrush(Colors.Crimson);
            else
                ((Rectangle)sender).Fill = new SolidColorBrush(Colors.LightPink);
        }

        private void RectTag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            TagItem selTagItem = tagItems[(int)rect.Tag];
            
            if (selTagItem.Status == TagStatus.Inactive)
            {
                ((Rectangle)sender).Fill = new SolidColorBrush(Colors.LightPink);
                selTagItem.Status = TagStatus.Active;
            } else
            {
                ((Rectangle)sender).Fill = new SolidColorBrush(Colors.Crimson);
                selTagItem.Status = TagStatus.Inactive;
            }
           // Console.WriteLine(selTagItem.Status);
        }

        void AddTagItem(TagItem newTag)
        {
            // Add tag to tagItem List
            if (tagItems.Count == 1)
            {
                if (tagItems[0].TimeStamp < newTag.TimeStamp)
                    tagItems.Add(newTag);
                else
                {
                    newTag.Tag.Tag = 0;
                    tagItems.Insert(0, newTag);
                    ResetTagItemList();
                }

            } else if(tagItems.Count > 1)
            {
                if (tagItems[0].TimeStamp > newTag.TimeStamp)
                {
                    newTag.Tag.Tag = 0;
                    tagItems.Insert(0, newTag);
                    ResetTagItemList();
                }
                else if(tagItems[tagItems.Count-1].TimeStamp<newTag.TimeStamp)
                {
                    tagItems.Add(newTag);
                } else 
                {
                    for(int i = 0; i<(tagItems.Count-1); i++)
                    {
                        if((tagItems[i].TimeStamp<newTag.TimeStamp) && (tagItems[i + 1].TimeStamp > newTag.TimeStamp)){
                            newTag.Tag.Tag = i + 1;
                            tagItems.Insert(i+1, newTag);
                            ResetTagItemList();
                        }
                    }
                }
            }
            else {
                tagItems.Add(newTag);
            }

            
        }

        void RemoveTagItem(int idx)
        {
            if (tagItems.Count > 0)
            {
                Rectangle rect = tagItems[idx].Tag;
                Canvas cv = (Canvas)rect.Parent;
                cv.Children.Remove(rect);
                tagItems.RemoveAt(idx);
                // Reset Index Tag in each Tag Rectangle
                for(int i = 0; i<tagItems.Count; i++)
                {
                    ResetTagItemList();
                }
            }
        }
        void ResetTagItemList()
        {
            for (int i = 0; i < tagItems.Count; i++)
            {
                tagItems[i].Tag.Tag = i;
            }
        }

        private void BtnRemoveTag_Click(object sender, RoutedEventArgs e)
        {
            if (tagItems.Count > 0)
            {
                List<int> removeIdx = new List<int>();
                for (int i = 0; i < tagItems.Count; i++)
                {
                    if(tagItems[i].Status == TagStatus.Active)
                    {
                        //Console.WriteLine("To remove idx >> " + i);
                        removeIdx.Add(i);
                    }
                }
                if (removeIdx.Count > 0)
                {
                    for (int i = removeIdx.Count - 1; i >= 0; i--)
                    {
                        RemoveTagItem(removeIdx[i]);
                    }
                }
            }
        }

        #endregion

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {

            if (tagItems.Count == 1)
            {
                MessageBox.Show("Please tags or un tags the videos.");
            }
            else if (tagItems.Count == 2)
            {
                var inputFile = new MediaFile { Filename = @VideoPath.AbsolutePath };
                string outputPath = System.IO.Path.GetDirectoryName(VideoPath.AbsolutePath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(VideoPath.AbsolutePath) + "_Trim" + System.IO.Path.GetExtension(VideoPath.AbsolutePath);
                var outputFile = new MediaFile { Filename = @outputPath };
                //Console.WriteLine(System.IO.Path.GetDirectoryName(VideoPath.AbsolutePath)+"\\"+ System.IO.Path.GetFileNameWithoutExtension(VideoPath.AbsolutePath)+"_Trim"+ System.IO.Path.GetExtension(VideoPath.AbsolutePath));

                using (var engine = new Engine())
                {
                    engine.GetMetadata(inputFile);
                    var options = new ConversionOptions();
                    options.CutMedia(TimeSpan.FromMilliseconds(tagItems[0].TimeStamp), TimeSpan.FromMilliseconds(tagItems[1].TimeStamp - tagItems[0].TimeStamp));
                    engine.Convert(inputFile, outputFile, options);
                }
                VideoPath = new Uri(outputPath);
                this.Close();
            } else
            {
                
                this.Close();
            }
            
        }
    }
}
