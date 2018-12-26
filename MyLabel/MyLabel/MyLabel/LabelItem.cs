using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace MyLabel
{
    public class LabelItem
    {
        public double StartTime;
        public double EndTime;
        public string Label;
        public TagStatus Status;
        private Button button;
        public BehaviorType BehaviorType = BehaviorType.None;
        public int ID;

        public Button Button
        {
            get { return button; }
            set
            {
                button = value;
                ButtonTag = ID;
            }
        }

        

        public int ButtonTag
        {
            get
            {
                return (int)Button.Tag;
            }
            set
            {
                Button.Tag = value;
            }
        }

        public LabelItem(double startTime, string label, int Id, BehaviorType type)
        {
            StartTime = startTime;
            Label = label;
            ID = Id;
            Status = TagStatus.Inactive;
            BehaviorType = type;
        }

        public LabelItem(double startTime, string label, int Id)
        {
            StartTime = startTime;
            Label = label;
            ID = Id;
            Status = TagStatus.Inactive;
        }

        public LabelItem(double startTime, double endTime, string label, Button btn, int Id)
        {
            StartTime = startTime;
            EndTime = endTime;
            Label = label;
            ID = Id;
            Button = btn;
            Status = TagStatus.Inactive;
        }


        public double Duration()
        {
            return EndTime - StartTime;
        }

        public void SetLabelName(string label)
        {
            Label = label;
            button.Content = label;
        }

    }

    public class TagItem
    {
        public TagStatus Status = TagStatus.Inactive;
        public double TimeStamp;
        public Rectangle Tag;
    }

    public enum MediaPlayerState
    {
        Playing,
        Stopping,
        Pausing
    }

    public enum TagStatus
    {
        Active,
        Inactive
    }

    public enum BehaviorType
    {
        Action,
        Pose,
        None
    }
}
