   M S B A M L   `   `   `  ����T  PDiscreteGestureBasics-WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullo<clr-namespace:Microsoft.Samples.Kinect.DiscreteGestureBasics.Microsoft.Samples.Kinect.DiscreteGestureBasics  5   	   ?    9Microsoft.Samples.Kinect.DiscreteGestureBasics.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Nlocal<clr-namespace:Microsoft.Samples.Kinect.DiscreteGestureBasics     5   	   -   5          � Title$  VOICE��5   	   $��551.493��5   	   $	��750��6    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� DataTypeF   @Microsoft.Samples.Kinect.DiscreteGestureBasics.GestureResultView � RowDefinitions �� StringFormat��%�  &	        &	 #     &	 F     (�� i     5      *, +)�� 5	   
   $ 	#ff6e6e6e��62   �� 5
   
   $ 	#ff52318f��64   �� 5   
   $ 	#ff00BCF2��62   �� 5   
   "  6   .R�5      � $
��Auto��6   $��5q�6    ��6+   �� *	BodyColor+ 5      �� 5      $
��Auto �6$   �� 5      �� 5      $
��Auto �6$   �� 5      $
��Auto �6$   .+�5      �� $��Stretch=�6   $��14(�6u   $��Segoe UI)�6�   $��SemiBold"�6�   $��5q�6�   ��6:   �� *	BodyIndex+$ Body Index: {0}��6�   �� 5      $��Uniform=�67   $��1��6I   $��5q�6V   ��6   �� *ImageSource+6V   �� 5      $��18(�6R   $��Times New Roman)�6`   $��SemiBold"�6}   $��2��6�   $��5 5 0 0q�6�   ��6   �� *
Detected+$ Seated: {0}��6�   �� 5      $��18(�6X   $��Times New Roman)�6f   $��SemiBold"�6�   $��3��6�   $��5 0 0 0q�6�   ��6   �� *
Confidence+$ Confidence: {0}��6�   5      5      5      .��5      � $��	10 0 10 0q�6    5   
   �� 5       $
��Auto �6   �� 5!      $��* �6   �� 5"      $
��Auto �6   .+�5%   
   � -   5         �Name$ contentGrid��5%      $��1��6$    � ColumnDefinitions 5&      �� 5'      $��0 �6#   �� 5(      $��0 �6#   �� 5)      $��1* �6#    5+      �� 5,      $��0 �6    �� 5-      $	��350 �6    �� 5.      $	��190 �6    .+�52      ��  �� Orientation$ Vertical=�6   $��2��60   $��3��6@   $��20 10 0 180q�6Q   $��Center=�6f   $��Center=�6�   .+�53      (� -   5        	 (�Name$	 kinectBodyViewbox��53      .��54      �� $
��Fill=�6;   ��6   �� *ImageSource+6;   55      �� 56      $
��Wrap=�6   $��5 10 5 5q�60   #��[  6B   $��comic sans ms)�6p   $��30(�6�   $	��434��6�   $��80��6�   �� 6�   $�        ��6�   �� 6�   $�Connect Your Kinect.��6�   6�   57      �� 58      -   5        
 ��Name$
 Display_Tags��58      $��3��6,   $
��Left=�6@   $��39��6[   $��	28,23,0,0q�6g   $��2��6z    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $��24(�6�   $	��487��6�    �� 
IsReadOnly . 6�   �� 59      -   5        $
 Display��59      $��3��6'   $
��Left=�6;   $��39��6V   $��	28,83,0,0q�6b   $��2��6u   $
 Wrap=�6�   $	��Top=�6�   $��24(�6�   $	��487��6�    . 6�   �� 5:      -   5         ��Name$ 
Controller��5:      $��3��6)   $
��Play��6=   $
��Left=�6L   $��39��6g   $��
581,23,0,0q�6s   $��2��6�   $	��Top=�6�   $	��109��6�   6�   �� 5;      -   5        $ 	The_voice��5;      $��3��6(   $��Voice IT��6<   $
��Left=�6O   $��39��6j   $��
581,83,0,0q�6v   $��2��6�   $	��Top=�6�   $	��109��6�   6�   5=      �� 5?   
   -   5        $��	statusBar��5?   ?   $��2��6   $��Stretch=�6!   $��Bottom=�6P   
�������#��[  6~   .@�5@      �� ��6   �� *
StatusText+5A      5B      5C      