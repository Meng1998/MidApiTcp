   M S B A M L   `   `   `  ����J  FDeploymentTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:DeploymentTools.ViewDeploymentTools.View  5   	   -    'DeploymentTools.View.EventSubscriptions   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:DeploymentTools.View     5   	   S OMaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  数据订阅��5	   	   $	��497��6   $	��954��6#    � Icon$. (/DeploymentTools;component/ConigIcon.png��6/   .��5
      �  � ColumnDefinitions 5   
   �� 5      $
��145* �6   �� 5      $
��147* �6   .+�5   
   #  MaterialDesignThemes.Wpf.Card  +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�5      $��32q�5      $��34,54,36,95q�6   $��Stretch=�6/   $��2��6K   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ EventListData��5      $��40,59,41,100q�5         CanUserSortColumns .    CanUserAddRows .     AutoGenerateColumns .  -  'MaterialDesignThemes.Wpf.DataGridAssist   CellPadding$ 13 8 8 8q�5      	   ColumnHeaderPadding$	 8q�5      $��2��69   
�������
   HorizontalGridLinesBrush
 6M   Z�    Columns 5      4  .System.Windows.Controls.DataGridCheckBoxColumn  5      1  +System.Windows.Controls.DataGridBoundColumn   Binding 6)   �� *CheckBoxColumnN+   ClipboardContentBinding 6M   Z� 1  +MaterialDesignThemes.Wpf.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Width5	  /System.Windows.Controls.DataGridLengthConverter$ *	 64      Header$ 订阅名称��5       5  1MaterialDesignDataGridTextColumnPopupEditingStyle   EditingElementStyle# [  5        5      �� *SubscriptionType+5         5!      $ *	 64   $ 事件名称��5#      # [  5$       5"      �� *	EventType+5$        5%      $ *	 64   $ 事件代码��5'      # [  5(       5&      �� *	EventCode+5(        5*      $ *	 64   $ 回调地址��5,      # [  5-       5+      �� *
TokenUrl+5-        5/      $ *	 64   $ 订阅状态��51      # [  52       50      �� *SubscriptionStatus+52      55      � 56   
   -   5         �Name$ CardN��56       � IsIndeterminate . $��2��57      $��
35,0,38,95q�6.   $��6��6o   $��Bottom=�6z   ���	#CC673AB7��6B   Z� ��6Y   Z� 6�   � 58   
   $��	0,0,351,0q�6   $��Right=�6"   $	��124��6>   $��1��6J   .+�59      �� -   5         ��Name$ 	Subscribe��59        MaterialDesignRaisedButton#��[ 5:      3
  -MaterialDesignThemes.Wpf.ButtonProgressAssist 
  Value$ -1Q�5;       
  IsIndicatorVisible .  
  IsIndeterminate . $��订阅全部事件��5>      $��2,0,2,28q�5?      $��Bottom=�6#   {�.  6S   5@      � 5A   
   $��	326,0,0,0q�6   $
��Left=�6"   $	��124��6=   .+�5B      �� -   5        $ TestB��5B      #��[ 5C      $ -1Q�5D       .  . $��取消全部订阅��5G      $��2,0,2,28q�5H      $��Bottom=�6#   {�.  6P   5I      �� 5J   
   -   5         ��Name$ TokenUrl��5J      $��1��6$   $
��Left=�64   $��25��6O   $��
235,0,0,29q�6[    �� TextWrapping$
 Wrap=�6o   $�� ��6�   $��Bottom=�6�   $	��205��6�   �� 5K   
   -   5         ��Name$ label��5K      $��设置回调地址：��6   $��1��61   $
��Left=�6A   $��
145,0,0,24q�6\   $��Bottom=�6p   � 5L   
   $��	0,0,311,0q�6   $��Right=�6"   $	��124��6>   .+�5M      �� -   5        $ SubscriptionSelection��5M      #��[ 5N      $ -1Q�5O       .  . $��订阅选中事件��5R      $��2,0,2,28q�5S      $��Bottom=�6#   {�.  6b   5T      � 5U   
   $��	0,0,161,0q�6   $��Right=�6"   $	��124��6>   .+�5V      �� -   5        $ Unsubscribe��5V      #��[ 5W      $ -1Q�5X       .  . $��取消选中订阅��5[      $��2,0,2,28q�5\      $��Bottom=�6#   {�.  6X   5]        5^   
   $ Depth2=�5_      $��32q�5`      $��	36,10,0,0q�6   $	��Top=�6-   $
��Left=�6E   $	��283��6`   $��25��6l   �� 5a   
   -	   5        $ textBox��5a      $
��Left=�6$   $��25��6?   $��	37,10,0,0q�6K   $
 Wrap=�6^   $	��Top=�6r   $	��281��6�   ���	#FF007ACC���	#FF999999$��搜索事件名称��6   �� 
CaretBrush
 �   ���6�   Z� 6#  '  !MaterialDesignThemes.Wpf.PackIcon  5b   
      Kind$ Search=�6"   $��
300,15,0,0q�60   �� 5c   
   -
   5        $ SearchBT��5c       " MaterialDesignFlatDarkBgButton#��[ 5d       � ToolTip$ 搜索��5e      $��
295,11,0,0q�6   $��23��6&   $	��Top=�6H   $
��Left=�6`   $��24��6{   ��62   Z� ��6�   Z� 6�   5g      5h      