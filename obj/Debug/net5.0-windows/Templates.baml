   M S B A M L   `   `   `  ����E  AViewSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:ViewSample.ModelsViewSample.Models  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity 5      \ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089b ^GalaSoft.MvvmLight.Platform, Version=5.4.1.0, Culture=neutral, PublicKeyToken=5f873c45e98af8a1   	BoolToVis  �� DataType#    ViewSample.Models.FolderModel � RowDefinitions  folder_image J� IsHitTestVisible �� 	Converter.  (System.Windows.Interactivity.Interaction   Triggers/  )System.Windows.Interactivity.EventTrigger   	EventName.  (System.Windows.Interactivity.TriggerBase   Actions/  )GalaSoft.MvvmLight.Command.EventToCommand   PassEventArgsToCommand   Command!   ViewSample.Models.FileModel	 � ColumnDefinitions�� 5      H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      :x,http://schemas.microsoft.com/winfx/2006/xaml     5      )localclr-namespace:ViewSample.Models  5      ViPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  5      *cmd http://www.galasoft.ch/mvvmlight  5      %  &	        (��      5
      *,  +)7   (�� �    5,      *, +)�� 5      6#   �� 5
      "    6   .R�5   
   � $	��100��6   $	��100��6   
������  5      �� 5      �� 5      $
��0.5* �6    .+�5      �� #���  6   $ False��6<   6W   �� 5      $��1��6   
�������$��Center=�68   $ False��6n   ��6U   �� *Name+6�   �� 5      
���   �$��5q�6)   y�6=   �� *
IsSelected+8   6�   z�5      T� 5      tt��6%    5        5      $ 	MouseMove��6!   . 5        $
 True��6J    6)   �� *DragElement+6J   5        5!      $ MouseLeftButtonDown��6!   . 5"        $
 True��6J    6)   �� *ClickElement+6J   5#      5'      5(      �� 5,      "   6   .R�5-   
   �� 
�������$	��0.5q�6$   ._�5.      � 	 5/      �� 50      �� 51      .+�54      �� ��6   �� *
FileName+67   �� 55      
����ح�$��1��60   ��6@   �� *
OpenFile+.��6]   Open6c   56      57      58      5;      