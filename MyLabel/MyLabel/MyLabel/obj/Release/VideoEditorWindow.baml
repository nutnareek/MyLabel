   M S B A M L   `   `   `  ���� B  >MyLabel, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:MyLabelMyLabel      MyLabel.VideoEditorWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:MyLabel     -     � Title$  VideoEditorWindow��$	��500��$	��600��.���  � RowDefinitions �� $	��300 ��� $
��Auto ��� $
��Auto �.+��� $��0��
���   �r� -    r�Name$ editorPlayer��$��0��$
��10 5q� r� Source$< 6C:\\Users\\nutnaree\\Desktop\\Group2_Product_Video.mp4?� r� LoadedBehavior$ Manual=� r� ScrubbingEnabled . �� -    ��Name$ 	CanvasTag��$��1��$
��10 5q��� -    ��Name$ SlrEditProg��$��1��$
��10 5q�� $��2��$��	10 5 10 5q� � ColumnDefinitions �� $��2* ��� $��* �.+��� $��0��	 �� Orientation$	 
Horizontal=�.+��� -   
 ��Name$
 	BtnRewind��{�.  $��0 0 5 0q�$��30��$��30��
������    CustomButtonFocusStyle#���   ���� -   	ImgRewind�� �� ImageSource$ Images/rewind.png���� -   $
 BtnPlay��$��0 0 5 0q�$��30��$��30��
������   CustomPlayFocusStyle#���  ���� -   ImgPlay��$ Images/play.png���� -	   $
 BtnStop��$��0 0 5 0q�$��30��$��30��
������   StopButtonStyle#���  ���� $ Images/stop.png���� -
   $
 	BtnAddTag��$��0 0 5 0q�$��30��$��30��
������   AddTagButtonStyle#���  ���� $ Images/plus.png���� -   $
 BtnRemoveTag��$��0,0,5,1q�$��30��
������   RemoveTagButtonStyle#���  ���� $ Images/minus.png���� $��1��$	 
Horizontal=�$��Right=�.+��� -   $
 BtnOK��$��OK��$��0 0 5 0q�$��5q��� -   $
 	BtnCancel��$��Cancel��$��5q�