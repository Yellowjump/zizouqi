using SkillSystem;
using UnityEditor;
using UnityEngine;

namespace Editor.SkillSystem
{
    [SkillDrawer(typeof(OneTrigger))]
    public class OneTriggerEditor
    {
        public static void OnGUIDraw(OneTrigger oneTrigger)
        {
            if (oneTrigger !=null)
            {
                oneTrigger.CurTriggerType =(TriggerType)EditorGUILayout.EnumPopup("触发类型",oneTrigger.CurTriggerType);
                var oldConditionType = oneTrigger.CurCondition.CurConditionType;
                var newConditionType = (ConditionType)EditorGUILayout.EnumPopup("触发条件",oneTrigger.CurCondition.CurConditionType);
                if (newConditionType != oldConditionType)
                {
                    oneTrigger.CurCondition = SkillFactory.CreateCondition(newConditionType);
                }
                SkillSystemDrawerCenter.DrawOneInstance(oneTrigger.CurCondition);
                var oldTargetPickerType = oneTrigger.CurTargetPicker.CurTargetPickerType;
                var newTargetPickerType = (TargetPickerType)EditorGUILayout.EnumPopup("目标选择",oneTrigger.CurTargetPicker.CurTargetPickerType);
                if (oldTargetPickerType != newTargetPickerType)
                {
                    oneTrigger.CurTargetPicker = SkillFactory.CreateTargetPicker(newTargetPickerType);
                }
                SkillSystemDrawerCenter.DrawOneInstance(oneTrigger.CurTargetPicker);
                if (GUILayout.Button("添加Command"))
                {
                    oneTrigger.CurCommandList.Add(SkillFactory.CreateCommand(CommandType.CauseDamage));
                }

                if (oneTrigger.CurCommandList != null && oneTrigger.CurCommandList.Count != 0)
                {
                    EditorGUILayout.BeginVertical();
                    
                    for (var commandIndex = 0; commandIndex < oneTrigger.CurCommandList.Count; commandIndex++)
                    {
                        var oneCommand = oneTrigger.CurCommandList[commandIndex];
                        EditorGUILayout.BeginHorizontal();
                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            oneTrigger.CurCommandList.Remove(oneCommand);
                        }
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        
                        // 绘制边界线
                        Rect rect = GUILayoutUtility.GetRect(0, 0);
                        EditorGUI.DrawRect(new Rect(rect.x - 2, rect.y, 2, rect.height), Color.black);
                        var oldCommandType = oneCommand.CurCommandType;
                        var newCommandType =
                            (CommandType)EditorGUILayout.EnumPopup("Command类型", oneCommand.CurCommandType);
                        if (oldCommandType != newCommandType)
                        {
                            oneTrigger.CurCommandList[commandIndex] = SkillFactory.CreateCommand(newCommandType);
                        }
                        SkillSystemDrawerCenter.DrawOneInstance(oneCommand);
                        EditorGUILayout.EndVertical();
                        GUILayout.Space(5);
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUILayout.EndVertical();
                }
                
            }
        }
    }
}