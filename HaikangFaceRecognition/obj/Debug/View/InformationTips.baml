   M S B A M L   `   `   `  ����J  FDeploymentTools, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:DeploymentTools.ViewDeploymentTools.View  5   	   *    $DeploymentTools.View.InformationTips   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:DeploymentTools.View     5   	   S OMaterialDesignThemes.Wpf, Version=1.0.1.0, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  InformationTips��5	   	   $	��315��6!   $	��445��6.    � WindowStyle$
 None=�6:    � AllowsTransparency . ��6M   Z� .��5
      � .+�5   
   �� $��	6,13,9,29q�6    J� RenderTransformOrigin$ 0.5,0.5&�6$   z�5      V� 5      .~�5      ��  �� ScaleY$ -1Q�6%   �� 5      �� 5       �� Angle$ 0Q�6&   T� 5      5       J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5         
BlurRadius$ 13Q�6#      ShadowDepth$ 0Q�63   	   Opacity$		 0.5Q�6C   ._�5      �� -   5        
 ��Name$
 
FaceLogin2��5      $��3��6*   $��2��67   $��	10,0,0,13q�6G   $ 0.5,0.5&�6Z   
����������	#FF303030
�������$	��251��6�   $
��Left=�6�   $	��410��6�   $��Bottom=�6�    � Cursor$ Arrow��6  z�5      V� 5      .~�5      �� $ -1Q�6)   �� 5      �� 5      T� 5      5      ��5       �� 5!      .R�5"      �� #��z��6!   $��2q�6M    �� CornerRadius$ 4,4,4,4��6a   
�������._�5#      �� $��Center=�60   $��Center=�6M   5$      5%      5'      5(      �� 5)   
   $ 0,5,0,5��6   $��1q�6(   $
��Left=�6<   $��20��6W   $��
399,25,0,0q�6c   $	��Top=�6w   $��27��6�   ���	#FFFF72726�   '  !MaterialDesignThemes.Wpf.PackIcon  5*   
      Kind$ WindowClose=�6"   $��
405,28,0,0q�65   
�������$��13��6\   $��17��6h   �� 5+   
   -   5         ��Name$ 
LabelTitle��5+      $��信息标题��6$   $
��Left=�63   $��	16,25,0,0q�6N   $	��Top=�6a   $��30��6y   $��11(�6�   $	��384��6�   ���	#DA000000$��Perpetua Titling MT)�6�   6�   �� 5,   
   -   5         ��Name$ Content��5,   i    �� TextAlignment$ Center=�6   $��Center=�61   $��Center=�6L   $
��Wrap=�6z   $s��m在此输入要换行的文字，前提是它足够长，并且Label的MaxWidth不足以让它一行显示。��5-      $��48,90,57,65q�66   
���   �$��Perpetua Titling MT)�6^   $��14(�6   �� 5.   
   -   5        $
 button��5.      $��Button��6!   $
��Left=�62   $��
401,25,0,0q�6M   $	��Top=�6a   $��25��6y   $��20��6�   ��6�   Z� ��6�   Z� 6�   5/      50      