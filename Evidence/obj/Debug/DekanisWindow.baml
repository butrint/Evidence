   M S B A M L   `   `   `  ����C  ?Evidence, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:EvidenceEvidence  5   	       Evidence.DekanisWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Evidence     5   	     � Title$  DekanisWindow��5   	   $	��503��6   $	��926��6,    � WindowStartupLocation$ CenterScreen=�68   ��5	      �� 5
   
    �� EndPoint$ 0.5,1&�6    �� 
StartPoint$ 0.5,0&�6/   .��5      �  � Color$ Black��6    � Offset$ 0Q�6)   � 5      $ White��6   $ 1Q�6)   5      .��5      � ��5   
   �� 5      $ 0.5,1&�6"    � MappingMode$ RelativeToBoundingBox=�63   $ 0.5,0&�6W   .��5      � $ 	#FF866991��6   � 5      $ White��6   $ 1Q�6-   5      .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ gridTodaySubs��5      $
��Left=�6�   $��	24,75,0,0q�6�   $	��Top=�6�   $	��368��6�   $	��872��6�      AutoGenerateColumns .  	   CanUserAddRows	 .  ���	#FFC1B5C5��6*   �� *todayAllData+
 �� Mode$
 OneWay=� �� NotifyOnSourceUpdated .  �� UpdateSourceTrigger$ PropertyChanged=� � 	Resources 5      )  #System.Windows.Controls.DataGridRow �� 
TargetType �� Property �� Value��%u   '        �� 5      "  6   .�5      ��  � �� 63   �� *
rowColor+$ PropertyChanged=�5      5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6%      Header$ 	Profesori��6/      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6B   �� *
username+6_     5      $ * 6%   $ Lenda��6/    .  6>   �� *	subject+6Z     5       $ * 6%   $ L / U��6/    .  6>   �� *lush+6W     5!      $ * 6%   $ Koha e fillimit��6/    .  6H   �� *
start_Time+6g     5"      $ * 6%   $ Koha e mbarimit��6/    .  6H   �� *
end_Time+6e   5+      �� 5,   
   $��VIJIMI I PROFESOREVE��6   $
��Left=�6/   $��	24,28,0,0q�6J   $	��Top=�6]   $��24(�6u   $
��Bold"�6�   ���	#FFC35454 J� OpacityMask
 �   �$	��872��6�   $��Center=�6�   ��6�   Z� 6�   5-      5.      