   M S B A M L   `   `   `  ����  ID3Classifier    ID3Project.TreeVis   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6F     � Title$  TreeVis��5   	   $	��638��6   $
��1425��6&    � 	Resources 5       �� MergedDictionaries �� Source �� DataType �� ItemsSource �� XPath ��Name �� CornerRadius	 �� Padding
 � ToolTip �� Triggers �� Value �� Binding �� 
TargetName �� Property �� Value�� 5   
    5      �� 5	      $ GraphStyle.xaml?�6%   %�  (��       5   '   *Node+)�� 6   $
 Node��6'    5      �� $ Children/Node��.R�5      �� $	 bdr��6   $ 20��6&   $��2q�68   
���   �$
��Auto��5      $��60��6(   $��50��64   $��2q�6B   $	 2q�6M   ._�5      �� 
 5      �� 5      .+�5   "   �� $
��Bold"�6,   �� 6?   $�Node Details��6C   6Z   �� 5   "   �� 6-   $�	Node Has ��61   �� 6D   .F�5   $   �� ��6.   �� $ 
@Instances��5   $   �� 67   $�
 Instances��6;   6P   5      .+�5      �� $��Center=�5      $��Center=�5      ��5   $   �� $ @Name��5      �� 5      $��Center=�6B   $��Center=�5      ��5   $   �� $ @Value��5      5      5        5!      �� 5"      $
 good��6C    6"   �� $ @Value��.a�5#      �� $	 bdr��6!    � ��
 ��  �6I   5$      �� 5%      $	 bad��6C    6"   �� $ @Value��.a�5&      �� $	 bdr��6!    � ��
 �  ��6I   5'      5)      5*      .��5,      �� $��0,0,0,0q�6   .+�5-   
   �� $��Center=�6   $
��Bold"�61   $��20(�6C   �� 6R   $�Tree Visualization��6V   6s   �� 5.   
   $��Center=�6   $
��Bold"�61   $��16(�6C   �� 6R   $q�kEach Node Has 2 rows of data: Upper - Value of parent split attribute, Lower - Current Node Split Attribute��6V   6�   R� 5/   
   -   5         R�Name$ tv_Tree��5/         
GraphStyle#��[  6$   ��50      �� 51      52      