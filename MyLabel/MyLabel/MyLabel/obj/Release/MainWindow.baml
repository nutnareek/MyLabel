   M S B A M L   `   `   `  ���� B  >MyLabel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:MyLabelMyLabel      MyLabel.MainWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:MyLabel     -     � Title$  
MainWindow��$��914.5��$��1331.5�� � 
ResizeMode$ NoResize=� � 	Resources    CursorHorizontalAdjustment � Cursor�� %3   &	        �� $# Data/HorizontalAdjustment.cur��.���� .+�� -    � ColumnDefinitions �� $	��87* ��� $	��61* ��� $
��736* ��� $
��442* �.+��� $��0�� �� Orientation$ 
Horizontal=�$��10,10,9.5,10q�$��3��.+��� -    ��Name$ BtnOpenInstruction��$��Instruction��$��5q�$��0 0 5 0q��� -   $ 
BtnLoadVid��$��
Load Video��$��5q�$��0 0 5 0q� J� RenderTransformOrigin$ 	0.502,0.5&�$��28��$��Bottom=��� -   $ BtnLoadTrimVid��$��Load and Trim Video��$��5q�$��0 0 5 0q�$ 	0.502,0.5&�$��28��$	��Top=��� -   $ BtnSaveGroundTruths��$��Save Labels��$��5q�$��0 0 5 0q�$��28��$	��Top=��� -   $ BtnLoadGroundTruths��$��Load Labels��$��5q�$��0 0 5 0q�$��28��$��Bottom=��� -   $ BtnResetLabels��{�.  $��Reset Labels��$��5q�$��28��$	��Top=��� $��3��$ 
Horizontal=�$��0,10,9.5,10q�$��Right=�.+��� $��Time Offset��$��5q�$��5 0 0 0q��� -	   $��txtBxTimeOffset��$
��0.00��$��5 0 0 0q�$��5q��� -
   $��btnUpdateOffsetTime��$��Update��$��5 0 10 0q�$��5q��� $��Setting��$��5q�$��5 0 0 0q��  � RowDefinitions �� $	��600 ��� $
��Auto ��� $
��Auto ��� $��* �.+��� $��0��
���   �r� -   	 r�Name$	 mePlayer��$��0��$
��10 5q�
 r� Source$=
 7C:\\Users\\NutnareeK\\Desktop\\Group2_Product_Video.mp4?� r� LoadedBehavior$ Manual=� r� ScrubbingEnabled . �� -    ��Name$ slrProgress��$��
10 10 10 0q�$��1���� -    ��Name$ CvsLabel��$
��Auto��$��70��$��2��.+��� -    ��Name$ TrackEmbrace��
������� �� Left$ 0��
 �� Top$ 0��$��35������  �� ElementName$ CvsLabel�� �� Path$ ActualWidth��� $��Embrace Track��$��10(��� -   $ 
TrackTouch��
�������$ 0��$ 35��$��35������ $ CvsLabel��$ ActualWidth��� $��Touch Gesture Track��$��10(�$ 0��$ 35���� -   $ CvsTrackProgress��$
��Auto��$��70��$��2��.+��� -   $ 	TrackLine��
���  ��$ 0��$ 0��$��1��$��70���� $ 
Horizontal=�$��3��$��10q�.+��� -    ��Name$ 	lblStatus��$��Not Playing...���� -   $ btnPlay��$	��4 2q�$
��Play���� -   $ btnPause��$	��4 2q�$	��5 0q�$��Pause���� -   $ btnStop��$	��4 2q�$��5 0 20 0q�$
��Stop���� $��
Play Speed���� -    ��Name$ 	playSpeed��$��5 0 20 0q�.@��� .��0.3x�� .��0.5x��  �� 
IsSelected . .��1x�� .��1.3x�� .��1.5x�� -   $ btnReset��$	��4 2q�$	��5 0q�$��Reset���� -   $ 	btnRemove��$	��4 2q�$	��5 0q�$��Remove��$��26���� -   $ button��$��Get Label Info��$��91��$��26��$	��Top=��� -   $ 
btnEditTag��$��Edit Tag Names��$	��4 2q�$	��5 0q�$��26��$	��Top=�{�.  �� -   $ btnSaveEditTag��$��Edit Tag Names��$	��4 2q�$	��5 0q�$��26��$	��Top=�{�.  �� -    ��Name$ chckBoxSetOffset��$��
Set Offset���� -   $ chckBoxSetOffset10��$��
Set Offset���� -   $ btnStartTag��$��Start��$	��4 2q�$	��5 0q�$��26��$	��Top=�{�.  �� -   $ 
btnStopTag��$	��End��$	��4 2q�$	��5 0q�$��26��$	��Top=�{�.  � �  �� $��2* ��� $��2* � �� $
��Auto ��� $
��Auto �.+��� $��Embrace Type��$
��Bold"�$��0��$��0��$��10 0 0 5q��� $ 
Horizontal=�$��0��$��1��$
��10 0q�.+��� $ Vertical=�$��0 0 5 0q�.+��� $��Cradle L��$	��4 2q��� $��A��$��Center=��� $ Vertical=�$��0 0 5 0q�.+��� $��Cradle R��$	��4 2q��� $��Z��$��Center=��� $ Vertical=�$��0 0 5 0q�.+��� $��	Snuggle L��$	��4 2q��� $��S��$��Center=��� $ Vertical=�$��0 0 5 0q�.+��� $��	Snuggle R��$	��4 2q��� $��X��$��Center=��� $ Vertical=�$��0 0 5 0q�.+��� $
��Lift��$	��4 2q��� $��D��$��Center=��� $ Vertical=�.+��� $��Heelo��$��0 0 5 0q�$	��4 2q��� $��F��$��Center=��� $ Vertical=�.+��� $��
Transition��$��0 0 5 0q�$	��4 2q��� $��V��$��Center=��� $ Vertical=�.+��� $��Touch��$	��4 2q��� $��C��$��Center=��� $��Gesture Type��$
��Bold"�$��1��$��0��$��0 0 0 5q��� $ 
Horizontal=�$��1��$��1��.+��� $ Vertical=�$��0 0 5 0q�.+��� $��	PatArouse��$	��4 2q��� $��G��$��Center=��� $ Vertical=�$��0 0 5 0q�.+��� $��PatCalm��$	��4 2q��� $��H��$��Center=��� $ Vertical=�$��0 0 5 0q�.+��� $	��Rub��$	��4 2q��� $��J��$��Center=��� $ Vertical=�$��0 0 5 0q�.+��� $��Stroke��$	��4 2q��� $��K��$��Center=��� $ Vertical=�$��0 0 5 0q�.+��� $
��Slap��$	��4 2q��� $��L��$��Center=��� $ Vertical=�$��0 0 5 0q�.+��� $��Updown��$	��4 2q��� $��B��$��Center=��� $ Vertical=�.+��� $��Shake��$	��4 2q��� $��N��$��Center=�