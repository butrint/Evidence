   M S B A M L   `   `   `  ����C  ?Evidence, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:EvidenceEvidence  5   	       Evidence.StudentsWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:Evidence     5   	     � Title$  StudentsWindow��5   	   $��346.689��6    $��726.316��61   .��5	      � .+�5
   
   &   System.Windows.Controls.DataGrid  -   5          Name$ gridStudents��5
      $
��Left=�6J   $��	10,60,0,0q�6e      AutoGenerateColumns .  $	��Top=�6�   $	��198��6�   $	��698��6�      CanUserAddRows .  ��6)   �� *
students+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6%      Header$ ID��6/      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6;   �� *ID+6R     5      $ * 6%   $
 Emri��6/    .  6=   �� *Name+6V     5      $ * 6%   $ Mbiemri��6/    .  6@   �� *	Surname+6\     5      $ * 6%   $ 
Nenshkrimi��6/    . 6C   5      �� 5   
   -   5        	 ��Name$	 btnPrint��5      $��Printo��6#   $
��Left=�64   $��598,263,0,0q�6O   $	��Top=�6d   $	��110��6|   $��44��6�   6�   �� 5   
   $��VIJIMI I STUDENTËVE��6   $
��Left=�6.   $��	10,10,0,0q�6I   $	��Top=�6\   $��20(�6t   $
��Bold"�6�   ���	#FFC35454
 J� OpacityMask

 �   �$	��698��6�   $��Center=�6�   ��6�   Z� 6�   5      5      