   M S B A M L   `   `   `  ����J  FDeploymentTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:DeploymentTools.ViewDeploymentTools.View  5   	   (    "DeploymentTools.View.LnstallWEBAPI   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:DeploymentTools.View     5   	   S OMaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          � Title$  安装集成环境��5
   	   $	��472��6   $	��815��6%   .��5      � $��519,0,-0.2,0.4q�6   .+�5   
   � $��
-515,0,0,0q�6   .+�5      � -   5         �Name$ ParasiticSettings��5      $��0,-3,-0.2,3.4q�6.   $��2��6E   .+�5      �� -   5         ��Name$ portTxt��5   D    �� IsInputMethodEnabled .  $
��Left=�6U    �� TextWrapping$
 Wrap=�6p   $��77��6�   
����������	#FF007ACC���	#FF2C2C2C$��80��6�   $��25��6�   $��Bottom=�6�    �� SelectionOpacity$ 0Q�6   J� RenderTransformOrigin$ 0,0.5&�6  $��346,0,0,258q�66  6K  �� 5      -   5         ��Name$ label_Copy4��5      $��端口:��6-   $
��Left=�6;   $��307,0,0,258q�6V   $��Bottom=�6k   $��46��6�   �� 5      -   5        $ DatabaseAddress��5      $
��Left=�64   $
 Wrap=�6O   $	��460��6c   
����������	#FF007ACC���	#FF2C2C2C$�� ��6�   $��25��6�   $��Bottom=�6�   $ 0Q�6�   $ 0,0.5&�6�   $��	342,0,0,1q�6  �� 5      -   5        $ label_Copy5��5      $��数据库地址:��6-   $
��Left=�6>   $��	266,0,0,1q�6Y   $��Bottom=�6l   $��71��6�   �� 5      -   5        $��生成lic文件��5      $��311,0,0,170.6q�6+   $��Bottom=�6B   ���	#FF474747$��30��6�   $
��Left=�6�   $	��120��6�   ��6v   Z� 6�   �� 5      -   5        $ SQLTXTPATH_Copy��5      $
��Left=�64   $
 Wrap=�6O   $	��155��6c   
����������	#FF007ACC���	#FF2C2C2C$��postgres��6�   $��25��6�   $��Bottom=�6�   $ 0Q�6�   $ 0,0.5&�6�   $��140,0,0,132.6q�6  63  �� 5      -	   5        $ label_Copy6��5      $��用户   名称:��6-   $
��Left=�6@   $��64,0,0,132.6q�6[   $��Bottom=�6q   $��73��6�   5      �� 5      -
   5         ��Name$ comboBox1_Copy��5      $��140,0,510,259q�6/   $��Bottom=�6F   $��27��6a   	 �� 
IsEditable	 . $��2��6   6�   �� 5      -   5        $ label��5      $��服务器   IP:��6#   $
��Left=�67   $��63,0,0,257.4q�6R   $��Bottom=�6h   $��70��6�   �� 5      -   5        $ 
SQLTXTPATH��5      $
��Left=�6+   $
 Wrap=�6F   $	��155��6Z   
����������	#FF007ACC���	#FF2C2C2C$��aimap��6�   $��25��6�   $��Bottom=�6�   $ 0Q�6�   $ 0,0.5&�6�   $��140,0,0,213q�6  $��2��6%  69  �� 5      -   5        $ label_Copy1��5      $��数据库名称:��6)   $
��Left=�6:   $��
62,0,0,215q�6U   $��Bottom=�6i   $��73��6�   �� 5      -   5        $ SQLTXTPATH_Copy1��5      $
��Left=�61   $
 Wrap=�6L   $	��155��6`   
����������	#FF007ACC���	#FF2C2C2C$��tyaimap��6�   $��25��6�   $��Bottom=�6�   $ 0Q�6�   $ 0,0.5&�6�   $��139,0,0,175q�6  $��2��6-  6A  �� 5      -   5        $ label_Copy2��5      $��数据库密码:��6)   $
��Left=�6:   $��
63,0,0,175q�6U   $��Bottom=�6i   $��73��6�   �� 5      -   5        
 ��Name$
 button��5      $��测试连接��6%   $
��Left=�64   $��310,0,0,213q�6O   $��Bottom=�6d   $	��120��6   ���	#FF474747$��30��6�   ��6�   Z� 6�   � 5!      -   5        $ control��5!      $	��100��6$   $��-3,0,-0.4,0.4q�61   $��Bottom=�6H   .+�5"      d� -   5         d�Name$ Toggle��5"       (  $MaterialDesignSwitchDarkToggleButton#��[  5#       � ToolTip$' !数据库是否在其他服务器��5$      $��-6,0,0,-14.6q�6'   $
��Left=�6=   $	��260��6X   $��59��6d   $��Bottom=�6p   $��2��6�   6�   �� 5%      -   5        $ 
label_Copy��5%      $��数据库是否为远程��6,   $
��Left=�6?   $��	0,0,0,3.4q�6Z   $��Bottom=�6m   $��24��6�   $	��106��6�   d� 5&      -   5        $ Toggle_Copy��5&      #��[  5'      $ 调试模式��5(      $��98,0,0,-13.6q�6    $
��Left=�66   $	��260��6Q   $��59��6]   $��Bottom=�6i   6�   �� 5)      -   5        $ label_Copy3��5)      $��调试模式��6-   $
��Left=�6<   $��149,0,0,3.4q�6W   $��Bottom=�6l   $��24��6�   $��58��6�   5+      � 5.      -   5        $ progressBar��5.      $	��100��6(   $��-3,0,-0.4,-50.6q�65   $��Bottom=�6N    � ColumnDefinitions 5/      �� 50      .+�52      � -   5         �Name$ UnzipProgressBar_Total��52      $��17��6{   $	��Top=�6�   $��0,23,0,0q�6�   ���	#FF80AC70 � Maximum 6?   �� *
TotalCoutn+ � Value 6^   �� *
TotalIndex+6�   � 53      -   5        $ UnzipProgressBar_File��53      $��12��6w   $��0,37,0,0q�6�   $	��Top=�6�   ���	#FF6443B3 6=   �� *	FileCoutn+ 6[   �� *	FileIndex+6�   54      56      #  MaterialDesignThemes.Wpf.Card  58   
   -   5          Name$ CardMess_Copy1��58      +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�59      $��32q�5:      $��25,42,22,43q�6   $��Stretch=�6/   ���	#FF2525266K   �� 5;   
   -   5        $
 CurrentCatalogue��5;      $��安装目录��6+   $��
0,0,28,109q�6:   ���	#FF016B6E$��25��6|   $��10(�6�   ���	#DDFFFFFF$��Bottom=�6�   $��Right=�6�   $��75��6  ��6N   Z� 6  �� 5<   
   -   5        $ LabelPathcss��5<   5   $��Center=�6   $��安装文件目录名称��6K   $
��Left=�6^   $��31,0,0,108.6q�6y   $��Bottom=�6�   $	��114��6�   $ 0.5,0.5&�6�   ���	#F2FFFFFF���	#FF0F0F0F6�   �� 5=   
   -   5        $
 SynchronizationButton��5=        MaterialDesignRaisedButton#��[ 5>      3  -MaterialDesignThemes.Wpf.ButtonProgressAssist   Value$ -1Q�5?         IsIndicatorVisible .    IsIndeterminate . $��安装��5B      $��
80,0,72,51q�5C      $��Bottom=�6!   $��30��6R   �� 5D   
   -   5        $ 	comboBox1��5D      $��72,0,60,341q�6&   $��Bottom=�6;   $��27��6V   �� 5E   
   -   5        $ textport��5E      $
��Left=�6$   $��23��6?   $��25,0,0,7q�6K   $
 Wrap=�6]   $
��8080��6q   $��Bottom=�6}   $��50��6�   $y�Hidden=�6�   5F      5G      