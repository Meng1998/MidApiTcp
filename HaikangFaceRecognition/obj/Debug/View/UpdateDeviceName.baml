   M S B A M L   `   `   `  ����J  FDeploymentTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:DeploymentTools.ViewDeploymentTools.View  5   	   +    %DeploymentTools.View.UpdateDeviceName   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:DeploymentTools.View     5   	   S OMaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  UpdateDeviceName��5	   	   $	��450��6"   $	��800��6/   .��5
      � .+�5   
   #  MaterialDesignThemes.Wpf.Card  +  %MaterialDesignThemes.Wpf.ShadowAssist   ShadowDepth$ Depth2=�5      $��32q�5      $��34,23,33,236q�6   $��Stretch=�60   �� 5   
   -   5         ��Name$ But_Toupdate��5      $��开始更新名称��6'   $
��Left=�68   $��
320,0,0,10q�6S   $��Bottom=�6g   $	��205��6�   6�   �� 5   
   -   5        $ 
But_Import��5      $��导入��6%   $
��Left=�62   $��
240,0,0,10q�6M   $��Bottom=�6a   $��75��6|   ���	#FF3A79B7���	#FF3A79B76�   �� 5   
   -   5         ��Name$ label��5      $!��名称与库不匹配数据��6   $
��Left=�63   $��33,0,0,0q�6N   $	��Top=�6`   $	��128��6x   �� 5   
   -   5         ��Name$ 	listView1��5      $��38,26,37,239q�6&    �� View 5      �� 5      .P�5      �� $��监控点名称��6%    �� Width$	 200��64   -   5        Name�� �� DisplayMemberBinding 5   N   �� *Name+�� 5      $��监控点编号��6%   $	 400��64   -   5        Code�� 5   N   �� *Code+5      5        5   
   $ Depth2=�5      $��32q�5      $��
32,0,35,57q�6   $��Bottom=�6.   $	��160��6I   �� 5   
   -   5        $ listView1_Copy��5      $��
36,0,39,60q�6+   $	��154��6?   $��Bottom=�6L    5      �� 5       .P�5!      �� $��监控点名称��6%   $	 200��64   -   5        	Name1�� 5!   O   �� *Name+�� 5"      $��监控点编号��6%   $	 400��64   -	   5        	Code1�� 5"   O   �� *Code+5#      5&      �� 5'   
   -
   5        $ 
label_Copy��5'      $!��未添加或未同步设备��6$   $
��Left=�68   $��
28,0,0,216q�6S   $��Bottom=�6g   $	��128��6�   5)      5*      