   M S B A M L   `   `   `  ����J  FDeploymentTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:DeploymentTools.ViewDeploymentTools.View  5   	   .    (DeploymentTools.View.ZoneSynchronization   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:DeploymentTools.View     5   	   S OMaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$,  &ZoneSynchronization-同步防区信息��5	   	   $	��450��6,   $	��800��69   .��5
      � .+�5   
   #  MaterialDesignThemes.Wpf.Card  -   5          Name$ CardMess��5      +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�5   	   $��32q�5   	   $��322,14,63,95q�6   $��Stretch=�6,   $�� ��6H   ���	#FF2525266S   � 5   
   $��295,0,303.6,-5q�6   $	��419��6'   $��Bottom=�64   .+�5      �� -   5         ��Name$ SynchronizationButton��5         MaterialDesignRaisedButton#��[  5      3  -MaterialDesignThemes.Wpf.ButtonProgressAssist   Value$ -1Q�5         IsIndicatorVisible .    IsIndeterminate . $��暂不支持4.0��5      $��
9,0,9.4,32q�5      $��Bottom=�6%   {�.  6U   5      �� 5   
   -   5         ��Name$ 	listView1��5      $��322,19,63,100q�6&   .@�5      �� $��Hello��6   �� 5      $��World��6   �� 5      $��:)��6   5      �� 5   
   -   5         ��Name$ 	comboBox1��5      $��0,0,24.6,63q�6&   $��Bottom=�6;   $��Right=�6V   $	��120��6r   $��27��6~   �� 5   
   -   5        $ 	comboBox2��5      $��Right=�6&   $��0,0,23.6,25q�6B   $��Bottom=�6W   $	��120��6r   $��27��6~   �� 5    
   -   5        	 ��Name$		 zho��5       $��设备总数：��6   $
��Left=�6-   $��182,332,0,0q�6H   $	��Top=�6]   $	��108��6u   d� 5!   
   -   5        
 d�Name$
 SPCCSelection��5!       ( $MaterialDesignSwitchDarkToggleButton#��[ 5"       � ToolTip$ 是否为SPCC平台��5#      $��	470,0,0,9q�6!   $
��Left=�64   $	��260��6O   $��59��6[   $��Bottom=�6g   6�   �� 5$   
   -   5        $	 label_Copy1��5$      $��平台是否为SPCC��6%   $
��Left=�69   $��
484,0,0,27q�6T   $��Bottom=�6h   $��24��6�   $	��106��6�   �� 5%   
   -	   5        $ button��5%      $��导出设备��6!   $
��Left=�60   $��	35,0,0,12q�6K   $��Bottom=�6^   $	��105��6y   {�.  6�   �� 5&   
   -
   5        $ button_Copy��5&      $��导出表格��6&   $
��Left=�65   $��	35,0,0,52q�6P   $��Bottom=�6c   $	��105��6~   6�     5'   
   -   5        $ CardMess_Copy��5'      $ Depth2=�5(      $��32q�5)      $��65,15,481,94q�6   $��Stretch=�60   $�� ��6L   ���	#FF2525266W   �� 5*   
   -   5        $ listView1_Copy��5*      $��66,20,482,99q�6+   .@�5+      �� $��Hello��6   �� 5,      $��World��6   �� 5-      $��:)��6   5.      50      51      