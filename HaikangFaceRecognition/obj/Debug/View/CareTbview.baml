   M S B A M L   `   `   `  ����J  FDeploymentTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:DeploymentTools.ViewDeploymentTools.View  5   	   %    DeploymentTools.View.CareTbview   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:DeploymentTools.View     5   	   S OMaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   -   5          � Title$  
CareTbview��5	   	   $	��450��6   $	��800��6)   .��5
      �  � ColumnDefinitions 5   
   �� 5      $
��343* �6   �� 5      $	��50* �6   �� 5      $
��399* �6    � RowDefinitions 5   
   �� 5      $
��135* �6   �� 5      $	��15* �6   .+�5   
   � -   5        $��Probar��5       � Value$ 25Q�5      $
��Auto��5       � IsIndeterminate . $��0,134,0,187q�6(   $��3��6=   $y�Hidden=�6Q   ���	#FFBDBBC3���	#FF57565B���	#FFE1DDE86�   �� 5   
   -   5         ��Name$ base_Txt��5      $
��Left=�6$   $��25��6?   $��133,0,0,300q�6K    �� TextWrapping$
 Wrap=�6`   $�� ��6t   $��Bottom=�6|   $	��205��6�   �� 5   
   -   5         ��Name$ label��5      $��数据库名：��6   $
��Left=�6/   $��
43,0,0,295q�6J   $��Bottom=�6^   �� 5   
   -   5        $ PC_Txt��5      $
��Left=�6"   $��25��6=   $��133,0,0,254q�6I   $
 Wrap=�6^   $��	localhost��6r   $��Bottom=�6�   $	��205��6�   �� 5   
   -   5        $ 
label_Copy��5      $��	主机：��6$   $
��Left=�62   $��
43,0,0,249q�6M   $��Bottom=�6a   �� 5   
   -   5        $ Post_Txt��5      $
��Left=�6$   $��25��6?   $��133,0,0,192q�6K   $
 Wrap=�6`   $
��5432��6t   $��Bottom=�6�   $	��205��6�   �� 5   
   -   5        $ label_Copy1��5      $��	端口：��6%   $
��Left=�63   $��
43,0,0,187q�6N   $��Bottom=�6b   �� 5   
   -	   5        $ User_Txt��5      $
��Left=�6$   $��25��6?   $��133,0,0,131q�6K   $
 Wrap=�6`   $��postgres��6t   $��Bottom=�6�   $	��205��6�   �� 5    
   -
   5        $ label_Copy2��5       $��用户名：��6%   $
��Left=�64   $��
43,0,0,126q�6O   $��Bottom=�6c    J� RenderTransformOrigin$ -4.444,-7.606&�6~   �� 5!   
   -   5        $ Pwd_Txt��5!      $
��Left=�6#   $��25��6>   $��
133,0,0,64q�6J   $
 Wrap=�6^   $�� ��6r   $��Bottom=�6z   $	��205��6�   �� 5"   
   -   5        $ label_Copy3��5"      $��	密码：��6%   $
��Left=�63   $��	43,0,0,59q�6N   $��Bottom=�6a   $ -4.444,-7.606&�6|   �� 5$   
   -   5        	 ��Name$ 	 SubscriptionSelection_Copy��5$         MaterialDesignRaisedButton#��[  5%      3  -MaterialDesignThemes.Wpf.ButtonProgressAssist
   Value$
 -1Q�5&         IsIndicatorVisible .    IsIndeterminate . $��请稍后。。��5)      $��10,0,211.254,10q�5*      $��Bottom=�6&   {�. $ 0.087,2.483&�6v   $��1��6�   �� 5+   
   -   5        $	 SubscriptionSelectionSql��5+      #��[  5,      $
 -1Q�5-       .  . $��测试连接��50      $��316,0,294,11q�51      $��Bottom=�6#   {�. $ 0.087,2.483&�6v   $��3��6�   $��2��6�   �� 52   
   -   5        $	 SubscriptionSelectionFild��52      #��[  53      $
 -1Q�54      $��选择文件夹��56      $��277,0,10,340q�57      $��Bottom=�6#   {�. $ 0.087,2.483&�6w   $��2��6�   $��27��6�   �� 58   
   $��2��6   R� 59   
   -   5         R�Name$ treeView��59      $��
1,30,-1,32q�6%   $��2��69   5<      �� 5@   
   -   5        $"	 SubscriptionSelectionSqlCome��5@      #��[  5A      $
 -1Q�5B      $��数据入库��5C      $��261,0,13,11q�5D      $��Bottom=�6"   {�. $ 0.087,2.483&�6P   $��2��6t   $��2��6�   �� 5I   
   -   5        $	 SubscriptionSelection��5I      #��[  5J      $
 -1Q�5K      $��创建��5L      $��10,0,211.254,10q�5M      $��Bottom=�6&   {�. $ 0.087,2.483&�6v   $��1��6�   �� 5O   
   -   5        $��替换��5O      $
��Left=�6   $��
121,0,0,11q�69   $��Bottom=�6M   $	��126��6h   $��2��6�   $��2��6�   5Q      5R      