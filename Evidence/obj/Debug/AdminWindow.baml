   M S B A M L   `   `   `  ����C  ?Evidence, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:EvidenceEvidence  5   	       Evidence.AdminWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Evidence     5   	     � WindowState$  	Maximized=�5   F    � Title$ AdminWindow��5   	   $��470.424��6   $��925.206��6.   $	��400��6>   $	��800��6N    � WindowStartupLocation$ CenterScreen=�6]   .��5	      �  � RowDefinitions 5
   
   �� 5      $��1* �6   �� 5      $��2* �6   ��5   
   �� 5       �� EndPoint$ 0.5,1&�6"    � MappingMode$ RelativeToBoundingBox=�63    �� 
StartPoint$ 0.5,0&�6W   .��5      �  � Color$ 	#FF65B497��6   � 5      $ White��6    � Offset$ 1Q�6-   5      .+�5   
   &   System.Windows.Controls.DataGrid  -   5        	  Name$	 gridTodaySubs��5       $��1��6   $��20q�67   
   AutoGenerateColumns
 .     CanUserAddRows .  
���������6C   �� *todayAllData+ �� Mode$ OneWay=� �� NotifyOnSourceUpdated .  �� UpdateSourceTrigger$ PropertyChanged=� � 	Resources 5      )  #System.Windows.Controls.DataGridRow �� 
TargetType �� Property �� Value��%u   '        �� 5      "  6   .�5      ��  � �� 63   �� *
rowColor+$ PropertyChanged=�5      5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6%      Header$ 	Profesori��6/      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *
username+6_     5      $ * 6%   $ Lenda��6/    .  6>   �� *	subject+6Z     5      $ * 6%   $ L / U��6/    .  6>   �� *lush+6W     5      $ * 6%   $ Koha e fillimit��6/    .  6H   �� *
start_Time+6g     5      $ * 6%   $ Koha e mbarimit��6/    .  6H   �� *
end_Time+6e     5       $ * 6%   $ Rr / Z��6/    .  6?   �� *isSubstitute+$ PropertyChanged=�6�   4  .System.Windows.Controls.DataGridTemplateColumn  5!      $
 0.6* 6)   $ Fillo��66      CellTemplate 5"      �� 5#      .R�5$      �� -   5        {�5$   :   �� *	isEnabled+$ PropertyChanged=���6   �� *
btnContent+5%      5'      5)      �� 5*   
   -   5         ��Name$ lblCurrUser��5*      $��0��6%   $�� ��62   $��20 0 0 0q�6=   $
��Left=�6O   $
��Left=�6j   $��Bottom=�6�   $��20(�6�   �� 5+   
   -   5        $ lblTodayDate��5+      $��0��6&   $��Data: ��63   $��Right=�6D   $��Bottom=�6`   $��Right=�6{   $��16(�6�   $��0 0 20 0q�6�   �� 5,   
   -   5         ��Name$ 	btnLogOut��5,      $��Çkyçu��6$   $��0��64   $��16(�6A   $
��Left=�6O   $	��Top=�6j   $��	20 20 0 0q�6�   $��93��6�   $��45��6�   ��6�   Z� ��6�   Z� 5A      �� 5B   
   $��VIJIMI I LIGJËRATËS��6   $��0��6.   $��Center=�6;   $��24(�6V   $
��Bold"�6d   ���	#FFC35454 J� OpacityMask
 �   �$��Stretch=�6�   $��Center=�6�   ��6v   Z� 6�   5D      5E      