   M S B A M L   `   `   `  ����C  ?Evidence, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:EvidenceEvidence  5   	       Evidence.EvidenceWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Evidence     5   	     � WindowState$  	Maximized=�5   P    � Title$ EvidenceWindow��5   	   $	��300��6    $��925.206��6-   $	��400��6=   $	��800��6M    � WindowStartupLocation$ CenterScreen=�6\    � 	Resources 5	       �� 
TargetType   EvidenceCheckBox �� Property �� Value��%�   &	        �� 5
   
   " ��6   .�5      ��  �@J�Opacity$ 1Q�6(   �� 5       � ��$ Center=�64   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��0.8* �6   �� 5      $��2* �6   ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"   	 �� 
StartPoint$	 0.5,0&�63   .��5      � 
 � Color$
 	#FF65B497��6    � Offset$ 0Q�61   � 5      $
 White��6   $ 1Q�6-   5      .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ gridStudents��5       $��1��6   $��20q�6W      AutoGenerateColumns .     CanUserAddRows .  ��66   �� *
students+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6%      Header$ ID��6/      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *Num_ID+6V     5      $ * 6%   $
 Emri��6/    .  6=   �� *Name+6V     5      $ * 6%   $ Mbiemri��6/    .  6@   �� *	Surname+6\   4  .System.Windows.Controls.DataGridTemplateColumn  5      $
 0.3* 6)   $ Check��66      CellTemplate 5       �� 5!      .R�5"      �� $	��Chk��6'   #��[  62    d� 	IsChecked 6\   �� *	isPresent+ �� Mode$ OneWay=� �� UpdateSourceTrigger$ PropertyChanged=�5#      5%      5(      �� 5)   
   -   5         ��Name$ 
lblAllStud��5)      $��1��6$   $�� ��61   $
��Left=�6<   $��20,0,0,0q�6W   $	��Top=�6i   $��Stretch=�6�   �� 5*   
   -   5        $ lblStudActive��5*      $��1��6'   $�� ��64   $��Right=�6?   $��0,0,20,0q�6[   $	��Top=�6m   $��Stretch=�6�   � 5,   
   $��0��6    � ColumnDefinitions 5-      �� 5.      $��1* �6#   �� 5/      $��1* �6#   �� 50      $��1* �6#   �� 51      $��1* �6#   .+�53      �� -   5        $ lblSubjects��53      $��0��6)   $��0��66   $��Lenda��6F    �� Target cmbSubjects6V   $
��Left=�6k   $��	20 0 0 40q�6�   $��Bottom=�6�   $	��120��6�   �� 54      -   5         ��Name$ cmbSubjects��54      $��0��6,   $��0��69   $
��Left=�6I   $��	20 0 0 20q�6d   $��Bottom=�6w   $	��120��6�    �� SelectedIndex$ 0��6�   �� 55      -   5        $ lblLush��55      $��0��6%   $��1��62   $��Lloji Lendes��6B    cmbLush6Y   $
��Left=�6j   $��	20 0 0 40q�6�   $��Bottom=�6�   $	��120��6�   �� 56      -   5        $ cmbLush��56      $��0��6(   $��1��65   $
��Left=�6E   $��	20 0 0 20q�6`   $��Bottom=�6s   $	��120��6�   $ 0��6�   �� 57      -   5        $ 	lblGroups��57      $��0��6'   $��2��64   $��Grupi��6D    	cmbGroups6T   $
��Left=�6g   $��	20 0 0 40q�6�   $��Bottom=�6�   $	��120��6�   �� 58      -	   5        $ 	cmbGroups��58      $��0��6*   $��2��67   $
��Left=�6G   $��	20,0,0,20q�6b   $��Bottom=�6u   $	��120��6�   $ 0��6�   �� 59      -
   5         ��Name$ btnStart��59      $��0��6'   $��3��64   $��Fillo��6D   $��Right=�6T   $��	0 0 20 20q�6p   $	��100��6�   $��Bottom=�6�   $��22��6�   5:      5<      5=      