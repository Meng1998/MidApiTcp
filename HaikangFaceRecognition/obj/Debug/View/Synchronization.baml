   M S B A M L   `   `   `  ����J  FDeploymentTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:DeploymentTools.ViewDeploymentTools.View  5   	   *    $DeploymentTools.View.Synchronization   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:DeploymentTools.View     5   	   S OMaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          � Title$  同步数据��5	   	   $	��477��6   $	��869��6#    � Icon$@ :/DeploymentTools;component/Resources/VideoShootingLogo.ico��6H   .��5
      � -   5         �Name$ GridName��5
       � ColumnDefinitions 5   
   �� 5      $
��425* �6   �� 5      $
��436* �6   .+�5   
   #  MaterialDesignThemes.Wpf.Card  -   5          Name$ CardMess_Copy��5      +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�5      $��32q�5      $��15,35,15,101q�6   $��Stretch=�60   $�� ��6L   ���	#FF252526$��1��6n     5   
   -   5        $ CardMess��5      $ Depth2=�5      $��32q�5      $��12,35,5,101q�6   $��Stretch=�6/   $�� ��6K   ���	#FF2525266V   d� 5   
   -   5         d�Name$ ofRegionStateTiggleBT��5       (  $MaterialDesignSwitchDarkToggleButton#��[  5       � ToolTip$0 *设置是否同步数据库的设备信息��5       d� 	IsChecked$
 TrueY�5      $��	20,0,0,-1q�6   $
��Left=�6'   $	��260��6B   $��59��6N   $��Bottom=�6Z   6u   �� 5   
   -   5        	 ��Name$	 label��5      $��是否同步设备结构��6   $
��Left=�62   $��	12,0,0,17q�6M   $��Bottom=�6`   $��25��6{   $	��106��6�   d� 5   
   -   5        $ ofEquipmentStateTiggleBT��5      #��[  5      $- '设置是否同步数据的区域信息��5      $
 TrueY�5      $��	20,0,0,31q�6   $
��Left=�61   $	��260��6L   $��59��6X   $��Bottom=�6d   6   �� 5   
   -   5        $	 
label_Copy��5      $��是否同步组织结构��6$   $
��Left=�67   $��	12,0,0,49q�6R   $��Bottom=�6e   $��25��6�   $	��106��6�   �� 5   
   -	   5        
 ��Name$
 n��5         MaterialDesignLightSeparator#��[ 5       $��16��6C   $��
-20,0,0,36q�6O   ���	#1F2E2E2E$��Bottom=�6z   $
��Left=�6�   $	��184��6�   � 5!   
   -
   5         �Name$ Load_animation��5!       � Value$ 0Q�5"      ;  5MaterialDesignThemes.Wpf.Transitions.TransitionAssist   DisableTransitions . $��Bottom=�5#   =   $��2��6X   � 5%   
   $��336,0,332,0q�6   $	��419��6$   $��Bottom=�61   $��2��6L   .+�5&      �� -   5         ��Name$ SynchronizationButton��5&        MaterialDesignRaisedButton#��[ 5'      3  -MaterialDesignThemes.Wpf.ButtonProgressAssist   Value$ -1Q�5(         IsIndicatorVisible .    IsIndeterminate . $��同步数据��5+      $��9,0,9,29q�5,      $��Bottom=�6   62   5-      �� 5.   
   -   5        $	 label1��5.      $��设备数据��6    $��
172,11,0,0q�6/   $	��Top=�6C   ���	#FF191919$��25��6r   $
��Left=�6~   $��58��6�   �� 5/   
   -   5        $	 label1_Copy��5/      $��区域数据��6%   $��
0,11,192,0q�64   ���	#FF151515$��25��6_   $	��Top=�6k   $��1��6�   $��Right=�6�   $��58��6�   �� 50   
   -   5         ��Name$ 	comboBox1��50      $��	0,0,34,56q�6&   $��Bottom=�69   $��Right=�6T   $	��120��6p   $��1��6|   $��27��6�   �� 51   
   -   5        $ 	comboBox2��51      $��Right=�6&   $��	0,0,33,18q�6B   $��Bottom=�6U   $	��120��6p   $��1��6|   $��27��6�   �� 52   
   -   5         ��Name$ 	listView1��52      $��12,40,5,106q�6&   .@�53      �� $��Hello��6   �� 54      $��World��6   �� 55      $��:)��6   56      �� 57   
   -   5        $ 	listView2��57      $��15,40,15,106q�6&   $��1��6<   .@�58      �� $��Hello��6   �� 59      $��World��6   �� 5:      $��:)��6   5;      � 5<   
   -   5        $ CardN��5<       � IsIndeterminate . $��2��5=   $   $��4��6e   $��Bottom=�6p   ���	#CA673AB7��68   Z� ��6O   Z� 6�   d� 5>   
   -   5        $ SPCCSelection��5>      #��[  5?      $ 是否为SPCC平台��5@      $��	0,0,67,15q�6!   $��Right=�64   $	��260��6P   $��59��6\   $��Bottom=�6h   $��1��6�   �� 5A   
   -   5        $	 label_Copy1��5A      $��平台是否为SPCC��6%   $��Right=�69   $��
0,0,211,32q�6U   $��Bottom=�6i   $��25��6�   $	��106��6�   $��1��6�   �� 5B   
   -   5        $ button��5B      $��导出设备��6!   $
��Left=�60   $��
220,0,0,14q�6K   $��Bottom=�6_   $	��105��6z   6�   �� 5C   
   -   5        $ button_Copy��5C      $��导出表格��6&   $
��Left=�65   $��
220,0,0,54q�6P   $��Bottom=�6d   $	��105��6   6�   5D      5E      