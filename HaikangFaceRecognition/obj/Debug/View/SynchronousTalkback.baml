   M S B A M L   `   `   `  ����J  FDeploymentTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:DeploymentTools.ViewDeploymentTools.View  5   	   .    (DeploymentTools.View.SynchronousTalkback   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:DeploymentTools.View     5   	   S OMaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$,  &SynchronousTalkback-同步对讲设备��5	   	   $	��450��6,   $	��800��69   .��5
      � .+�5   
   #  MaterialDesignThemes.Wpf.Card  -   5          Name$ CardMess��5      +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�5   	   $��32q�5   	   $��185,15,199.6,94q�6   $��Stretch=�6/   $�� ��6K   ���	#FF2525266V   � 5   
   $��295,0,303.6,-5q�6   $	��419��6'   $��Bottom=�64   .+�5      �� -   5         ��Name$ SynchronizationButton��5         MaterialDesignRaisedButton#��[  5      3  -MaterialDesignThemes.Wpf.ButtonProgressAssist   Value$ -1Q�5         IsIndicatorVisible .    IsIndeterminate . $��同步数据��5      $��9,0,9,29q�5      $��Bottom=�6#   6>   5      �� 5   
   -   5         ��Name$ 	listView1��5      $��185,20,199.6,99q�6&   .@�5      �� $��Hello��6   �� 5      $��World��6   �� 5      $��:)��6   5      �� 5   
   -   5         ��Name$ 	comboBox1��5      $��0,0,24.6,63q�6&   $��Bottom=�6;   $��Right=�6V   $	��120��6r   $��27��6~   �� 5   
   -   5        $ 	comboBox2��5      $��Right=�6&   $��0,0,23.6,25q�6B   $��Bottom=�6W   $	��120��6r   $��27��6~   �� 5    
   -   5        	 ��Name$		 zho��5       $��设备总数：��6   $
��Left=�6-   $��
182,0,0,64q�6H   $��Bottom=�6\   $	��108��6w   d� 5!   
   -   5        
 d�Name$
 SPCCSelection��5!       ( $MaterialDesignSwitchDarkToggleButton#��[ 5"   	    � ToolTip$ 是否为SPCC平台��5#   	   $��	470,0,0,9q�6   $
��Left=�60   $	��260��6K   $��59��6W   $��Bottom=�6c   6~   �� 5$   
   -   5        $	 label_Copy1��5$      $��平台是否为SPCC��6%   $
��Left=�69   $��
484,0,0,27q�6T   $��Bottom=�6h   $��24��6�   $	��106��6�   5&      5'      